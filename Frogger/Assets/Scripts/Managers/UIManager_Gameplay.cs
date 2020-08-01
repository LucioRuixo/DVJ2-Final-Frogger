using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager_Gameplay : MonoBehaviour
{
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI finalScoreText;
    public GameObject pauseScreen;
    public GameObject endOfLevelScreen;
    public GameObject deathScreen;
    public GameObject defeatScreen;

    public static event Action onPauseScreenToggle;
    public static event Action onNextLevelButtonPressed;
    public static event Action onLevelRestart;

    void OnEnable()
    {
        GameManager.onPauseToggle += TogglePauseScreen;
        GameManager.onGameEnd += ActivateDefeatScreen;
        LevelManager.onCurrentLevelUpdate += UpdateLevel;
        GameManager.onScoreUpdate += UpdateScore;
        GameManager.onLivesUpdate += UpdateLives;
        GameManager.onTimeUpdate += UpdateTime;
        PlayerController.onLevelEndReached += ActivateEndOfLevelScreen;
        PlayerController.onDeath += ActivateDeathScreen;
    }

    void OnDisable()
    {
        GameManager.onPauseToggle -= TogglePauseScreen;
        GameManager.onGameEnd -= ActivateDefeatScreen;
        LevelManager.onCurrentLevelUpdate -= UpdateLevel;
        GameManager.onLivesUpdate -= UpdateLives;
        GameManager.onScoreUpdate -= UpdateScore;
        GameManager.onTimeUpdate -= UpdateTime;
        PlayerController.onLevelEndReached -= ActivateEndOfLevelScreen;
        PlayerController.onDeath -= ActivateDeathScreen;
    }

    void TogglePauseScreen(bool state)
    {
        pauseScreen.SetActive(state);

        //SoundManager.Get().PlaySound(SoundManager.Sounds.Button);
    }

    void UpdateScore(int score)
    {
        scoreText.text = "Score: " + score;
    }

    void UpdateLevel(int level)
    {
        levelText.text = "Level: " + level;
    }

    void UpdateLives(int lives)
    {
        livesText.text = "Lives: " + lives;
    }

    void UpdateTime(float time)
    {
        timeText.text = "Time: " + ((int)(time / 60)).ToString() + ":" + ((int)(time % 60)).ToString("00");
    }

    void ActivateEndOfLevelScreen()
    {
        endOfLevelScreen.SetActive(true);
    }

    void ActivateDeathScreen()
    {
        deathScreen.SetActive(true);
    }

    void ActivateDefeatScreen(int finalScore)
    {
        finalScoreText.text = "Final score: " + finalScore;

        defeatScreen.SetActive(true);
    }

    public void TogglePause()
    {
        if (onPauseScreenToggle != null) onPauseScreenToggle();
    }

    public void GoToNextLevel()
    {
        endOfLevelScreen.SetActive(false);

        if (onNextLevelButtonPressed != null) onNextLevelButtonPressed();

        //SoundManager.Get().PlaySound(SoundManager.Sounds.Button);
    }

    public void RestartLevel()
    {
        deathScreen.SetActive(false);

        if (onLevelRestart != null) onLevelRestart();

        //SoundManager.Get().PlaySound(SoundManager.Sounds.Button);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");

        //SoundManager.Get().PlaySound(SoundManager.Sounds.Button);
    }
}