/// <summary>
/// Bachelor of Software Engineering
/// Media Design School
/// Auckland
/// New Zealand
/// (c) 2024 Media Design School
/// File Name : GameManager.cs
/// Description : This class is responsible for managing the overall game state and flow.
///               It handles game initialization, restarts, scoring, timer management,
///               achievement unlocking, and transitions between game states.
/// Author : Kazuo Reis de Andrade
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public bool won = false;
    public bool reset = false;
    public bool gameStarted = false;
    public GameObject playerRef;
    public ColorToPrefab[] colorMappings;
    public float points;
    public Timer time;
    public float maxPointsPerAcquisition = 100f; // Maximum points for instant acquisition
    public float baseTime = 1f; // Base time for max points

    private float lastPointTime;

    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        lastPointTime = Time.time;
    }

    public void RestartTimer()
    {
        time.StartTimer();
    }
  

    public void AddPoints()
    {
        float elapsedTime = Time.time - lastPointTime;

        float pointsToAdd;
        if (elapsedTime < 1f)
        {
            pointsToAdd = maxPointsPerAcquisition;
        }
        else
        {
            pointsToAdd = Mathf.Clamp(maxPointsPerAcquisition / (elapsedTime + baseTime), 0, maxPointsPerAcquisition);
        }

        points += pointsToAdd;
        lastPointTime = Time.time;
        if(points> 200)
        {
            UnlockAchievement(GPGSIds.achievement_points_a_plenty);
        }    
        UiManager.Instance.UpdatePoints(points);
    }

    public void SetWin()
    {
        won = true;
    }
    public void UnlockAchievement(string achievementId)
    {
        if (Social.localUser.authenticated)
        {
            Social.ReportProgress(achievementId, 100.0f, (bool success) =>
            {
                Debug.Log($"Achievement '{achievementId}' unlocked: {success}");
                if (success)
                {
                    // Display the achievements UI
                    Social.ShowAchievementsUI();
                }
            });
        }
        else
        {
            Debug.LogWarning("User is not authenticated. Cannot unlock achievements.");
        }
    }
    public void StartGame()
    {
        gameStarted = true;
        SceneManager.LoadScene("Game");
        playerRef = GameObject.FindGameObjectWithTag("Player");
    }

    public void MainMenu()
    {
        gameStarted = false;
        PauseManager.Instance.SetPaused(false);
        SceneManager.LoadScene("MainMenu");
    }

    public void RestartGame()
    {
        ResetPlayer();
        ResetLevelGeneration();
        ResetGameState();
       
    }

    private void ResetPlayer()
    {
        if (playerRef != null)
        {
            PlayerScript playerScript = playerRef.GetComponent<PlayerScript>();
            if (playerScript != null)
            {
                playerScript.ResetPlayer();
            }
        }
    }

    private void ResetLevelGeneration()
    {
        LevelGenerator generator = FindObjectOfType<LevelGenerator>();
    
        if (generator != null)
        {
            generator.ResetGeneratedRooms();
        }
    }
    public void RevivePlayer()
    {
     
        if (playerRef != null)
        {
            PlayerScript playerScript = playerRef.GetComponent<PlayerScript>();
            if (playerScript != null)
            {
                playerScript.RevivePlayer();
            }
        }
        WaveController wave = FindObjectOfType<WaveController>();
        wave.waveZPosition = playerRef.transform.position.z - 150;
       CurrencySystem.Instance.AddCurrency(-100);

    }
    private void ResetGameState()
    {
        won = false;
        reset = true;
        gameStarted = true;
    
        points = 0;
        lastPointTime = Time.time;
        if (time != null)
        {
            time.RestartTimer();
        }
        if (UiManager.Instance != null)
        {
            UiManager.Instance.UpdatePoints(points);
        }
        WaveController wave = FindObjectOfType<WaveController>();
        wave.waveZPosition = playerRef.transform.position.z - 150;
    }
}
