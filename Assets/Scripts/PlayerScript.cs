using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private InputMode inputMode = InputMode.JOYSTICK;
    [SerializeField] private float movementSpeed = 5.0f;
    [SerializeField] private float accelerationRate = 2.0f;
    [SerializeField] private float decelerationRate = 2.0f;
    [SerializeField] private float maxSpeed = 10.0f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        InputManager.Instance.SetInputMode(inputMode); // Set the input mode
    }

    void Update()
    {
        if (transform.position.y < -20)
        {
            UiManager.Instance.SetLoseMenu(true);
            PauseManager.Instance.Pause();
            ResetPlayer();
        }
    }

    private void FixedUpdate()
    {
        // Get the movement vector from the InputManager
        Vector2 moveVector = InputManager.Instance.MovementVector;

        // Convert the 2D movement vector to 3D
        Vector3 targetVelocity = new Vector3(moveVector.x, 0, moveVector.y) * movementSpeed;

        if (moveVector.magnitude > 0)
        {
            // Apply acceleration towards the target velocity
            rb.velocity += new Vector3(moveVector.x * accelerationRate, 0, moveVector.y * accelerationRate) * Time.deltaTime;
        }
        else
        {
            // Apply deceleration towards zero velocity
            rb.velocity = Vector3.MoveTowards(rb.velocity, Vector3.zero, decelerationRate * Time.deltaTime);
        }

        // Clamp the velocity to the max speed
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        UiManager.Instance.SetWinMenu(true);
    }

    public void SetMovementParameters(float acceleration, float deceleration, float maxSpeed)
    {
        this.accelerationRate = acceleration;
        this.decelerationRate = deceleration;
        this.maxSpeed = maxSpeed;
    }

    public void ResetPlayer()
    {
        transform.position = new Vector3(0, 1, 0);
        rb.velocity = Vector3.zero;
    }
}
