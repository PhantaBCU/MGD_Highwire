using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class PlatformGenerator : MonoBehaviour
{
    //Use relevant gameObjects / scripts
    public Camera mainCam;
    public Transform startPoint;
    public PlatformTile tilePF;
    public Player player;

    //Game setup
    public int startingTileCount = 15;
    public int freeTiles = 3;

    public float speedIncreaseStep = 1;
    public float maxSpeed = 1;
    public float startSpeed = 1;

    private float speed;
    
    float startTime = 0;
    float elapsedTime = 0;
    float timeRemaining = 0;
    public float levelTimeLimit = 30.0f;

    List<PlatformTile> spawnedTiles = new List<PlatformTile>();

    bool gameOver = false;
    bool gameStarted = false;

    public static PlatformGenerator instance;

   private void Start()
    {
        instance = this;

        speed = startSpeed;
        timeRemaining = levelTimeLimit;
        Vector3 spawnPos = startPoint.position;
        int freeTiling = freeTiles;
        for (int i = 0; i < startingTileCount; i++)
        {
            spawnPos -= tilePF.startPoint.localPosition;
            PlatformTile spawnedTile = Instantiate(tilePF, spawnPos, Quaternion.identity) as PlatformTile;
            if (freeTiling > 0)
            {
                spawnedTile.DeactivateObstacles();
                freeTiling--;
            }
            else
            {
                spawnedTile.SpawnObstacles();
            }
            spawnPos = spawnedTile.endPoint.position;
            spawnedTile.transform.SetParent(transform);
            spawnedTiles.Add(spawnedTile);
            startTime = Time.time;
        }
    }
    private void Update()
    {
        if (!gameOver && gameStarted)
        {
            transform.Translate(-spawnedTiles[0].transform.forward * Time.deltaTime * speed, Space.World);
            speed += Time.deltaTime * speedIncreaseStep;
            elapsedTime += Time.deltaTime;
            timeRemaining -= Time.deltaTime;
            if (timeRemaining < 0)
            {
                timeRemaining = 0.0f;
                GameOver();
            }
        }
        if (mainCam.WorldToViewportPoint(spawnedTiles[0].endPoint.position).z < 0)
        {
            PlatformTile tileTemp = spawnedTiles[0];
            spawnedTiles.RemoveAt(0);
            tileTemp.transform.position = spawnedTiles[spawnedTiles.Count - 1].endPoint.position - tileTemp.startPoint.localPosition;
            tileTemp.SpawnObstacles();
            spawnedTiles.Add(tileTemp);
        }
        if (gameOver || !gameStarted)
        {
            if (Input.anyKeyDown || Input.touchCount > 0)
            {
                if (gameOver)
                {
                    //Reload scene
                    Scene scene = SceneManager.GetActiveScene();
                    SceneManager.LoadScene(scene.name);
                }
                if (!gameStarted)
                {
                    gameStarted = true;
                }
            }
        }
    }
    private void OnGUI()
    {
        if (gameOver)
        {
            GUI.color = Color.red;
            GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 100, 200, 200), "Game Over\nYour score is: " + (player.GetScore()) + "\nTap to restart");
        }
        else
        {
            if (!gameStarted)
            {
                GUI.color = Color.red;
                GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 100, 200, 200), "Tap anywhere to start!");
            }
        }


        GUI.color = Color.green;
        GUI.Label(new Rect(Screen.width - 205, Screen.height / 2 - 100, 200, 25), "Time Remaining: " + Math.Round((double)timeRemaining, 2));
    }
    public void GameOver()
    {
        gameOver = true;
    }
}
