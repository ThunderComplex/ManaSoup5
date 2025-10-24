using UnityEngine;

public class ShootBall : MonoBehaviour
{
    public GameObject ballPrefab;

    private InputSystem_Actions _inputActions;

    void Awake()
    {
        _inputActions = InputManager.Controls;
    }

    void OnEnable()
    {
        _inputActions.Enable();

        _inputActions.Player.Attack.performed += ctx => Shoot();
    }

    void OnDisable()
    {
        _inputActions.Disable();

        _inputActions.Player.Attack.performed -= ctx => Shoot();

    }

    public void Shoot()
    {
        // Ray from camera to mouse position
        Ray ray = Camera.main.ScreenPointToRay(_inputActions.Player.MousePos.ReadValue<Vector2>());
        Vector3 targetPoint = transform.position + transform.forward;
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            targetPoint = hitInfo.point;
        }
        Vector3 direction = (targetPoint - transform.position).normalized;
        Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
        Instantiate(ballPrefab, transform.position + Vector3.forward * 2f, rotation);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
