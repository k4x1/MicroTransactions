/// <summary>
/// Bachelor of Software Engineering
/// Media Design School
/// Auckland
/// New Zealand
/// (c) 2024 Media Design School
/// File Name : CameraMove.cs
/// Description : This class manages the camera movement in the game.
///               It handles smooth camera following of the player,
///               including look-ahead positioning and separate Z-axis smoothing.
/// Author : Kazuo Reis de Andrade
/// </summary>
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform PlayerTransform;
    public float smoothTime = 0.3f;
    public float zSmoothTime = 0.05f; 
    public Vector3 offset;
    public float lookAheadDistance = 2.0f;

    private Vector3 velocity = Vector3.zero;
    private Rigidbody playerRigidbody;
    private float zVelocity = 0f;

    private void Start()
    {
        if (PlayerTransform == null)
        {
            PlayerTransform = GameObject.FindWithTag("Player").transform;
        }
        playerRigidbody = PlayerTransform.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 targetPosition = PlayerTransform.position + offset;

        // Calculate look-ahead position based on player's velocity
        Vector3 lookAheadPosition = playerRigidbody.velocity.normalized * lookAheadDistance;

        // Add look-ahead position to the target position
        targetPosition += lookAheadPosition;

        // Separate Z component
        float targetZ = targetPosition.z;

        // Create a new vector with only X and Y components
        Vector3 xyTargetPosition = new Vector3(targetPosition.x, targetPosition.y, transform.position.z);

        // Smoothly move the camera's X and Y positions
        Vector3 smoothedXYPosition = Vector3.SmoothDamp(transform.position, xyTargetPosition, ref velocity, smoothTime);

        // Smooth Z position separately with a smaller smoothing factor
        float smoothedZ = Mathf.SmoothDamp(transform.position.z, targetZ, ref zVelocity, zSmoothTime);

        // Combine smoothed XY with slightly smoothed Z
        transform.position = new Vector3(smoothedXYPosition.x, smoothedXYPosition.y, smoothedZ);
    }
}
