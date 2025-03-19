using UnityEngine;

public class GameTrigger : MonoBehaviour
{
    private bool gameStarted = false;
    private float rightIndexInput;
    void Start()
    {
        // Pause everything at the beginning
        Time.timeScale = 0f;
    }

    void Update()
    {
        rightIndexInput = OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger);
        if (!gameStarted && (rightIndexInput >= 0.1f || Input.GetKeyDown(KeyCode.S)))
        {
            StartGame();
        }
    }

    void StartGame()
    {
        gameStarted = true;
        Time.timeScale = 1f; // Resume game
        Debug.Log("Game Started!");
    }
}
