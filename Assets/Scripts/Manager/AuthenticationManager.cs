/// <summary>
/// Bachelor of Software Engineering
/// Media Design School
/// Auckland
/// New Zealand
/// (c) 2024 Media Design School
/// File Name : AuthenticationManager.cs
/// Description : This class is responsible for handling user authentication using Google Play Games services.
///               It activates the Google Play Games platform and authenticates the local user.
/// Author : Kazuo Reis de Andrade
/// </summary>
using UnityEngine;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class AuthenticationManager : MonoBehaviour
{
    public bool m_authenticated { get; private set; }

    void Start()
    {
        // Activate the Google Play Games platform
        PlayGamesPlatform.Activate();

        // Authenticate the local user
        Social.localUser.Authenticate((bool _success) =>
        {
            if (_success)
            {
                Debug.Log("Authentication was successful");
                m_authenticated = true;
            }
            else
            {
                Debug.Log("Authentication failed");
                m_authenticated = false;
            }
        });
    }
}
