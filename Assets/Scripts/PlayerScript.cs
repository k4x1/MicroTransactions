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
    [SerializeField] private float maxRate = 50f; 

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
        Vector2 moveVector = InputManager.Instance.MovementVector;
        Vector3 targetVelocity = new Vector3(moveVector.x, 0, moveVector.y) * movementSpeed;

        if (moveVector.magnitude > 0)
        {
            float accelerationPercentage = Mathf.Clamp01(accelerationRate / maxRate);

            if (accelerationPercentage >= 1f)
            {
                rb.velocity = targetVelocity;
            }
            else if (accelerationPercentage > 0f)
            {
                rb.velocity = Vector3.MoveTowards(rb.velocity, targetVelocity, accelerationRate * Time.deltaTime);
            }
        }
        else
        {
            float decelerationPercentage = Mathf.Clamp01(decelerationRate / maxRate);

            if (decelerationPercentage > 0f)
            {
                if (decelerationPercentage >= 1f)
                {
                    rb.velocity = Vector3.zero;
                }
                else
                {
                    float decelerationAmount = Mathf.Lerp(0, maxSpeed, decelerationPercentage) * Time.deltaTime;
                    rb.velocity = Vector3.MoveTowards(rb.velocity, Vector3.zero, decelerationAmount);
                }
            }
            // If deceleration is 0, maintain current velocity
            else
            {
                // No change to velocity
            }
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

    public void SetMovementParameters(float _maxRate, float _acceleration, float _deceleration, float _maxSpeed)
    {
        this.accelerationRate = _acceleration;
        this.decelerationRate = _deceleration;
        this.maxSpeed = _maxSpeed;
        this.maxRate = _maxRate;
    }

    public void ResetPlayer()
    {
        transform.position = new Vector3(0, 1, 0);
        rb.velocity = Vector3.zero;
    }
}
