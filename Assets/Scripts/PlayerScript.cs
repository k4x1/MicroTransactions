using System.Collections;
using UnityEngine;
using TMPro;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private InputMode inputMode = InputMode.JOYSTICK;
    [SerializeField] private float movementSpeed = 5.0f;
    [SerializeField] private float accelerationRate = 2.0f;
    [SerializeField] private float decelerationRate = 2.0f;
    [SerializeField] private float maxSpeed = 10.0f;
    [SerializeField] private float maxRate = 50f;
    [SerializeField] private string bounceObjectTag = "BouncyObject";
    [SerializeField] private string speedObjectTag = "Speed";
    [SerializeField] private float reflectionFactor = 0.8f;
    [SerializeField] private float speedBoostDuration = 5f;
    [SerializeField] private float speedBoostMultiplier = 2f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody rb;
    private bool isBouncing = false;
    private float bounceTimer = 0f;
    private const float bounceDuration = 0.5f;
    private bool isOnSpeedSurface = false;
    private float originalMaxSpeed;
    private float originalAccelerationRate;

    private Timer speedBoostTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        InputManager.Instance.SetInputMode(inputMode);

        speedBoostTimer = gameObject.AddComponent<Timer>();
        speedBoostTimer.timerDuration = speedBoostDuration;
        speedBoostTimer.StopTimer(); // Initially stopped
        speedBoostTimer.SetOnTimerComplete(() => ResetSpeed());
    }
    void Update()
    {
        if (transform.position.y < -20)
        {
            UiManager.Instance.SetLoseMenu(true);
            PauseManager.Instance.Pause();
            ResetPlayer();
        }

        if (isBouncing)
        {
            bounceTimer -= Time.deltaTime;
            if (bounceTimer <= 0f)
            {
                isBouncing = false;
            }
        }

        CheckForSpeedSurface();
    }

    private void CheckForSpeedSurface()
    {
        RaycastHit hit;
        Vector3 rayOrigin = transform.position;
        Vector3 rayDirection = Vector3.down;

        if (Physics.Raycast(rayOrigin, rayDirection, out hit, 1f))
        {
            Debug.DrawRay(rayOrigin, rayDirection * hit.distance, Color.green, Time.deltaTime);
            if (hit.collider.CompareTag(speedObjectTag) && !isOnSpeedSurface)
            {
                ApplySpeedBoost();
            }
            else if (!hit.collider.CompareTag(speedObjectTag) && isOnSpeedSurface)
            {
               // ResetSpeed();
            }
        }
        else
        {
            if (isOnSpeedSurface)
            {
              //  ResetSpeed();
            }
        }
    }

    private void ApplySpeedBoost()
    {
        isOnSpeedSurface = true;
        originalMaxSpeed = maxSpeed;
        originalAccelerationRate = accelerationRate;

        maxSpeed *= speedBoostMultiplier;
        accelerationRate *= speedBoostMultiplier;

        speedBoostTimer.StartTimer();
    }

    private void ResetSpeed()
    {
        isOnSpeedSurface = false;
        maxSpeed = originalMaxSpeed;
        accelerationRate = originalAccelerationRate;
        speedBoostTimer.StopTimer();
    }

    private void FixedUpdate()
    {
        if (!isBouncing)
        {
            Vector2 moveVector = InputManager.Instance.MovementVector;

            if (moveVector.magnitude > 0)
            {
                Vector3 targetDirection = new Vector3(moveVector.x, 0, moveVector.y).normalized;
                Accelerate(targetDirection);
            }
            else
            {
                Decelerate();
            }

            // Ensure the velocity doesn't exceed max speed
            if (rb.velocity.magnitude > maxSpeed)
            {
                rb.velocity = rb.velocity.normalized * maxSpeed;
            }
        }
    }

    private void Accelerate(Vector3 targetDirection)
    {
        float accelerationPercentage = Mathf.Clamp01(accelerationRate / maxRate);

        Vector3 targetVelocity = targetDirection * maxSpeed;

        if (accelerationPercentage >= 1f)
        {
            rb.velocity = targetVelocity;
        }
        else
        {
            Vector3 acceleration = (targetVelocity - rb.velocity) * accelerationPercentage;
            rb.velocity += acceleration * Time.fixedDeltaTime;
        }
    }

    private void Decelerate()
    {
        float decelerationPercentage = Mathf.Clamp01(decelerationRate / maxRate);

        if (decelerationPercentage >= 1f)
        {
            rb.velocity = Vector3.zero;
        }
        else
        {
            Vector3 deceleration = -rb.velocity * decelerationPercentage;
            rb.velocity += deceleration * Time.fixedDeltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(bounceObjectTag))
        {
            ReflectPlayer(collision);
        }
    }

    private void ReflectPlayer(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];

        Vector3 incidentVector = rb.velocity.normalized;
        Vector3 normalVector = contact.normal;
        Vector3 reflectionVector = Vector3.Reflect(incidentVector, normalVector);

        float incomingSpeed = rb.velocity.magnitude;
        Vector3 newVelocity = reflectionVector * incomingSpeed * reflectionFactor;
        Debug.Log(rb.velocity + " | " + newVelocity);
        rb.velocity = newVelocity;
        isBouncing = true;
        bounceTimer = bounceDuration;
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
        isBouncing = false;
    }
}