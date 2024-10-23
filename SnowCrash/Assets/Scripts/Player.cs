using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Player : MonoBehaviour
{
    public int maxLives;
    private float horizontalSpeed = 0.0f;
    private int livesRemaining;
    public float maxSpeed = 2.0f;
    public float speedStep = 0.1f;
    private float score = 0.0f;
    private Gyroscope m_Gyro;
    public bool DebugMessagesOn = true;
    // Start is called before the first frame update
    void Start()
    {
        livesRemaining = maxLives;
        m_Gyro = Input.gyro;
        m_Gyro.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        //Move player according to input
        HandleInput();
        transform.Translate(horizontalSpeed * Time.deltaTime, 0, 0);
        
        //Keep within bounds of playArea
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, -4.0f, 4.0f);
        transform.position = clampedPosition;
        DebugMessages();
    }
    private void DebugMessages()
    {
        if (DebugMessagesOn && m_Gyro.enabled)
        {
            //Output the rotation rate, attitude and the enabled state of the gyroscope as a Label
            Debug.Log("Gyro rotation rate " + m_Gyro.rotationRate);
            Debug.Log("Gyro attitude" + m_Gyro.attitude);
            Debug.Log("Gyro enabled : " + m_Gyro.enabled);
        }
    }
    public void removeLife()
    {
        if (livesRemaining > 0)
        {
            livesRemaining--;
        }
        else
        {
            PlatformGenerator.instance.GameOver();
        }
    }
    public void gainLife()
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
    public void addScore()
    {
        score += 100;
    }
    public void removescore()
    {
        score -= 100;
    }
    public float GetScore()
    {
        return score;
    }
    private void HandleInput()
    {
//        Mathf.Clamp01(horizontalSpeed -= m_Gyro.rotationRate);

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Mathf.Clamp01(horizontalSpeed -= speedStep);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Mathf.Clamp01(horizontalSpeed += speedStep);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision with: " + collision.gameObject.tag);
        if (collision.gameObject.tag == "Obstacle")
        {
            removeLife();
        }
        else if (collision.gameObject.tag == "ScoreObject")
        {
            addScore();
        }
        else if (collision.gameObject.tag == "Gate")
        {
            gainLife();
        }
        
        if (collision.gameObject.tag == "Railing")
        {
            horizontalSpeed = -horizontalSpeed * 0.5f;
        }
    }
    private void OnGUI()
    {
        GUI.color = Color.black;
        GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 100, 200, 200), "Lives: " + livesRemaining + "\nScore: "+ score);
    }
}
