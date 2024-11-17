/// <summary>
/// Bachelor of Software Engineering
/// Media Design School
/// Auckland
/// New Zealand
/// (c) 2024 Media Design School
/// File Name : LoseScreenMenu.cs
/// Description : This class manages the lose screen menu functionality.
///               It handles button click events for retrying the game and returning to the main menu.
/// Author : Kazuo Reis de Andrade
/// </summary>
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LoseScreenMenu : MonoBehaviour
{
    public Button RetryButton;
    public Button MainMenuButton;

    private void OnEnable()
    {
  
        MainMenuButton.onClick.AddListener(GameManager.Instance.MainMenu);

    }
    private void OnDisable()
    {
        MainMenuButton.onClick.RemoveAllListeners();


    }
   
}
