﻿using System.Collections;
using System.Collections.Generic;
using BayatGames.SaveGameFree;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI gameScoreText;
    public TextMeshProUGUI endGameScoreText;
    public TextMeshProUGUI endGameHighScoreText;
    private Camera camera;
    private Player player;
    public int scoreRefreshToSkip = 30;
    public float baseScoreMultiplier = 100f;
    public float[] velocityThresholds = { 1.25f, 2f }; // ascending values [n]
    public float[] velocityMultipliers = { 1f, 2f, 4f }; // ascending values [n+1]
    public float[] scoreThresholds = { 5000f, 20000f }; // ascending values [n]
    public float[] scoreMultipliers = { 1f, 1.25f, 1.5f }; // ascending values [n+1]

    int m_GameScore = 0;
    int m_GameHighScore = 0;
    int m_SkippedScoreRefreshes = 0;

    void Awake()
    {
        camera = FindObjectOfType<Camera>();
        player = FindObjectOfType<Player>();
    }

    void Start()
    {
        if (SaveGame.Exists("highScore"))
        {
            m_GameHighScore = SaveGame.Load<int>("highScore");
        }
    }

    void OnDestroy()
    {
        SaveGame.Save<int>("highScore", m_GameHighScore);
    }

    void FixedUpdate()
    {
        float scoreToAdd = camera.GetVelocity().z * Time.fixedDeltaTime * baseScoreMultiplier;

        {
            int scrTreshIdx = 0;
            for (; scrTreshIdx < scoreThresholds.Length; ++scrTreshIdx)
            {
                if (m_GameScore < scoreThresholds[scrTreshIdx])
                {
                    break;
                }
            }
            scoreToAdd *= scoreMultipliers[scrTreshIdx];
            //Debug.Log("Current score idx: " + scrTreshIdx);
        }

        {
            int velTreshIdx = 0;
            float currentLinearVelocity = player.GetVelocity().magnitude;
            //Debug.Log("Current velocity: " + currentLinearVelocity);
            for (; velTreshIdx < velocityThresholds.Length; ++velTreshIdx)
            {
                if (currentLinearVelocity < velocityThresholds[velTreshIdx])
                {
                    break;
                }
            }
            //Debug.Log("Chosen vel idx: " + velTreshIdx);
            scoreToAdd *= velocityMultipliers[velTreshIdx];
        }

        m_GameScore += Mathf.RoundToInt(scoreToAdd);
        if (m_GameHighScore < m_GameScore)
        {
            m_GameHighScore = m_GameScore;
        }

        if (m_SkippedScoreRefreshes == scoreRefreshToSkip)
        {
            gameScoreText.text = m_GameScore.ToString();
            m_SkippedScoreRefreshes = 0;
        }
        SetEndGameScores();
        ++m_SkippedScoreRefreshes;
    }

    void SetEndGameScores()
    {
        endGameScoreText.text = m_GameScore.ToString();
        endGameHighScoreText.text = m_GameHighScore.ToString();
    }
}