using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class LevelController : MonoBehaviour
{
    public Player player;
    public GameOverUI goUI;
    public GameObject playerUI;
    public GameObject gameOverUI;

    float startTime = 0;
    float timeElapsed = 0;
    float timeRemaining = 0;
    public float levelTimeLimit = 30.0f;
    private float highScore;

    bool gameOver = false;
    bool gameStarted = false;


    // Start is called before the first frame update
    void Awake()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        LoadPlayerPrefs();
    }

    public void StartGame()
    {
        gameStarted = true;
        startTime = Time.time;
        timeElapsed = 0.0f;
        timeRemaining = levelTimeLimit;
        player.StartGame();
    }
    public void GameOver()
    {
        gameOver = true;
        SavePlayerPrefs();
        goUI.Show();
    }

    // Update is called once per frame
    void Update()
    {
        gameStarted = player.GetGameStarted();
        gameOver = player.GetGameOver();

        if (!gameOver && gameStarted)
        {
            timeElapsed += Time.deltaTime;

            timeRemaining -= Time.deltaTime;
            if (timeRemaining < 0)
            {
                timeRemaining = 0.0f;
                GameOver();
            }
        }
    }
    void LoadPlayerPrefs()
    {
        highScore = PlayerPrefs.GetFloat("HighScore", 0);
    }
    void SavePlayerPrefs()
    {
        float ps = player.GetScore(); ;
        if (ps > highScore)
        {
            PlayerPrefs.SetFloat("HighScore", ps);
            PlayerPrefs.Save();
        }
    }
    public float GetTimeRemaining()
    {
        return timeRemaining;
    }
    public float GetTimeElapsed()
    {
        return timeElapsed;
    }
    public float GetHighScore()
    {
        return highScore;
    }
}
