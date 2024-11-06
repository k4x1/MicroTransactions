using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform PlayerTransform;
    public float smoothTime = 0.3f;
    public Vector3 offset;
    public float lookAheadDistance = 2.0f;

    private Vector3 velocity = Vector3.zero;
    private Rigidbody playerRigidbody;

    private void Start()
    {
        if (PlayerTransform == null)
        {
            PlayerTransform = GameObject.FindWithTag("Player").transform;
        }
        playerRigidbody = PlayerTransform.GetComponent<Rigidbody>();
        offset = transform.position - PlayerTransform.position;
    }

    private void FixedUpdate()
    {
        Vector3 targetPosition = PlayerTransform.position + offset;

        // Calculate look-ahead position based on player's velocity
        Vector3 lookAheadPosition = playerRigidbody.velocity.normalized * lookAheadDistance;

        // Add look-ahead position to the target position
        targetPosition += lookAheadPosition;

        // Smoothly move the camera to the target position
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
