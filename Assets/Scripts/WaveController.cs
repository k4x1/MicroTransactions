using UnityEngine;
using System.Collections.Generic;

public class WaveController : MonoBehaviour
{
    public float initialSpeed = 1f;
    public float acceleration = 0.1f;
    public float waveZPosition = 0f;
    public LayerMask destroyableLayers;

    private float currentSpeed;

    void Start()
    {
        currentSpeed = initialSpeed;
    }

    void Update()
    {
        MoveWave();
        CheckAndDestroyObjects();
        IncreaseSpeed();
    }

    void MoveWave()
    {
        waveZPosition += currentSpeed * Time.deltaTime;
        transform.position = new Vector3(transform.position.x, transform.position.y, waveZPosition);
    }

    void CheckAndDestroyObjects()
    {
        Collider[] hitColliders = Physics.OverlapBox(
            new Vector3(transform.position.x, transform.position.y, waveZPosition),
            new Vector3(float.MaxValue, float.MaxValue, 0.1f),
            Quaternion.identity,
            destroyableLayers
        );
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.CompareTag("Player"))
            { 
                continue;
            }
            Destroy(hitCollider.gameObject);
        }
    }

    void IncreaseSpeed()
    {
        currentSpeed += acceleration * Time.deltaTime;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(
            new Vector3(transform.position.x, transform.position.y, waveZPosition),
            new Vector3(float.MaxValue, float.MaxValue, 0.1f)
        );
    }
}
