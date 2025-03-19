using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioFb : MonoBehaviour
{
    public AudioSource audioSource;
    public VisualFb visualFb;

    public float minDistance = 1f; // Minimum distance for sound playback
    public float maxDistance = 25f; // Maximum distance for sound playback
    public float minPlayRate = 0f; // Slowest play rate (e.g., 1 beep every 2 seconds)
    public float maxPlayRate = 5f; // Fastest play rate (e.g., 5 beeps per second)

    private float timeSinceLastPlay;
    private float distanceToBlueCar = 0;
    private int action = 0;
    private int audioIndex = 1;
    void Update()
    {
        
        distanceToBlueCar = visualFb.distanceToBlueCar;
        
        
        action = TCP.action;
        //action = audioIndex + 1;
        Debug.Log($"action value {action}");
        if (action != audioIndex)
        {
            return;
        }
        if (distanceToBlueCar > 0) 
        {
            if (distanceToBlueCar > maxDistance)
            {
                return;
            }
            // Map the distance to a play rate between minPlayRate and maxPlayRate
            // float playRate = Mathf.Lerp(maxPlayRate, minPlayRate, Mathf.InverseLerp(minDistance, maxDistance, distanceToBlueCar));
            float playRate = Mathf.Lerp(maxPlayRate, minPlayRate, visualFb.feedbackIntensity);

            // Calculate the delay between plays based on the play rate
            float delayBetweenPlays = 1f / playRate;

            // Play the sound at the calculated rate
            timeSinceLastPlay += Time.deltaTime;
            if (timeSinceLastPlay >= delayBetweenPlays)
            {
                audioSource.PlayOneShot(audioSource.clip); // Play the sound
                timeSinceLastPlay = 0f; // Reset the timer
            }
        }
    }
}
