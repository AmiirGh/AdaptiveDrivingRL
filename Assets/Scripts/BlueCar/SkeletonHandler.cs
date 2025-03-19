using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonHandler : MonoBehaviour
{
    public GameObject mainCar;
    private float blueForwardSpeed;
    private float lowFrequency = 0.5f;
    private float mainCarForwardSpeedMax = 0;
    public Rigidbody rb;
    //public static int hitCount = 0;
    private int maxDistance = 25;
    private int amplitude = 3;
    public AudioSource trexSound;
    void Start()
    {
        CarHandler2 mainCarScript = mainCar.GetComponent<CarHandler2>();
        mainCarForwardSpeedMax = mainCarScript.forwardSpeedMax;
    }


    void Update()
    {
        MoveBlueCar();

        if (mainCar.transform.position.z / 10 > 0.01f)
        {
            CheckHitCondition();
        }
        CheckBlueCarPosition();

    }

    void MoveBlueCar()
    {
        float timeFactor = Time.time * lowFrequency;
        float sinWave = amplitude * Mathf.Sin(timeFactor);

        float blueSpeed = 0.90f * mainCarForwardSpeedMax + sinWave;
        Debug.Log($"Blue car speed {blueSpeed}");
        rb.velocity = transform.forward * blueSpeed;

    }
    void CheckHitCondition()
    {
        float zDifference = Mathf.Abs(transform.position.z / 10 - mainCar.transform.position.z / 10);
        if (zDifference < 2)
        {

            //trexSound.Play();
            ResetBlueCarPosition("mid");
        }
        if (zDifference > maxDistance)
        {
            ResetBlueCarPosition("max");
        }
    }
    void ResetBlueCarPosition(string loc)
    {
        if (loc == "mid")
        {
            Vector3 newPosition = new Vector3(
                0,
                0,
                (mainCar.transform.position.z - 150)
            );

            transform.position = newPosition;
        }
        else if (loc == "max")
        {
            Vector3 newPosition = new Vector3(
                0,
                0,
                (mainCar.transform.position.z - 10 * maxDistance)
            );

            transform.position = newPosition;
        }

    }
    void CheckBlueCarPosition()
    {
        float zDifference = (mainCar.transform.position.z - transform.position.z) / 10;

        if (zDifference > maxDistance)
        {
            Debug.Log("Blue car too far behind! Resetting position.");
            ResetBlueCarPosition("max");
        }
    }

}
