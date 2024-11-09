using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum InputMode
{
    JOYSTICK,
    RELATIVE_TOUCH,
    GYROSCOPE
}


public class UiManager : MonoBehaviour
{
    public static UiManager Instance { get; private set; }
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject loseMenu;
    [SerializeField] GameObject winMenu;
    [SerializeField] GameObject joystick;
    [SerializeField] GameObject resetGyroButton;
    [SerializeField] Timer timer;
    [SerializeField] TMP_Text points;
    [SerializeField] TMP_Text gems;
    private bool updateGems = false;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
          /*  DontDestroyOnLoad(gameObject);*/
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        SwitchInputMode(InputManager.Instance.CurrentInputMode);
    }
    public void SwitchInputMode(InputMode mode)
    {
        joystick.SetActive(InputMode.JOYSTICK == mode);
        if (InputMode.GYROSCOPE == mode)
        {
            resetGyroButton.SetActive(true);
            resetGyroButton.GetComponentInChildren<Button>().onClick.AddListener(ResetGyroscope);
        }
        else
        {
            resetGyroButton.SetActive(false);
            resetGyroButton.GetComponentInChildren<Button>().onClick.RemoveAllListeners();
        }
        InputManager.Instance.SetInputMode(mode);
    }

    public void ResetGyroscope()
    {
        InputManager.Instance.ResetGyroscope();
    }
    public void SetPauseMenu(bool paused)
    {
        pauseMenu.SetActive(paused);
    }  
    public void SetLoseMenu(bool active)
    {
        if (!active)
        {
            timer.StartTimer();
            GameManager.Instance.RestartGame();
            updateGems = false;
        }
        else if (!updateGems) 
        {
            CurrencySystem.Instance.AddCurrency((int)GameManager.Instance.points / 10);
            UiManager.Instance.UpdateGems(CurrencySystem.Instance.GetCurrency());
            updateGems = true;
        }
        PauseManager.Instance.SetPaused(active);
        loseMenu.SetActive(active);
    }
    public void RevivePlayer()
    {
        GameManager.Instance.RevivePlayer();
        SetLoseMenu(false);
        PauseManager.Instance.SetPaused(false);
    }

    public void SetWinMenu(bool active)
    {
        if (!active)
        {
            timer.StartTimer();
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().ResetPlayer();
        }
        PauseManager.Instance.SetPaused(active);
        winMenu.SetActive(active);
    }
    public void UpdatePoints(float pointsText)
    {
        points.text = "Points: " + (int)pointsText; 
    }  
    public void UpdateGems(float gemsText)
    {
        gems.text = "Gems: " + (int)gemsText; 
    }       

}