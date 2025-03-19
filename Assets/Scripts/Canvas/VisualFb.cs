using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualFb : MonoBehaviour
{

    public Image circleImage; // Reference to the UI Image component
    public GameObject blueCar;
    public GameObject mainCar;
    public int flag = 1; // Flag to control visibility (0 = disappear, 1 = appear)
    public float distanceToBlueCar = 0;
    private int action = 0;
    private float maxDistance = BlueCarHandler.maxDistance; // Maximum distance for sound playback
    private int visualIndex = 2;
    public float feedbackIntensity = 0;
    void Update()
    {
        distanceToBlueCar = (int)Mathf.Abs(mainCar.transform.position.z / 10 - blueCar.transform.position.z / 10);
        UpdateCircleColor();
    }

    void UpdateCircleColor()
    {
        distanceToBlueCar = mainCar.transform.position.z / 10 - blueCar.transform.position.z / 10;
        feedbackIntensity = Mathf.InverseLerp(0, maxDistance / 2, distanceToBlueCar);
        action = TCP.action;
        action = visualIndex;
        // if (TCP.action != 1)
        if (action != visualIndex)
        {
            circleImage.enabled = false;
            return;
        }
        else
        {

            circleImage.enabled = true;

            

            if (distanceToBlueCar > 0.95f*maxDistance)
            {
                
                circleImage.color = Color.white;
                return;
            }

            if (distanceToBlueCar > 0)
            {
                Debug.Log($"distnaceToBlueCar: {distanceToBlueCar}");
                
                Debug.Log($"Color value: {feedbackIntensity}");
                circleImage.color = Color.Lerp(Color.red, Color.white, feedbackIntensity);
            }
            
        }
    }
}
