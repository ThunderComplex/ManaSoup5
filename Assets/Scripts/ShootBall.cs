using UnityEngine;

public class ShootBall : MonoBehaviour
{
    public GameObject ballPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
 
    }
    
    public void Shoot()
    {
        // Ray from camera to mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, transform.position); // XZ plane at player's Y
        float enter;
        Vector3 targetPoint = transform.position + transform.forward;
        if (plane.Raycast(ray, out enter))
        {
            targetPoint = ray.GetPoint(enter);
        }
        Vector3 direction = (targetPoint - transform.position).normalized;
        Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
        Instantiate(ballPrefab, transform.position, rotation);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }
}
