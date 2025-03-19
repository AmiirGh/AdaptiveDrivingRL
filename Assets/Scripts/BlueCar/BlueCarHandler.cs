using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BlueCarHandler : MonoBehaviour
{
    public GameObject mainCar;
    public AudioSource trexSound;
    public Rigidbody rb;

    private float blueForwardSpeed;
    private float lowFrequency = 0.5f;
    private float mainCarForwardSpeedMax = 0;
    public static int hitCount = 0;
    public static int maxDistance = 13;
    public static int midDistance = 10; // 
    private int amplitude = 3;

    //private float initialVelocityMultiplier = 1.2f; // // 1.2f //  Blue car initial speed in each episode is the main car pseed multiplied by this factor 
    //private float finalVelocityMultiplier = 1f;  // 0.85f // Blue car  final  speed in each episode is the main car pseed multiplied by this factor

    private float blueCarMinVelocityMultiplier = 0.92f; // 0.9f the speed that the blue car have when its at the minimum distance from the main car wrt the main car
    private float blueCarMaxVelocityMultiplier = 1.2f; // 1.42f  '    '     '   '    '   '    '   '    '   '   ' maximum     '      '   '    '   '   '   '    '   '

    private float blueCarChasingTimeFrame = 25.0f; // 25.0f Car chasing operator + not chasing operator // In other words, its the whole timeframe
    private float blueCarChasingTimeSpeedDecay = 16.5f; // 16.5f blue car chases the operator for 15 seconds
    public bool isBlueCarChasing = false;
    
    private float mainCarForwardVelocity;
    private int temp = 0;
    float timerrr = 0;
    private bool isGeneratingBlueCar = false;
    private float zDifference = 0f;
    private float zDifferenceInVelocityBlock = 0f;
    private float hitThreshold = 0.3f;
    private float elTime = 0f;


    void Start()
    {
        Debug.Log("inside start");
        CarHandler2 mainCarScript = mainCar.GetComponent<CarHandler2>();
        mainCarForwardSpeedMax = mainCarScript.forwardSpeedMax;
        mainCarForwardSpeedMax = 25f;
        Debug.Log($"max speed: {mainCarForwardSpeedMax}");
        InvokeRepeating("GenerateBlueCar", 4.0f, blueCarChasingTimeFrame);
    }


    void Update()
    {
        elTime += Time.deltaTime;

        //PreviousBluCarMoveAlgorithm(); // This code moves the blue car constantly 
        CheckBlueCarPosition();
        CheckHitCondition();

    }


    void GenerateBlueCar()
    {
        Debug.Log("generate blue car is called");
        isBlueCarChasing = true;
        Rigidbody mainCarRb = mainCar.GetComponent<Rigidbody>();
        mainCarForwardVelocity = Vector3.Dot(mainCarRb.velocity, mainCar.transform.forward);
        transform.position = new Vector3(0, 0, (mainCar.transform.position.z - midDistance * 10)); // the blue car starts 15 meters behind the main car

        Debug.Log($"blue car min vel   {blueCarMinVelocityMultiplier * mainCarForwardSpeedMax}");
        Debug.Log($"blue car max vel   {blueCarMaxVelocityMultiplier * mainCarForwardSpeedMax}");

        StartCoroutine(DecayVelocity(mainCarForwardVelocity));
    }

    IEnumerator DecayVelocity(float initialVelocity)
    {

        // float blueCarInitialVelocity = Mathf.Max(initialVelocity * initialVelocityMultiplier, (initialVelocityMultiplier-0.1f) * mainCarForwardSpeedMax);
        // float blueCarTargetVelocity = Mathf.Max(initialVelocity * finalVelocityMultiplier, (initialVelocityMultiplier-0.1f) * mainCarForwardSpeedMax);
        float elapsedTime = 0f;

        Debug.Log($"vell initial main: {initialVelocity}");
        while (elapsedTime < blueCarChasingTimeSpeedDecay)
        {
            zDifferenceInVelocityBlock = (mainCar.transform.position.z / 10) - (transform.position.z / 10);
            
            float currentBlueCarVelocity = Mathf.Lerp(
                                            blueCarMinVelocityMultiplier * mainCarForwardSpeedMax,
                                            blueCarMaxVelocityMultiplier * mainCarForwardSpeedMax,
                                            Mathf.InverseLerp(hitThreshold, midDistance, zDifferenceInVelocityBlock)
                                        );
            if (zDifferenceInVelocityBlock > 15f)
                Debug.Log($"more than 15: {currentBlueCarVelocity}");

            rb.velocity = new Vector3(0, 0, currentBlueCarVelocity);

            Debug.Log($"blue car velll: {rb.velocity}");
            CheckHitCondition();
            elapsedTime += Time.deltaTime;
            yield return null; 

        }
        CheckBlueCarPosition();
        //CallResetBlueCarPosition();
        rb.velocity = new Vector3(0, 0, 0);
        isBlueCarChasing = false;


    }
    void CallResetBlueCarPosition()
    {
        isBlueCarChasing = false;
        ResetBlueCarPosition("max");
    }
    void CheckHitCondition()
    {
        float zDifference = (mainCar.transform.position.z / 10) - (transform.position.z / 10);
        Debug.Log($"distance Difference: {zDifference}");
        if (zDifference < hitThreshold)
        {
            ResetBlueCarPosition("mid");
            hitCount++;
            Debug.Log($"hit count: {hitCount}");
            trexSound.Play();
        }

    }







    void PreviousBluCarMoveAlgorithm()
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
        // Debug.Log($"Blue car speed {blueSpeed}");
        rb.velocity = transform.forward * blueSpeed;

    }
    
    void ResetBlueCarPosition(string loc)
    {
        if (loc == "mid")
        {
            transform.position = new Vector3(0, 0, (mainCar.transform.position.z - 10*midDistance));
            Debug.Log($"mid pos reset {mainCar.transform.position.z - 10 * midDistance}");
            float zzDifference = (mainCar.transform.position.z - transform.position.z) / 10;
            Debug.Log($"mid pos reset zz difference {zzDifference}");

        }
        else if (loc== "max")
        {
            transform.position = new Vector3(0, 0, (mainCar.transform.position.z - 10*maxDistance));
        }
        else if (loc == "tooFar")
        {
            transform.position = new Vector3(0, 0, (mainCar.transform.position.z - 10 * 3 * maxDistance));
        }
        else
        {
            
        }

    }
    void CheckBlueCarPosition()
    {
        zDifference = (mainCar.transform.position.z - transform.position.z) / 10;

        if (zDifference > maxDistance)
        { // Blue car is too far away. dont let it go any further
            Debug.Log("Blue car too far behind! Resetting position.");
            ResetBlueCarPosition("max");
        }
    }

}