using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Speedometer : MonoBehaviour
{
    public Rigidbody target;
    public GameObject mainCar;
    public float maxSpeed = 0.0f; // The maximum speed of the target ** IN KM/H **

    public float minSpeedArrowAngle;
    public float maxSpeedArrowAngle;
    private float mainCarForwardSpeedMax = 0;
    [Header("UI")]
    public TMP_Text speedLabel; // The label that displays the speed;
    public RectTransform arrow; // The arrow in the speedometer

    public float speed = 0.0f;
    void Start()
    {
        CarHandler2 mainCarScript = mainCar.GetComponent<CarHandler2>();
        mainCarForwardSpeedMax = mainCarScript.forwardSpeedMax;
    }
    private void Update()
    {
        // 3.6f to convert in kilometers
        // ** The speed must be clamped by the car controller **
        speed = target.velocity.magnitude;

        int speedTextVal = (int)(speed * 260 / mainCarForwardSpeedMax);
        speedLabel.text = (speedTextVal + " km/h");

        arrow.localEulerAngles = new Vector3(0, 0, Mathf.Lerp(minSpeedArrowAngle, maxSpeedArrowAngle, speed / mainCarForwardSpeedMax));
    }
}
