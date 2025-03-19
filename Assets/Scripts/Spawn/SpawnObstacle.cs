using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SpawnObstacle : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public Transform mainCar;
    public float spawnDistanceMin = 1f * 10;
    public float spawnDistanceMax = 1.2f * 10;

    private float timer = 0;
    private float nextSpawnZ;
    private float spawnInterval = 2.0f; // 2.0f
    private float xMin = -1f, xMax = 1f;


    private float lastSpawnZ = 0; // Track last spawn position
    private float spawnDistanceThreshold = 4.5f;//5.7f // can be changed to 5 // Distance required for next spawn


    void Update()
    { 
        if (mainCar.position.z - lastSpawnZ >= spawnDistanceThreshold)
        { // the main car has traveled at least 3 meters from the last spawn position
            ObstacleSpawner();
            
        }
    }

    void ObstacleSpawner()
    {
        float randomX = Random.Range(xMin, xMax);
        float spawnZ = mainCar.position.z + Random.Range(spawnDistanceMin, spawnDistanceMax);
        Vector3 spawnPosition = new Vector3(randomX, 0.049f, spawnZ);

        GameObject newObstacle = Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);
        Debug.Log("One obstacle instantiated");
        Destroy(newObstacle, 30f);


        lastSpawnZ = mainCar.position.z; // Update last spawn position
    }
}