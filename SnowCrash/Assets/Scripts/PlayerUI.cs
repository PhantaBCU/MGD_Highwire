using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerUI : MonoBehaviour
{
    public Player player;
    public LevelController lc;

    private VisualElement playerUI;
    private VisualElement goUI;
    private ProgressBar speedBar;
    private ProgressBar lrBar;
    private VisualElement life1;
    private VisualElement life2;
    private VisualElement life3;
    private Label timerLabel;
    private Label scoreLabel;

    private float playerForwardSpeed = 0;
    private float playerMaxForwardSpeed = 0;
    private float playerHorizontalSpeed = 0;
    private float playerMaxHorizontalSpeed = 0;

    private int playerLivesRemaining = 0;
    private float playerScore = 0;
    
    private float lcTimeRemaining = 0;
    private float lcTimeElapsed = 0;

    // Start is called before the first frame update
    void Start()
    {

        GetPlayerData();
        playerUI = GetComponent<UIDocument>().rootVisualElement;

        timerLabel = playerUI.Q<Label>("timerLabel");
        scoreLabel = playerUI.Q<Label>("scoreLabel");
        speedBar = playerUI.Q<ProgressBar>("speedBar");
        lrBar = playerUI.Q<ProgressBar>("lrBar");
        life1 = playerUI.Q<VisualElement>("life1");
        life2 = playerUI.Q<VisualElement>("life2");
        life3 = playerUI.Q<VisualElement>("life3");

        life1.visible = true;
        life2.visible = true;
        life3.visible = true;


        playerMaxForwardSpeed = player.GetMaxForwardSpeed();
        
        speedBar.lowValue = 0.0f;
        speedBar.highValue = playerMaxForwardSpeed;
        speedBar.value = playerForwardSpeed;

        playerMaxHorizontalSpeed = player.GetMaxHorizontalSpeed();

        lrBar.lowValue = -playerMaxHorizontalSpeed;
        lrBar.highValue = playerMaxHorizontalSpeed;

        Debug.Log("Low Value: " + lrBar.lowValue);
        Debug.Log("High Value: " + lrBar.highValue);

        lrBar.value = playerHorizontalSpeed;
        lrBar.title = playerHorizontalSpeed.ToString("#.##" + "m/s");
    }

    // Update is called once per frame
    void Update()
    {

        int lifecheck = playerLivesRemaining;

        GetPlayerData();

        if (lifecheck != playerLivesRemaining) {
            switch (playerLivesRemaining)
            {
                case 0:
                    GameOver();
                    break;
                case 1:
                    life1.visible = true;
                    life2.visible = false;
                    life3.visible = false;
                    break;
                case 2:
                    life1.visible = true;
                    life2.visible = true;
                    life3.visible = false;
                    break;
                case 3:
                    life1.visible = true;
                    life2.visible = true;
                    life3.visible = true;
                    break;
            }
        }

        speedBar.value = playerForwardSpeed;
        speedBar.title = playerForwardSpeed.ToString("#.##") + " m/s";
        
        lrBar.value = playerHorizontalSpeed;
        lrBar.title = playerHorizontalSpeed.ToString("#.##") + " m/s";

        timerLabel.text = "Time left: " + lcTimeRemaining.ToString("##.##") + "s";
        scoreLabel.text = "Score: " + playerScore;
    }

    private void GameOver()
    {
        gameObject.SetActive(false);
    }
    private void GetPlayerData()
    {
        playerForwardSpeed = player.GetForwardSpeed();  
        playerHorizontalSpeed = player.GetHorizontalSpeed();
        playerLivesRemaining = player.GetLivesRemaining();
        playerScore = player.GetScore();

        lcTimeElapsed = lc.GetTimeElapsed();
        lcTimeRemaining = lc.GetTimeRemaining();

    }
}
