using UnityEngine;

public class BallScript : MonoBehaviour
{
    public float speed = 5f;
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

        // Move in local z direction (forward)
        moveDirection = transform.forward;
        moveDirection.y = 0;
        moveDirection.Normalize();
        rb.linearVelocity = moveDirection * speed;
    }


    void OnCollisionEnter(Collision collision)
    {
        // Ignore collisions with objects tagged "Player"
        if (collision.gameObject.CompareTag("Player"))
        {
            Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
            return;
        }
        // Reflect the move direction based on the collision normal
        Vector3 normal = collision.contacts[0].normal;
        moveDirection = Vector3.Reflect(moveDirection, normal);
        moveDirection.y = 0;
        moveDirection.Normalize();
        rb.linearVelocity = moveDirection * speed;
    }
}
