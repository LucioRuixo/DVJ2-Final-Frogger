using System;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    public int scoreIncrement;
    [HideInInspector] public int score;

    public float movementSpeed;

    public static event Action<int> onScoreUpdate;

    void Start()
    {
        score = 0;

        if (onScoreUpdate != null) onScoreUpdate(score);
    }

    public void IncreaseScore()
    {
        score += scoreIncrement;

        if (onScoreUpdate != null) onScoreUpdate(score);
    }
}