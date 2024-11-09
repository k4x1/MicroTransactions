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
