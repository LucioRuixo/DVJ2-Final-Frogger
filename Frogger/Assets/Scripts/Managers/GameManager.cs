using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    bool onPause = false;

    [HideInInspector] public Vector2 screenBounds;

    public PlayerModel playerModel;

    public static event Action<bool> onPauseToggle;
    public static event Action<int> onGameEnd;

    void OnEnable()
    {
        UIManager_Gameplay.onResumeButtonPressed += TogglePause;

        PlayerController.onDeath += EndGame;
    }

    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            TogglePause();

            //SoundManager.Get().PlaySound(SoundManager.Sounds.Button);
        }
    }

    void OnDisable()
    {
        UIManager_Gameplay.onResumeButtonPressed -= TogglePause;

        PlayerController.onDeath -= EndGame;
    }

    void TogglePause()
    {
        Time.timeScale = onPause ? 1f : 0f;
        onPause = !onPause;

        if (onPauseToggle != null)
            onPauseToggle(onPause);
    }

    void EndGame(int score)
    {
        if (onGameEnd != null)
            onGameEnd(score);
    }
}