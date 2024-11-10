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

    public bool adsEnabled = true;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

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

        UiManager.Instance.UpdatePoints(points);
    }

    public void SetWin()
    {
        won = true;
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
        Debug.Log(wave);
        wave.waveZPosition = playerRef.transform.position.z - 150;

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
    }
}
