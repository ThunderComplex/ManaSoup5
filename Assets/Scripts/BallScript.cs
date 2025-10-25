using UnityEngine;

public class BallScript : MonoBehaviour
{
    public float damageValue = 10f;
    public float speed = 5f;
    public int maxBounces = 3;
    public float maxLifeTime = 5f;
    public float explosionRadius = 0f;

    private int bounceCount = 0;
    private float lifeTimer = 0f;
    private Vector3 moveDirection;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;

        // Set this object to the Ball layer and ignore collisions with other Ball layer objects
        int ballLayer = LayerMask.NameToLayer("Ball");
        gameObject.layer = ballLayer;
        Physics.IgnoreLayerCollision(ballLayer, ballLayer);

        // Move in local z direction (forward)
        moveDirection = transform.forward;
        moveDirection.y = 0;
        moveDirection.Normalize();
        rb.linearVelocity = moveDirection * speed;
    }

    void Update()
    {
        lifeTimer += Time.deltaTime;
        if (lifeTimer >= maxLifeTime)
        {
            TriggerExplosion();
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Ignore collisions with objects tagged "Player" or on "Ball" layer
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.layer == LayerMask.NameToLayer("Ball"))
        {
            Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
            return;
        }
    
        // Deal damage to the enemy if it has a Health component
        EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
        if (enemyHealth != null && explosionRadius == 0)
        {
            enemyHealth.TakeDamage(damageValue);
        }
        TriggerExplosion();

        // Reflect the move direction based on the collision normal
        Vector3 normal = collision.contacts[0].normal;
        moveDirection = Vector3.Reflect(moveDirection, normal);
        moveDirection.y = 0;
        moveDirection.Normalize();
        rb.linearVelocity = moveDirection * speed;

        bounceCount++;
        if (bounceCount >= maxBounces)
        {
            
            Destroy(gameObject);
        }
    }

    private void TriggerExplosion()
    {
        if (explosionRadius <= 0) return;

        // Find all colliders within the explosion radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider collider in colliders)
        {
            // Check if the object has the "Enemy" tag or EnemyHealth component
            if (collider.CompareTag("Enemy") || collider.GetComponent<EnemyHealth>() != null)
            {
                EnemyHealth enemyHealth = collider.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(damageValue);
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        if (explosionRadius > 0)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, explosionRadius);
        }
    }
}
