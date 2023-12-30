using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIStartScreen : MonoBehaviour
{
    //big references
    [SerializeField] private GameManager gameManager;
    [SerializeField] private InputManager inputManager;

    //locals
    private int displayedLevelIndex = 0;

    //references to UI parts, set in inspector at the moment
    [SerializeField] private Button startLevel;
    [SerializeField] private Button showNextLevelButton;
    [SerializeField] private Button showPreviousLevelButton;

    [SerializeField] private Image levelPreviewImage;
    [SerializeField] private TMP_Text selectedLevelTitleDisplay;
    [SerializeField] private TMP_Text displayedLevelHighScore;
    [SerializeField] private TMP_Text goldDisplay;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        inputManager = FindObjectOfType<InputManager>();

        inputManager.OnRightSwipe.AddListener(ChangeDisplayedLevel);
        showNextLevelButton.onClick.AddListener(ChangeDisplayedLevel);
        showPreviousLevelButton.onClick.AddListener(ChangeDisplayedLevel);
        startLevel.onClick.AddListener(AskGMToStartGame);

        //showNextLevelButton.onClick.AddListener(ChangeDisplayedLevel);
        //showPreviousLevelButton.onClick.AddListener(ChangeDisplayedLevel);

        UpdateUI();
    }

    private void ConfirmClick()
    {
        Debug.Log("Click detected");
    }

    public void ChangeDisplayedLevel()
    {
        if (displayedLevelIndex < gameManager.GameLevels.Count - 1)
        {
            displayedLevelIndex += 1;
            Debug.Log("Index should vary. It is now: " + displayedLevelIndex);
        } else
        {
            displayedLevelIndex = 0;
            Debug.Log("Index should be 0. It is: " + displayedLevelIndex);
        }

        UpdateUI();
    }

    public void UpdateUI()
    {
        selectedLevelTitleDisplay.SetText(gameManager.GameLevels[displayedLevelIndex].levelTitle);
        displayedLevelHighScore.SetText("High Score: " + gameManager.GameLevels[displayedLevelIndex].highScore.ToString("F0"));
        levelPreviewImage.sprite = gameManager.GameLevels[displayedLevelIndex].levelThumbnail;
        goldDisplay.SetText(gameManager.gold.ToString());

    }

    public void AskGMToStartGame()
    {
        gameManager.StartGameplay(displayedLevelIndex);
    }
}
