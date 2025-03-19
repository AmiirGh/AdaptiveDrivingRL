using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCar : MonoBehaviour
{
    public GameObject mainCar;
    private Vector3 cameraOffset = new Vector3(0.0f, 0.44f, -0.74f);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        
    }
    void LateUpdate()
    {
        transform.position = mainCar.transform.position + cameraOffset;

    }
}
