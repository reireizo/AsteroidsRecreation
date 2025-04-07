using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    // Reference to Asteroid object through it's script.
    public Asteroid asteroidPrefab;
    // Float value representing the min and max variance on the spawned asteroid's trajectory (degrees).
    public float trajectoryVariance = 15.0f;
    // Float value representing the amount of time it takes to spawn a new asteroid (seconds).
    public float spawnRate = 2.0f;
    // Float value representing how far out from the spawner the asteroid can spawn (radius).
    public float spawnDistance = 15.0f;
    // Int value representing how many asteroids get spawned at one time.
    public int spawnAmount = 1;

    // Start needs to:
    // > Have Spawn function invoke repeatedly over time set in spawnRate.
    void Start()
    {
        InvokeRepeating(nameof(Spawn), spawnRate, spawnRate);
    }

    // Spawn needs to:
    // > Create a number of asteroids equal to spawnAmount.
    //   > Set a random spawn direction, and a spawn point based on that direction.
    //   > Set a random variance on rotation, and create a rotation Quaternion with it.
    //   > Instantiate an asteroid using the spawn point and rotation created
    //   > Set the asteroid size.
    void Spawn()
    {
        for (int i = 0; i < spawnAmount; i++)
        {
            Vector3 spawnDirection = Random.insideUnitCircle.normalized * spawnDistance;
            Vector3 spawnPoint = this.transform.position + spawnDirection;

            float variance = Random.Range(-trajectoryVariance, trajectoryVariance);
            Quaternion rotation = Quaternion.AngleAxis(variance, Vector3.forward);

            Asteroid asteroid = Instantiate(asteroidPrefab, spawnPoint, rotation);
            asteroid.size = Random.Range(asteroid.minSize, asteroid.maxSize);
            asteroid.SetTrajectory(rotation * -spawnDirection);
        }
    }
}
