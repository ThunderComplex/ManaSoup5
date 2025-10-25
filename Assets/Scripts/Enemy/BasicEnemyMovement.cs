using UnityEngine;

public class BasicEnemyMovement : MonoBehaviour, IEnemyMovement
{
    public float Speed = 1f;
    public float WaypointThreshold = 1f;
    public int NumWaypoints = 3;
    public float MovementStride = 30f;

    private Vector3 _waypoint = Vector3.zero;

    public void Move(ref Vector3 targetPosition, ref Rigidbody rigidbody)
    {
        UpdateWaypoint(ref targetPosition);

        rigidbody.MovePosition(
            Vector3.MoveTowards(
                rigidbody.position,
                _waypoint,
                Speed * Time.fixedDeltaTime
            )
        );
    }

    private void UpdateWaypoint(ref Vector3 targetPosition)
    {
        var distanceToWaypoint = Vector3.Distance(transform.position, _waypoint);
        if (_waypoint == Vector3.zero || distanceToWaypoint <= WaypointThreshold)
        {
            GetNewWaypoint(ref targetPosition);
        }
    }

    private void GetNewWaypoint(ref Vector3 targetPosition)
    {
        var distance = Vector3.Distance(transform.position, targetPosition);
        var directionToTarget = (targetPosition - transform.position).normalized;

        var rotation = Quaternion.identity;

        if (NumWaypoints > 1)
        {
            rotation = Quaternion.Euler(0, Random.Range(-MovementStride, MovementStride), 0f);
        }

        _waypoint = transform.position + (rotation * directionToTarget * (distance / NumWaypoints));

        if (NumWaypoints > 1) NumWaypoints--;
    }
}
