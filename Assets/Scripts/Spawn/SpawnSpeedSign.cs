using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;



public class SpawnSpeedSign : MonoBehaviour
{

    private int spawnDistanceMin = 27 * 10; // 27 
    private int spawnDistanceMax = 37 * 10; // 37
    private int spawnCameraSignoffsetDistanceMin = 5*10; // 50
    private int spawnCameraSignoffsetDistanceMax = 8*10; // 80
    

    public GameObject speedSignPrefab;
    //private SpeedSign speedSignScript;
    public GameObject speedCameraSignPrefab;
    public SpeedSign SpeedSign;
    public Transform mainCar;
    public GameObject mainCarr;
    Vector3 speedSignPos;
    Vector3 speedCameraSignPos;
    //float speedLimitt = 100;

    int xLeft = -1;
    int xRight = +1;
    public float spawnZ = 0;
    private float newSpawnZ = 0;

    private int speedLimit = 100;
    public static int speedSignLimitExceedCount = 0; // To count how many times the speed was exceeded
    private float timeAboveLimit = 0f; // Timer to track how long the car has been speeding
    private Rigidbody mainCarRb; // Rigidbody of the main car to track its velocity
    private float mainCarForwardSpeedMax = 0;
    private float mainCarForwardVelocity = 0;

    void Start()
    {
        CarHandler2 mainCarScript = mainCarr.GetComponent<CarHandler2>();
        mainCarForwardSpeedMax = mainCarScript.forwardSpeedMax;
        Debug.Log($"MainCarVelMax: {mainCarForwardSpeedMax}");

        SpeedLimitSignSpawner();

        // SpeedLimitSignSpawner();
    }
    void Update()
    {
        speedLimit = SpeedSign.speedLimit;
        if (isCarPassedSpeedCameraSign())
        {
            Debug.Log("Speed Camera sign Passed");
            SpeedLimitSignSpawner();
        }

    }
    void SpeedLimitSignSpawner()
    {
        float randomX = ((Random.Range(0, 2) * 2) - 1) * 1.2f; // A random value between (-1.2f, +1.2f) that is the location of the Speed sign

        float rnd = Random.Range(spawnDistanceMin, spawnDistanceMax);

        spawnZ = mainCar.position.z + rnd;

        speedSignPos = new Vector3(randomX, 1.18f, spawnZ);
        speedCameraSignPos = new Vector3(-speedSignPos.x, 0, speedSignPos.z + getSpeedCameraSignOffset());
        GameObject speedSign = Instantiate(speedSignPrefab, speedSignPos, Quaternion.identity);
        Destroy(speedSign, 120f);

        GameObject speedCameraSign = Instantiate(speedCameraSignPrefab, speedCameraSignPos, Quaternion.identity);
        Destroy(speedCameraSign, 120f);

        Debug.Log($"Speed sign speed limit: {speedLimit}");
        
    }

    
    float getSpeedCameraSignOffset()
    {
        return Random.Range(spawnCameraSignoffsetDistanceMin, spawnCameraSignoffsetDistanceMax); 
    }
    bool isCarPassedSpeedCameraSign()
    {
        if(mainCar.position.z >= speedCameraSignPos.z)
        {
            Rigidbody mainCarRb = mainCar.GetComponent<Rigidbody>();
            mainCarForwardVelocity = Vector3.Dot(mainCarRb.velocity, mainCar.transform.forward);

            if ((mainCarForwardVelocity * 260 / mainCarForwardSpeedMax) > speedLimit)
            {
                speedSignLimitExceedCount++;
                Debug.Log($"cnt val: {speedSignLimitExceedCount}");
                Debug.Log($"speedLimitt: {speedLimit}");
            }
            return true;
        }
        return false;
    }



}
