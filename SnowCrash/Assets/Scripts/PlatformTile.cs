using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformTile: MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public Transform[] spawnPoints;
    public GameObject[] obstacles;

    public void SpawnObstacles()
    {
        //Clear existing obstacles from previous tile placement
        DeactivateObstacles();
        //Set up random generator
        System.Random random = new System.Random();
        int randomObstacleInt = random.Next(0, obstacles.Length);
        int randomSpawnPointInt = random.Next(0, spawnPoints.Length);

        //Add a random number of obstacles to the platform as it spawns
        obstacles[randomObstacleInt].SetActive(true);
        obstacles[randomObstacleInt].transform.Translate(spawnPoints[randomSpawnPointInt].localPosition);
    }
    public void DeactivateObstacles()
    {
        //Clear existing obstacles
        for (int i = 0; i < obstacles.Length; i++)
        {
            obstacles[i].SetActive(false);
        }
    }
}
