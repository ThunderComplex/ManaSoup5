
using UnityEngine;

public interface IEnemyMovement
{
    void Move(ref Vector3 targetPosition, ref Rigidbody rigidbody);
}
