using System.Collections;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private Rigidbody rb;
    public GameObject target;
    [SerializeField] private float detectRange;
    private float distanceToTarget;
    private SpriteRenderer spriteRenderer;

    // Movement variables
    public float speed = 0.1f;
    public float speedCap = 5.0f;
    public float turnDelay = 0.5f; // Sets speed to this number when turning so slide isn't that bad
    public float turnSpeed = 0.4f;
    public float health = 100;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (TargetInAttackRange())
        {
            MoveTowardsTarget();
        }
    }

    private void MoveTowardsTarget()
    {
        Vector3 direction = target.transform.position - transform.position;
        direction.y = 0; // Keep movement on the horizontal plane
        rb.velocity += new Vector3(
            Mathf.Clamp(direction.x * speedCap, -speed, speedCap),
            0, // No vertical movement
            Mathf.Clamp(direction.z * speed, -speedCap, speedCap)
        );
    }

    private bool TargetInAttackRange()
    {
        distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
        return distanceToTarget <= detectRange;
    }

    public void TakeDamage(float damage)
    {
        spriteRenderer.color = new Color(255, 0, 50, 1);
        StartCoroutine(DamageAnimation(0.2f));
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator DamageAnimation(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        spriteRenderer.color = new Color(0, 0, 255, 1);
    }
}
