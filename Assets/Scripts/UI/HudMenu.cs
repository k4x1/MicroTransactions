/// <summary>
/// Bachelor of Software Engineering
/// Media Design School
/// Auckland
/// New Zealand
/// (c) 2024 Media Design School
/// File Name : HudMenu.cs
/// Description : This class manages the HUD (Heads-Up Display) menu functionality.
///               It handles the pause button click events, toggling the pause state,
///               and updating the pause menu UI.
/// Author : Kazuo Reis de Andrade
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HudMenu : MonoBehaviour
{
    public Button pauseButton;
    bool pause = true;
    private void OnEnable()
    {
        pauseButton.onClick.AddListener(PauseManager.Instance.TogglePause);
        pauseButton.onClick.AddListener(TurnOnPauseMenu);
      
            
        

    }
    private void OnDisable()
    {
        pauseButton.onClick.RemoveAllListeners();

     
    }
    void TurnOnPauseMenu()
    {
        UiManager.Instance.SetPauseMenu(pause);
        pause = !pause;
    }
}
