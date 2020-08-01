using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager_Gameplay : MonoBehaviour
{
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI scoreText;
    public GameObject pauseScreen;
    public GameObject endOfLevelScreen;
    public GameObject deathScreen;
    public TextMeshProUGUI deathText;
    public TextMeshProUGUI finalScoreText;

    public static event Action onPauseScreenToggle;
    public static event Action onNextLevelButtonPressed;

    void OnEnable()
    {
        GameManager.onPauseToggle += TogglePauseScreen;
        GameManager.onGameEnd += ActivateDeathScreen;
        LevelManager.onCurrentLevelUpdate += UpdateLevel;
        PlayerModel.onScoreUpdate += UpdateScore;
        PlayerController.onLevelEndReached += ActivateEndOfLevelScreen;
    }

    void OnDisable()
    {
        GameManager.onPauseToggle -= TogglePauseScreen;
        GameManager.onGameEnd -= ActivateDeathScreen;
        LevelManager.onCurrentLevelUpdate -= UpdateLevel;
        PlayerModel.onScoreUpdate -= UpdateScore;
        PlayerController.onLevelEndReached -= ActivateEndOfLevelScreen;
    }

    void TogglePauseScreen(bool state)
    {
        pauseScreen.SetActive(state);

        //SoundManager.Get().PlaySound(SoundManager.Sounds.Button);
    }

    void UpdateScore(int score)
    {
        scoreText.text = "SCORE: " + score;
    }

    void UpdateLevel(int level)
    {
        levelText.text = "LEVEL: " + level;
    }

    void ActivateEndOfLevelScreen()
    {
        endOfLevelScreen.SetActive(true);
    }

    void ActivateDeathScreen(int finalScore)
    {
        deathText.text = "YOU LOST!";
        finalScoreText.text = "FINAL SCORE: " + finalScore;

        deathScreen.SetActive(true);
    }

    public void TogglePause()
    {
        if (onPauseScreenToggle != null) onPauseScreenToggle();
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");

        //SoundManager.Get().PlaySound(SoundManager.Sounds.Button);
    }

    public void GoToNextLevel()
    {
        endOfLevelScreen.SetActive(false);

        if (onNextLevelButtonPressed != null) onNextLevelButtonPressed();

        //SoundManager.Get().PlaySound(SoundManager.Sounds.Button);
    }
}