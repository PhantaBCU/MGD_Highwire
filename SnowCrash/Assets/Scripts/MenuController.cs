using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;



public class MenuController : MonoBehaviour
{
    public VisualElement ui;
    public Button playBtn;
    public Button optionsBtn;
    public Button exitBtn;

    public LevelController lc;

    // Start is called before the first frame update
    void Start()
    {
        ui = GetComponent<UIDocument>().rootVisualElement;
        playBtn = ui.Q<Button>("playButton");
        optionsBtn = ui.Q<Button>("optionsButton");
        exitBtn = ui.Q<Button>("exitButton");

        playBtn.clicked += OnPlayBtnClicked;
        optionsBtn.clicked += OnOptionsBtnClicked;
        exitBtn.clicked += OnExitBtnClicked;
    }

    private void OnPlayBtnClicked()
    {
        gameObject.SetActive(false);
        lc.StartGame();
    }

    private void OnOptionsBtnClicked()
    {
        Debug.Log("Options clicked - TO IMPLEMENT");
    }
    private void OnExitBtnClicked()
    {
        Application.Quit();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
