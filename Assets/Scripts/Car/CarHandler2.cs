using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarHandler2 : MonoBehaviour
{
    Vector2 input = Vector2.zero;
    Vector2 leftThumbInput;
    [SerializeField]
    Rigidbody rb;
    public static int hitCount = 0;
    private float rightIndexInput;
    public float forwardSpeed = 0.1f;
    public float forwardSpeedMax = 25f;
    private float forSpeed = 0.9f; // 0.9
    private float turnSpeed = 2.2f;
    private float maxSpeedTimer = 0f; // Timer to track how long the velocity has been at max speed
    private bool hasReachedMaxSpeed = false; // Flag to track if max speed has been reached
    private float maxSpeedTimeLimit = 5f;
    public static int speedLimitExceedCount = 0;
    private float maxXPos = 0.85f;
    private float minXPos = -0.85f;
    private float dragVal = 1.5f;
    private float dragBreakVal = 3;

    void Start()
    {
        forwardSpeedMax = 25;
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void Update()
    {
        updateInputs();
        
        //addForceMetaController();
        addForceKeyboard();

        checkMaxForwardVelocity();       
    }
    void FixedUpdate()
    {
        checkPosition(); //Checks if the car goes out of bounds
    }

    public void SetInput(Vector2 inputVector)
    {
        inputVector.Normalize();
        input = inputVector;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Obstacle")
        {
            hitCount++;
            
            Vector3 basedOnurrentVelocity = rb.velocity * 0.9f;
            float basedOnMaxVelocity = forwardSpeedMax * 0.50f;

            float newSpeed = Mathf.Min(basedOnurrentVelocity.magnitude, basedOnMaxVelocity);

            // Apply the new velocity while maintaining direction
            rb.velocity = basedOnurrentVelocity.normalized * newSpeed;
        }
    }


    void addForceMetaController()
    {
        if (rightIndexInput > 0.01f)
        {  // Operator is trying to accelerate
            Debug.Log($"Right index input {rightIndexInput}");
            rb.AddForce(rb.transform.forward * 10 * forSpeed * rightIndexInput);
            rb.drag = 0f;
        }
        else
        {
            rb.drag = dragVal;
        }
        

        float forwardSpeedd = Vector3.Dot(rb.velocity, rb.transform.forward);
        if (forwardSpeedd > 0.01f)
        { // Car has a forward direction speed
            if (Mathf.Abs(leftThumbInput.x) > 0.01f) // X is for sideways. Y is for forward and backward
            { // Player is trying to move the car sideways
                Debug.Log($"Left thumb index input {leftThumbInput}");
                rb.AddForce(rb.transform.right * turnSpeed * leftThumbInput.x);
                rb.drag = 0f;
                transform.Rotate(Vector3.up, Time.deltaTime * turnSpeed * leftThumbInput.x);
            }
        }
        
        if (leftThumbInput.y < -0.05f)
        {
            rb.drag = -(dragBreakVal * leftThumbInput.y);
        }

    }
    void addForceKeyboard()
    {
        if (input.y > 0.01f)
        { // Operator is trying to accelerate
            rb.AddForce(rb.transform.forward * forSpeed * input.y);
            rb.drag = 0f;
        }
        else if (input.y < -0.01f)
        { // Operator is trying to break
            rb.drag = dragBreakVal;
        }
        else
        { // Operator does nothing
            rb.drag = dragVal;
        }
            

        if (input.x != 0)
        {

            rb.AddForce(rb.transform.right * turnSpeed * input.x);
            rb.drag = 0f;
            
            float forwardSpeed = Vector3.Dot(rb.velocity, rb.transform.forward); // Calculate the forward speed of the Rigidbody
            if (forwardSpeed > 0.05f)
            {
                transform.Rotate(Vector3.up, Time.deltaTime * turnSpeed * input.x);
            }

        }
    }
    private void CheckMaxSpeedDuration(float currentSpeed)
    {
        // Check if the current speed is at or above the maximum speed
        if (currentSpeed >= (forwardSpeedMax-1))
        {
            if (!hasReachedMaxSpeed)
            {
                // Start the timer when max speed is first reached
                hasReachedMaxSpeed = true;
                maxSpeedTimer = 0f;
            }

            // Increment the timer
            maxSpeedTimer += Time.deltaTime;

            // Check if the timer has reached 3 seconds
            if (maxSpeedTimer >= maxSpeedTimeLimit)
            {
                Debug.Log("Max speed maintained for 3 seconds!");
                speedLimitExceedCount++;
                maxSpeedTimer = 0f;
                hasReachedMaxSpeed = false;
            }
        }
        else
        {
            // Reset the timer if the velocity drops below max speed
            maxSpeedTimer = 0f;
            hasReachedMaxSpeed = false;
        }
    }

    void checkPosition() //Checks if the car goes out of bounds
    { 
        Vector3 pos = rb.position;

        if (pos.x > maxXPos)
        {
            pos.x = maxXPos;
            rb.velocity = new Vector3(0, rb.velocity.y, rb.velocity.z);
        }
        else if (pos.x < minXPos)
        {
            pos.x = minXPos;
            rb.velocity = new Vector3(0, rb.velocity.y, rb.velocity.z);
        }

        rb.position = pos;
    }
    void updateInputs()
    {
        rightIndexInput = OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger); // (0.0f, 1.0f)
        leftThumbInput = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);     // (0.0f, 0.0f), (1.0f, 1.0f)
        Debug.Log($"rightIndex {rightIndexInput}");
        Debug.Log($"leftThumb {leftThumbInput.y}");
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");
    }
    void checkMaxForwardVelocity()
    {
        Vector3 forwardVelocity = Vector3.Project(rb.velocity, rb.transform.forward);
        if (forwardVelocity.magnitude > 0.95*forwardSpeedMax)
        {
            rb.velocity = Vector3.ClampMagnitude(forwardVelocity, forwardSpeedMax) + Vector3.Project(rb.velocity, rb.transform.right);
        }
        CheckMaxSpeedDuration(forwardVelocity.magnitude);
    }

    

}
