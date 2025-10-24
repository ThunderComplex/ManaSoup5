using UnityEngine;

public class PushPlayer : MonoBehaviour
{
    // Push force to apply to the player
    public float pushForce = 10f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Push in world z direction
                rb.AddForce(Vector3.forward * pushForce, ForceMode.Impulse);
            }
        }
    }
}
