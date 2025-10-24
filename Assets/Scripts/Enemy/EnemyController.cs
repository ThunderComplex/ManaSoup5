using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform PlayerLocation;

    private Rigidbody _rigidbody;
    private IEnemyMovement _enemyMovement;

    void Awake()
    {
        TryGetComponent(out _rigidbody);
        TryGetComponent(out _enemyMovement);
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        var targetPosition = PlayerLocation.position;
        _enemyMovement.Move(ref targetPosition, ref _rigidbody);
        // Vector3 direction = (PlayerLocation.position - transform.position).normalized;
        // _rigidbody.MovePosition(transform.position + direction * Time.fixedDeltaTime);
    }
}
