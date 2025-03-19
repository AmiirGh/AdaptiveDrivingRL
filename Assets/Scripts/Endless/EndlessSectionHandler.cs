using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessSectionHandler : MonoBehaviour
{
    Transform playerCarTransform;
    // Start is called before the first frame update
    void Start()
    {
        playerCarTransform = GameObject.FindGameObjectWithTag("Player").transform;

    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = transform.position.z - playerCarTransform.position.z;
        float temp = (distanceToPlayer - 100) / 150.0f;


        float lerpPercentage1 = 1.0f - temp;
        float lerpPercentage2 = Mathf.Clamp01(lerpPercentage1);

        transform.position = Vector3.Lerp(new Vector3(transform.position.x, -50, transform.position.z),
                                          new Vector3(transform.position.x, 0, transform.position.z),
                                          lerpPercentage2);

        //Debug.Log($"distance to player: {distanceToPlayer}");

        //Debug.Log($"dist: {(distanceToPlayer - 100)}");
        //Debug.Log($"lerp2: {(lerpPercentage2)}");
        //Debug.Log($"lerp1: {(lerpPercentage1)}");


    }
}
