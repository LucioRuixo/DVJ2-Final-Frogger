using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager_Gameplay : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public GameObject pauseScreen;
    public GameObject endOfLevelScreen;
    public GameObject deathScreen;
    public TextMeshProUGUI deathText;
    public TextMeshProUGUI finalScoreText;

    public static event Action onResumeButtonPressed;
    public static event Action onNextLevelButtonPressed;

    void OnEnable()
    {
        GameManager.onPauseToggle += ActivatePauseScreen;
        GameManager.onGameEnd += ActivateDeathScreen;

        LevelManager.onLevelEndReached += ActivateEndOfLevelScreen;

        PlayerModel.onScoreUpdate += UpdateScore;
    }

    void OnDisable()
    {
        GameManager.onPauseToggle -= ActivatePauseScreen;
        GameManager.onGameEnd -= ActivateDeathScreen;

        LevelManager.onLevelEndReached -= ActivateEndOfLevelScreen;

        PlayerModel.onScoreUpdate -= UpdateScore;
    }

    void UpdateScore(int score)
    {
        scoreText.text = "SCORE: " + score;
    }

    void ActivatePauseScreen(bool state)
    {
        pauseScreen.SetActive(state);
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

    public void UnpauseGame()
    {
        if (onResumeButtonPressed != null)
            onResumeButtonPressed();

        //SoundManager.Get().PlaySound(SoundManager.Sounds.Button);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");

        //SoundManager.Get().PlaySound(SoundManager.Sounds.Button);
    }

    public void GoToNextLevel()
    {
        endOfLevelScreen.SetActive(false);

        if (onNextLevelButtonPressed != null)
            onNextLevelButtonPressed();

        //SoundManager.Get().PlaySound(SoundManager.Sounds.Button);
    }
}