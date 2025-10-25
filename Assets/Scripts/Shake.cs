using UnityEngine;
using System.Collections;

public class Shake : MonoBehaviour
{
    [Header("Shake Settings")]
    public float shakeIntensity = 1f;
    public float shakeDuration = 0.3f;
    
    private Vector3 originalPosition;
    private bool isShaking = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originalPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    /// <summary>
    /// Shakes the GameObject in 3D space for the specified duration (default 1 second)
    /// </summary>
    public void ShakeObject()
    {
        if (!isShaking)
        {
            StartCoroutine(ShakeCoroutine());
        }
    }
    
    /// <summary>
    /// Shakes the GameObject in 3D space with custom parameters
    /// </summary>
    /// <param name="duration">Duration of the shake in seconds</param>
    /// <param name="intensity">Intensity of the shake</param>
    public void ShakeObject(float duration, float intensity)
    {
        if (!isShaking)
        {
            StartCoroutine(ShakeCoroutine(duration, intensity));
        }
    }
    
    private IEnumerator ShakeCoroutine(float duration = -1, float intensity = -1)
    {
        if (duration < 0) duration = shakeDuration;
        if (intensity < 0) intensity = shakeIntensity / 10;
        
        isShaking = true;
        originalPosition = transform.position;
        float elapsedTime = 0f;
        
        while (elapsedTime < duration)
        {
            // Generate random offset in 3D space
            Vector3 randomOffset = new Vector3(
                Random.Range(-intensity, intensity),
                Random.Range(-intensity, intensity),
                Random.Range(-intensity, intensity)
            );
            
            // Apply the shake offset to the original position
            transform.position = originalPosition + randomOffset;
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        // Return to original position
        transform.position = originalPosition;
        isShaking = false;
    }
}
