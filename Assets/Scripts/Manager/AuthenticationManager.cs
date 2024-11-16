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
