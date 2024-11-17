/// <summary>
/// Bachelor of Software Engineering
/// Media Design School
/// Auckland
/// New Zealand
/// (c) 2024 Media Design School
/// File Name : InputModeDropdown.cs
/// Description : This class manages the input mode selection dropdown in the UI.
///               It handles the initialization of the dropdown options,
///               updates the input mode when the selection changes,
///               and manages the visibility of the reset gyroscope button.
/// Author : Kazuo Reis de Andrade
/// </summary>
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class InputModeDropdown : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown inputModeDropdown;
    [SerializeField] private LevelMovement levelMovement;
    [SerializeField] private GameObject resetGyroscopeButton;

    private void Start()
    {
        if (inputModeDropdown == null)
        {
            inputModeDropdown = GetComponent<TMP_Dropdown>();
        }

        if (inputModeDropdown != null)
        {
            inputModeDropdown.ClearOptions();

            List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>
            {
                new TMP_Dropdown.OptionData("Joystick"),
                new TMP_Dropdown.OptionData("Relative Touch"),
                new TMP_Dropdown.OptionData("Gyroscope")
            };
            inputModeDropdown.AddOptions(options);

            inputModeDropdown.value = (int)InputManager.Instance.CurrentInputMode;

            inputModeDropdown.onValueChanged.AddListener(OnInputModeChanged);
        }
        else
        {
            Debug.LogError("dropdown not assigned or found");
        }

        UpdateResetGyroscopeButtonVisibility();
    }

    private void OnInputModeChanged(int index)
    {
        InputMode newMode = (InputMode)index;
        if (GameManager.Instance.gameStarted)
        {
            UiManager.Instance.SwitchInputMode(newMode);
        }
        else { 
            InputManager.Instance.SetInputMode(newMode);
        }
        UpdateResetGyroscopeButtonVisibility();
    }

    public void ResetGyroscope()
    {
        if (levelMovement != null)
        {
            levelMovement.ResetGyroscope();
        }
        else
        {
            InputManager.Instance.ResetGyroscope();
        }
    }

    private void UpdateResetGyroscopeButtonVisibility()
    {
        if (resetGyroscopeButton != null)
        {
            resetGyroscopeButton.SetActive(InputManager.Instance.CurrentInputMode == InputMode.GYROSCOPE);
        }
    }
}
