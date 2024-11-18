/// <summary>
/// Bachelor of Software Engineering
/// Media Design School
/// Auckland
/// New Zealand
/// (c) 2024 Media Design School
/// File Name : Bounce.cs
/// Description : This class handles the bouncing behavior when the player collides with certain objects.
///               It uses a trigger collider to detect player collisions and invokes the player's bounce method.
/// Author : Kazuo Reis de Andrade
/// </summary>
using UnityEngine;

public class Bounce : MonoBehaviour
{ 
    public string playerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Trigger Enter: {other.gameObject.tag}");
        if (other.gameObject.CompareTag(playerTag))
        {
            PlayerScript playerScript = other.GetComponent<PlayerScript>();
            if (playerScript != null)
            {
                playerScript.Bounce(transform.position);
            }
            else
            {
                Debug.LogError("Player object doesn't have PlayerScript component!");
            }
        }
    }
}
