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
}
