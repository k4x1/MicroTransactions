/// <summary>
/// Bachelor of Software Engineering
/// Media Design School
/// Auckland
/// New Zealand
/// (c) 2024 Media Design School
/// File Name : UnityServicesInit.cs
/// Description : This class initializes Unity Services for the application.
///               It handles the asynchronous initialization of Unity Services,
///               allowing for environment-specific setup and error handling.
/// Author : Kazuo Reis de Andrade
/// </summary>
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;

public class UnityServicesInit : MonoBehaviour
{
    public string m_environment = "production";
    public bool m_initialised { get; private set; }

    async void Start()
    {
        if (m_initialised) return;
        try
        {
            var options = new InitializationOptions()
                .SetEnvironmentName(m_environment);
            await UnityServices.InitializeAsync(options);

            print("unity services initialised");
            m_initialised = true;
        }
        catch (System.Exception e)
        {
            print("UnityServices failed to init with Exception: " + e);
            m_initialised = false;
        }
    }
}
