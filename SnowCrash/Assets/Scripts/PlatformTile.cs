using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformTile: MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public GameObject[] obstacles;

    public void SpawnObstacles()
    {
        //Clear existing obstacles from previous tile placement
        DeactivateObstacles();
        //Set up random generator
        System.Random random = new System.Random();
        int randomInt = random.Next(0, obstacles.Length);

        float randomX = Mathf.Round((float)random.NextDouble()) * 4 - 2;
        float randomZ = Mathf.Round((float)random.NextDouble()) * 12 - 6;

        //Add a random number of obstacles to the platform as it spawns
        obstacles[randomInt].SetActive(true);
        obstacles[randomInt].transform.Translate(new Vector3(randomX, 0, randomZ));
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
