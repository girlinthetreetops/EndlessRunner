using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIGameplay : MonoBehaviour
{

    //top bar
    [SerializeField] GameObject topBar;
    [SerializeField] private Button pauseButton;
    [SerializeField] TMP_Text levelScoreDisplay;
    [SerializeField] TMP_Text levelCoinsDisplay;

    //pause screen
    [SerializeField] GameObject pauseScreen;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button quitButton;

    //results screen
    [SerializeField] GameObject resultsScren;
    [SerializeField] private TMP_Text finalScore;
    [SerializeField] private Button exitFinalScreen;
    [SerializeField] private Button TryAgain;

    //other
    GameManager gameManager;
    LevelManager levelManager;
    InputManager inputManager;

    //Locals
    private bool isGamePaused;
    private bool isLevelComplete;


    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        levelManager = FindObjectOfType<LevelManager>();
        inputManager = FindObjectOfType<InputManager>();

        //set defaults
        isGamePaused = false;
        isLevelComplete = false;

        //Top bar
        pauseButton.onClick.AddListener(PauseToggle); //via top bar button
        inputManager.OnPauseToggle.AddListener(PauseToggle); //with shortkey button

        //Pause menu
        quitButton.onClick.AddListener(Quit);

        //EndScreen
        levelManager.OnDeath.AddListener(ShowResultUI); //trigger end screen
        exitFinalScreen.onClick.AddListener(ExitLevel); //exit the end screen
        TryAgain.onClick.AddListener(RestartLevel);
    }

    private void Update()
    {
        levelScoreDisplay.SetText(levelManager.levelScore.ToString());
        levelCoinsDisplay.SetText(levelManager.coinsCollectedInLevel.ToString());
    }

    private void PauseToggle()
    {
        if (!isGamePaused && !isLevelComplete)
        {
            isGamePaused = true;
            Time.timeScale = 0f;
            pauseScreen.SetActive(true);
        } else if (isGamePaused)
        {
            isGamePaused = false;
            pauseScreen.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    private void Quit()
    {
        isGamePaused = false;
        isLevelComplete = true;

        Time.timeScale = 1f;
        gameManager.QuitGameplay();
    }

    private void ShowResultUI()
    {
        isLevelComplete = true;
        topBar.SetActive(false); 
        resultsScren.SetActive(true); //change screens

        finalScore.SetText(levelManager.levelScore.ToString("F0")); //display score
        gameManager.UpdateHighScore(gameManager.selectedGameIndex, levelManager.levelScore); //update the global scores via gamemanager

    }

    private void ExitLevel()
    {
        isLevelComplete = true;
        gameManager.QuitGameplay();
    }

    private void RestartLevel()
    {
        gameManager.StartGameplay(gameManager.selectedGameIndex);
    }
}
