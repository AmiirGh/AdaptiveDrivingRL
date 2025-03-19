using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using TMPro;

public class SpeedSign : MonoBehaviour
{

    public TextMeshProUGUI speedLimitText;
    public static int speedLimit = 0;

    private void Start()
    {
        speedLimitText = GetComponent<TextMeshProUGUI>();
        UpdateSpeed();
    }

    void UpdateSpeed()
    {
        int[] speeds = {180, 220, 250};
        speedLimit = speeds[Random.Range(0, speeds.Length)];
        speedLimitText.text = $"<b>{speedLimit}</b>\n<b>Km/h</b>";
    }
}
