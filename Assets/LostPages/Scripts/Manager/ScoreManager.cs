using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public event Action<int, string> OnScoreChanged;

    private int scorePages = 0;
    private int scoreBooks = 0;
    private int healthNb = 3;
    public int ScorePages => scorePages;
    public int ScoreBooks => scoreBooks;
    public int HealthNb => healthNb;

    public void AddScorePages(int add)
    {
        scorePages += add;
        OnScoreChanged?.Invoke(scorePages, "pages");
    }

    public void AddScoreBooks(int add)
    {
        scoreBooks += add;
        OnScoreChanged?.Invoke(scoreBooks, "books");
    }

    public void SetHealth(int health)
    {
        healthNb += health;
        OnScoreChanged?.Invoke(healthNb, "health");
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

}
