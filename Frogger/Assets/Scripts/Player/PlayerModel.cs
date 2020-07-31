using System;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    [HideInInspector] public int score;

    public float movementSpeed;

    public static event Action<int> onScoreUpdate;

    void Start()
    {
        score = 0;

        if (onScoreUpdate != null)
            onScoreUpdate(score);
    }

    void UpdateScore(int value)
    {
        score += value;

        if (onScoreUpdate != null)
            onScoreUpdate(score);
    }
}