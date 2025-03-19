using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CarHandler : MonoBehaviour
{
    [SerializeField]
    Rigidbody rb;

    [SerializeField]
    Transform gameModel;

    float accelerationMultiplier = 15;
    float brakeMultiplier = 50;
    float steeringMultipier = 5;


    float maxForwardVelocity = 30;
    float maxSteeVelocity = 4;

    private float currentRotationY = 0f;
    Vector2 input = Vector2.zero;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Rotate car model when turning
        // gameModel.transform.rotation = Quaternion.Euler(0, rb.velocity.x * 12, 0);
        // Update the target rotation based on steering input only when input.x changes
        // Update the target rotation based on steering input only when input.x changes
        if (Mathf.Abs(input.x) > 0.01f) // A small threshold to avoid noise
        {
            float targetRotationY = input.x * 20;
            currentRotationY = Mathf.Lerp(currentRotationY, targetRotationY, Time.deltaTime * 5f);
        }

        // Apply the persistent rotation
        gameModel.transform.rotation = Quaternion.Euler(0, currentRotationY, 0);
    }


    private void FixedUpdate()
    {
        if (input.y > 0)
            Accelerate();
        else if (input.y == 0)
            rb.drag = 3.0f;

        else if (input.y < 0)
            Brake();
        //else
        // Do nothing

        Steer();

        if (rb.velocity.z <= 0)
            rb.velocity = Vector3.zero;
    }


    void Accelerate()
    {
        rb.drag = 0;
        if (rb.velocity.z >= maxForwardVelocity)
            return;

        rb.AddForce(rb.transform.forward * accelerationMultiplier * input.y);

    }
    void Brake()
    {
        // Don't brake unless we are going forward
        if (rb.velocity.z <= -400)
            return;
        rb.AddForce(rb.transform.forward * brakeMultiplier * input.y);
    }
    void Steer()
    {
        if (Math.Abs(input.x) > 0)
        {
            float speedBaseSteerLimit = rb.velocity.z / 5; // If the car is standing, it cannot go sideways
            speedBaseSteerLimit = Mathf.Clamp01(speedBaseSteerLimit);

            // Move the car sideways
            rb.AddForce(rb.transform.right * steeringMultipier * input.x * speedBaseSteerLimit);

            // Normalize the X velocity (sideways velocity)
            float normalizedX = rb.velocity.x / maxSteeVelocity;

            // Ensure that we dont allow it to be bigger than 1 in magnitude
            normalizedX = Mathf.Clamp(normalizedX, -1.0f, 1.0f);

            // Ensure that we stay within the turn speed limit
            rb.velocity = new Vector3(normalizedX * maxSteeVelocity, 0, rb.velocity.z);
        }
        else
        {
            rb.velocity = Vector3.Lerp(rb.velocity, new Vector3(0, 0, rb.velocity.z), Time.fixedDeltaTime * 3);
        }

    }


    public void SetInput(Vector2 inputVector)
    {
        inputVector.Normalize();
        input = inputVector;
    }

}

