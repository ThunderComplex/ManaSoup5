
using UnityEngine;

public class PushPlayer : MonoBehaviour
{
    // Push force to apply to the player
    public float pushForce = 10f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                // Push in world z direction
                playerController.PushPlayer(Vector3.forward * pushForce);
            }
        }
    }
}
