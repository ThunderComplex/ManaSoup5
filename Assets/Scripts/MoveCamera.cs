using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    public float speed = 3f;

    void Update()
    {
    transform.Translate(0, 0, speed * Time.deltaTime, Space.World);
    }
}
