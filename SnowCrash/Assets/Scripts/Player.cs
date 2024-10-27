using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    public Camera mainCam;
    public GameObject playerUI;
    private Gyroscope m_Gyro;
    private Vector3 camPos;
    private Vector3 camTilt;
    public int maxLives = 3;
    public enum InputModeSelector
    {
        Gyro,
        Joystick,
        Keyboard
    }
    public InputModeSelector InputSelected = InputModeSelector.Gyro;

    private float score = 0.0f;
    private float gyroZ = 0.0f;
    private float previousGyroZ = 0.0f;
    private int livesRemaining;
    
    private bool gameStarted = false;
    private bool gameOver = false;
    
    public bool DebugMessagesOn = true;

    public float maxHorizontalSpeed = 2f;
    private float horizontalSpeed = 0.0f;

    public float maxForwardSpeed = 20.0f;
    public float startForwardSpeed = 5.0f;
    private float forwardSpeed = 10.0f;
    
    public float horizontalSpeedStep = 0.01f;

    private float timeSinceLastBoost = 0;
    public float boostTimer = 2.0f;
    public float boostSpeedStep = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        livesRemaining = maxLives;
        m_Gyro = Input.gyro;
        m_Gyro.enabled = true;
        camPos = mainCam.transform.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameStarted && !gameOver)
        {
            //Move player according to input
            HandleInput();
            transform.Translate(horizontalSpeed * Time.deltaTime, 0, forwardSpeed * Time.deltaTime);
        
            //Keep within horizontal bounds of playArea
            Vector3 clampedPosition = transform.position;
            clampedPosition.x = Mathf.Clamp(clampedPosition.x, -4.0f, 4.0f);
            transform.position = clampedPosition;

            mainCam.transform.position = transform.position + camPos;

            DebugMessages();

            //Increase speed per boostTimer
            timeSinceLastBoost += Time.deltaTime;
            if (timeSinceLastBoost > boostTimer)
            {
                AddBoost();
            }
        }
    }
    private void DebugMessages()
    {
        if (DebugMessagesOn)
        {
            //Output the rotation rate, attitude and the enabled state of the gyroscope as a Label
            Debug.Log("Gyro rotation rate " + m_Gyro.rotationRate);
            Debug.Log("Gyro attitude" + m_Gyro.attitude);
            Debug.Log("Gyro enabled : " + m_Gyro.enabled);
        }
    }
    public void RemoveLife()
    {
        if (livesRemaining > 1)
        {
            livesRemaining--;
            
        }
        else if (livesRemaining == 1)
        {
           GameOver();
        }
    }
    public void GainLife()
    {
        if (livesRemaining < maxLives)
        {
            livesRemaining++;
        }
        else
        {
            score += 100;
        }
    }
    public void AddScore()
    {
        score += 100;
    }
    public void Removescore()
    {
        score -= 25;
    }


    public void AddBoost()
    {
        forwardSpeed = Mathf.Clamp(forwardSpeed += boostSpeedStep,-maxForwardSpeed,maxForwardSpeed);
        ResetBoostTimer();
    }
    public void ResetSpeed()
    {
        forwardSpeed = startForwardSpeed;
        ResetBoostTimer();
    }
    public void ResetBoostTimer()
    {
        timeSinceLastBoost = 0;
    }
    public void SetInputMode(InputModeSelector _inputMode)
    {
        InputSelected = _inputMode;
    }

    public void SetBaselineGyro(float _gyroInput)
    {
        gyroZ = _gyroInput;
    }
    public void StartGame()
    {
        gameStarted = true;
        gameOver = false;
        SetInputMode((InputModeSelector)InputSelected);
        SetBaselineGyro(m_Gyro.rotationRate.z);
        forwardSpeed = startForwardSpeed;
    }
    public void GameOver()
    {
        gameOver = true;
    }
    private void HandleInput()
    {
        if (gameStarted)
            {

            if (InputSelected == InputModeSelector.Gyro)
            {
                /*
                previousGyroZ = gyroZ;
                gyroZ += -m_Gyro.rotationRate.z;

                float gyroDelta = Mathf.Abs(gyroZ - previousGyroZ);

                if (gyroDelta > 0.01f)
                {
                    horizontalSpeed += gyroDelta * horizontalSpeedStep * Mathf.Sign(gyroZ);
                }
                */
                gyroZ += -m_Gyro.rotationRate.z;
                horizontalSpeed += MathF.Sign(gyroZ) * horizontalSpeedStep;
            }
            else if (InputSelected == InputModeSelector.Keyboard)
            {
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    horizontalSpeed -= horizontalSpeedStep;
                }
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    horizontalSpeed += horizontalSpeedStep;
                }
            }
            horizontalSpeed = Mathf.Clamp(horizontalSpeed, -maxHorizontalSpeed, maxHorizontalSpeed);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (DebugMessagesOn)
        {
            Debug.Log("Collision with: " + collision.gameObject.tag);
        }

        if (collision.gameObject.tag == "Obstacle")
        {
            RemoveLife();
            ResetSpeed();
        }
        else if (collision.gameObject.tag == "ScoreObject")
        {
            AddScore();
        }
        else if (collision.gameObject.tag == "Gate")
        {
            GainLife();
        }
        
        if (collision.gameObject.tag == "Railing")
        {
            horizontalSpeed = -horizontalSpeed * 0.3f;
        }
    }

    public float GetScore()
    {
        return score;
    }
    public float GetForwardSpeed()
    {
        return forwardSpeed;
    }
    public float GetMaxForwardSpeed()
    {
        return maxForwardSpeed;
    }
    public int GetLivesRemaining()
    {
        return livesRemaining;
    }
    public float GetHorizontalSpeed()
    {
        return horizontalSpeed;
    }
    public float GetMaxHorizontalSpeed()
    {
        return maxHorizontalSpeed;
    }

    public bool GetGameStarted()
    {
        return gameStarted;
    }
    public bool GetGameOver()
    {
        return gameOver;
    }
}
