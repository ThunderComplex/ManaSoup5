using UnityEngine;
using System.Collections.Generic;

public class ShootBall : MonoBehaviour
{
    public GameObject ballPrefab;

    public List<GameObject> projectilePool = new List<GameObject>();
    private int currentProjectilePoolIndex = 0;

    private InputSystem_Actions _inputActions;

    void Awake()
    {
        _inputActions = InputManager.Controls;
    }

    void OnEnable()
    {
        _inputActions.Player.Attack.performed += OnAttackPerformed;
    }

    void OnDisable()
    {
        _inputActions.Player.Attack.performed -= OnAttackPerformed;
    }

    private void OnAttackPerformed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        Shoot();
    }

    public void Shoot()
    {
        ballPrefab = projectilePool[currentProjectilePoolIndex];
        currentProjectilePoolIndex++;
        if (currentProjectilePoolIndex >= projectilePool.Count)
        {
            currentProjectilePoolIndex = 0;
        }

        // Ray from camera to mouse position
        Ray ray = Camera.main.ScreenPointToRay(_inputActions.Player.MousePos.ReadValue<Vector2>());
        Vector3 targetPoint = transform.position + transform.forward;
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            targetPoint = hitInfo.point;
        }
        Vector3 direction = (targetPoint - transform.position).normalized;
        Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
        PoolingSystem.Instance.SpawnObject(ballPrefab, transform.position + Vector3.forward, rotation);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
