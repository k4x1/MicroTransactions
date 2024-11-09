using System.Collections;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{

    private Rigidbody rb;
    private float distanceToTarget;
    private SpriteRenderer spriteRenderer;

    [Header("Movement Stuff")]
    [SerializeField] private float detectRange;
    public GameObject target;
    public float speed = 0.1f;
    public float speedCap = 5.0f;
    public float turnDelay = 0.5f; // Sets speed to this number when turning so slide isn't that bad
    public float turnSpeed = 0.4f;
    public float health = 100;


    [Header("Scale Stuff")]
    private Vector3 initialScale;
    [SerializeField] private float shrinkDuration = 5f;
    [SerializeField] private float minScaleFactor = 0.1f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        target = GameObject.FindGameObjectWithTag("Player");
        initialScale = transform.localScale;
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(ShrinkAndDestroy());
        }
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

    private IEnumerator ShrinkAndDestroy()
    {
        float elapsedTime = 0f;
        Vector3 startScale = transform.localScale;
        Vector3 endScale = initialScale * minScaleFactor;
        float startMass = rb.mass;
        float endMass = startMass * minScaleFactor;

        while (elapsedTime < shrinkDuration && transform.localScale.magnitude > endScale.magnitude)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / shrinkDuration);
            transform.localScale = Vector3.Lerp(startScale, endScale, t);
            rb.mass = Mathf.Lerp(startMass, endMass, t);

            yield return null;
        }

        transform.localScale = endScale;
        rb.mass = endMass;
        Destroy(gameObject);
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
