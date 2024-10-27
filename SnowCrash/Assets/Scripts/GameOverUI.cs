using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameOverUI : MonoBehaviour
{
    public Player player;
    public LevelController lc;

    private VisualElement gameOverUI;
    private Label scoreLabel;
    private Label highScoreLabel;

    private float playerScore = 0;
    private float lcHighScore = 0;

    // Start is called before the first frame update
    void Start()
    {
        gameOverUI = GetComponent<UIDocument>().rootVisualElement;
        scoreLabel = gameOverUI.Q<Label>("scoreLabel");
        scoreLabel = gameOverUI.Q<Label>("highScoreLabel");
        gameObject.SetActive(false);
    } 

    // Update is called once per frame
    public void Show()
    {
        gameObject.SetActive(true);
        GetPlayerData();
        scoreLabel.text = "Score: " + playerScore;
        highScoreLabel.text = "Score: " + lcHighScore;
        if (Input.anyKeyDown || Input.touchCount > 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
    private void GetPlayerData()
    {
        playerScore = player.GetScore();
        lcHighScore = lc.GetHighScore();
    }
}
