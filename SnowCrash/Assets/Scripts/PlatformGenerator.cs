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

    List<PlatformTile> spawnedTiles = new List<PlatformTile>();

    public static PlatformGenerator instance;

   private void Start()
    {
        instance = this;

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
        }
    }
    private void Update()
    {

        if (mainCam.WorldToViewportPoint(spawnedTiles[0].endPoint.position).z < -1)
        {
            PlatformTile tileTemp = spawnedTiles[0];
            spawnedTiles.RemoveAt(0);
            tileTemp.transform.position = spawnedTiles[spawnedTiles.Count - 1].endPoint.position - tileTemp.startPoint.localPosition;
            tileTemp.SpawnObstacles();
            spawnedTiles.Add(tileTemp);
        }
    }
}
