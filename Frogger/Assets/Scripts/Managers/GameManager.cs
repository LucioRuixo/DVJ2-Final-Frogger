using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    bool onPause = false;

    public int initialLives;
    public int scoreIncrementBase;
    int lives;
    int score;

    float time;

    [HideInInspector] public Vector2 screenBounds;

    public CameraController cameraController;
    public GameObject playerPrefab;

    public static event Action<int> onScoreUpdate;
    public static event Action<int> onLivesUpdate;
    public static event Action<float> onTimeUpdate;
    public static event Action<bool> onPauseToggle;
    public static event Action<int> onGameEnd;

    void OnEnable()
    {
        UIManager_Gameplay.onPauseScreenToggle += TogglePause;
        UIManager_Gameplay.onLevelRestart += SetPlayer;
        UIManager_Gameplay.onLevelRestart += ResetTime;
        PlayerController.onLevelEndReached += IncreaseScore;
        PlayerController.onDeath += DecreaseLives;
    }

    void Start()
    {
        lives = initialLives;

        SetPlayer();

        if (onScoreUpdate != null) onScoreUpdate(score);
        if (onLivesUpdate != null) onLivesUpdate(lives);
    }

    void Update()
    {
        time += Time.deltaTime;
        if (onTimeUpdate != null) onTimeUpdate(time);

        if (Input.GetButtonDown("Pause"))
        {
            TogglePause();

            //SoundManager.Get().PlaySound(SoundManager.Sounds.Button);
        }
    }

    void OnDisable()
    {
        UIManager_Gameplay.onPauseScreenToggle -= TogglePause;
        UIManager_Gameplay.onLevelRestart -= SetPlayer;
        UIManager_Gameplay.onLevelRestart -= ResetTime;
        PlayerController.onLevelEndReached -= IncreaseScore;
        PlayerController.onDeath -= DecreaseLives;
    }

    void TogglePause()
    {
        Time.timeScale = onPause ? 1f : 0f;
        onPause = !onPause;

        if (onPauseToggle != null) onPauseToggle(onPause);
    }

    public void IncreaseScore()
    {
        score += scoreIncrementBase / (int)time;

        if (onScoreUpdate != null) onScoreUpdate(score);
    }

    void DecreaseLives()
    {
        lives--;

        if (onLivesUpdate != null) onLivesUpdate(lives);

        if (lives <= 0)
            if (onGameEnd != null) onGameEnd(score);
    }

    void UpdateTime()
    {
        if (onTimeUpdate != null) onTimeUpdate(time);
    }

    void ResetTime()
    {
        time = 0f;

        UpdateTime();
    }

    void SetPlayer()
    {
        Transform player = Instantiate(playerPrefab).transform;
        cameraController.SetPivot(player);
    }
}