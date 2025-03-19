using UnityEngine;
using System.Net;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;
using System.Linq;

public class TCP : MonoBehaviour
{
    public TextUpdater displayReward;
    public BlueCarHandler blueCarHandler;
    public VisualFb visualFb;
    private string HOST;
    private int PORT;
    private TcpListener server;
    private TcpClient client;
    private NetworkStream netStream;
    public static int action = 0;

    public string receivedData = string.Empty;
    private float[] sentData;
    public UnityEngine.UI.Text display;
    public static Vector3 position;
    private Vector3 rotation;
    private DateTime startTime;
    private int tempRew = 10;
    private volatile bool isRunning = true;
    private static readonly object scoreLock = new object();

    async void Start()
    {
        HOST = "192.168.0.105";
        PORT = 12345;
        await StartServerAsync();
    }

    void Update()
    {
        position = transform.position;
        rotation = transform.eulerAngles;
    }

    private async Task StartServerAsync()
    {
        try
        {
            server = new TcpListener(IPAddress.Parse(HOST), PORT);
            server.Start();
            Debug.Log($"Server started at {HOST}:{PORT}. Waiting for client...");
            client = await server.AcceptTcpClientAsync();
            netStream = client.GetStream();
            Debug.Log("Client connected.");
            startTime = DateTime.Now;
            _ = ReceiveDataAsync();
            _ = SendDataAsync();
        }
        catch (SocketException ex)
        {
            Debug.LogError($"SocketException: {ex.Message}");
            Cleanup();
        }
    }

    private async Task ReceiveDataAsync()
    {
        try
        {
            byte[] lengthBuffer = new byte[4];  // To store the length prefix (4 bytes)

            while (isRunning)
            {
                if (netStream != null && netStream.CanRead)
                {
                    // First, read the length prefix (4 bytes)
                    int lengthBytesRead = await netStream.ReadAsync(lengthBuffer, 0, lengthBuffer.Length);
                    if (lengthBytesRead < 4)
                    {
                        // If we can't read 4 bytes for length, break out or handle error
                        break;
                    }

                    // Convert the length prefix to an integer (big-endian)
                    int messageLength = BitConverter.ToInt32(lengthBuffer.Reverse().ToArray(), 0);

                    byte[] dataBuffer = new byte[messageLength];
                    int totalBytesRead = 0;

                    // Now read the actual JSON data in chunks
                    while (totalBytesRead < messageLength)
                    {
                        int bytesRead = await netStream.ReadAsync(dataBuffer, totalBytesRead, messageLength - totalBytesRead);
                        if (bytesRead == 0)
                        {
                            // Connection closed or error, handle accordingly
                            break;
                        }
                        totalBytesRead += bytesRead;
                    }

                    if (totalBytesRead == messageLength)
                    {
                        try
                        {
                            string jsonData = Encoding.UTF8.GetString(dataBuffer, 0, totalBytesRead);
                            var receivedJson = JsonConvert.DeserializeObject<ReceivedData>(jsonData);

                            
                            if (receivedJson != null)
                            {
                                
                                action = receivedJson.action;
                                //lock (scoreLock)
                                //{
                                // ScoreManager.scoreCount += receivedJson.scoreIncrement;
                                //}
                            }
                            Debug.Log($"Received JSON: {jsonData}");
                        }
                        catch (JsonException ex)
                        {
                            Debug.LogError($"JSON Decode Error: {ex.Message}");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error in ReceiveDataAsync: {ex.Message}");
        }
    }

    private async Task SendDataAsync()
    {
        //private int tempRew = 10;
        try
        {
            while (isRunning && netStream != null && netStream.CanWrite)
            {
                var dataToSend = new SentData
                {
                    timestamp = (float)Math.Round((DateTime.Now - startTime).TotalSeconds, 5),
                    currentRewardValue = displayReward.RLTotalReward,
                    mainCarPosition = displayReward.mainCarPositionCurrent,
                    distanceToBeast = visualFb.distanceToBlueCar,
                    isBeastChasing = blueCarHandler.isBlueCarChasing,
                    feedbackIntensity = visualFb.feedbackIntensity,
                    obstacleHitCount = displayReward.obstacleHitCountCurrent,
                    beastHitCount = displayReward.blueCarHitCountCurrent,
                    speedSignLimitExceedCount = displayReward.speedSignLimitExceedCountCurrent
                };

                string jsonData = JsonConvert.SerializeObject(dataToSend);
                byte[] jsonDataBytes = Encoding.UTF8.GetBytes(jsonData);

                byte[] lengthPrefix = BitConverter.GetBytes(jsonDataBytes.Length);
                Array.Reverse(lengthPrefix);
                await netStream.WriteAsync(lengthPrefix, 0, lengthPrefix.Length);


                
                await netStream.WriteAsync(jsonDataBytes, 0, jsonDataBytes.Length);
                Debug.Log($"Sent JSON: {jsonData}");
                Debug.Log($"length prefix: {lengthPrefix}");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"SendData Exception: {ex.Message}");
        }
    }

    private void Cleanup()
    {
        isRunning = false;
        netStream?.Close();
        client?.Close();
        server?.Stop();
        Debug.Log("Server stopped and resources cleaned up.");
    }

    private void OnDestroy()
    {
        Cleanup();
    }
}

[Serializable]
public class SentData
{
    public float timestamp;
    public int currentRewardValue = 0;
    public int mainCarPosition = 0;
    public float distanceToBeast = 0;
    public bool isBeastChasing = false;
    public float feedbackIntensity = 0;
    public int obstacleHitCount = 0;
    public int beastHitCount = 0;
    public int speedSignLimitExceedCount = 0;
}

[Serializable]
public class ReceivedData
{
    public int action;
}
