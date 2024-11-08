using UnityEngine;

public class Bounce : MonoBehaviour
{
    public float reflectionFactor = 0.8f; // Controls how much of the incoming velocity is reflected
    public string playerTag = "Player";

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == playerTag)
        {
            Debug.Log("sss");
            ReflectPlayer(collision);
        }
    }

    private void ReflectPlayer(Collision collision)
    {
        Rigidbody playerRb = collision.rigidbody;
        ContactPoint contact = collision.contacts[0];

        // Calculate the reflection vector
        Vector3 incidentVector = playerRb.velocity.normalized;
        Vector3 normalVector = contact.normal;
        Vector3 reflectionVector = Vector3.Reflect(incidentVector, normalVector);

        // Apply the reflected velocity
        float incomingSpeed = playerRb.velocity.magnitude;
        Vector3 newVelocity = reflectionVector * incomingSpeed * reflectionFactor;

        playerRb.velocity = newVelocity;
    }
}
