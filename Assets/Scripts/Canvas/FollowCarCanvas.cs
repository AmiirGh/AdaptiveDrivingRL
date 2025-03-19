using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCarCanvas : MonoBehaviour
{
    public GameObject mainCar;
    private Vector3 canvasOffset = new Vector3(0.0f, 0.3544f, +0.88f);
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
        transform.position = mainCar.transform.position + canvasOffset;

    }
}
