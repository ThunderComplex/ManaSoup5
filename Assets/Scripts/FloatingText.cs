using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class FloatingText : MonoBehaviour
{
    [Header("Animation Settings")]
    public float duration = 1.0f;
    public Vector3 moveOffset = new Vector3(0, 200f, 0);
    public AnimationCurve moveCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    public float fadeStartTime = 0.2f; // When to start fading (as percentage of duration)
    
    private RectTransform rectTransform;
    private TextMeshProUGUI tmpText;
    private Color startColor;
    private Vector3 startPos;
    
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        tmpText = GetComponent<TextMeshProUGUI>();
        
        if (rectTransform == null || tmpText == null)
        {
            Debug.LogWarning("FloatingText: Missing required components (RectTransform or TextMeshProUGUI)");
            Destroy(gameObject);
            return;
        }
        
        startColor = tmpText.color;
        startPos = rectTransform.localPosition;
        
        StartCoroutine(AnimateText());
    }
    
    public void Initialize(string text, Vector3 position, float customDuration = -1f)
    {
        if (tmpText != null)
        {
            tmpText.text = text;
        }
        
        if (rectTransform != null)
        {
            rectTransform.localPosition = position;
            startPos = position;
        }
        
        if (customDuration > 0f)
        {
            duration = customDuration;
        }
    }
    
    public void Initialize(string text, Vector3 position, Vector3 customMoveOffset, float customDuration = -1f)
    {
        Initialize(text, position, customDuration);
        moveOffset = customMoveOffset;
    }
    
    private IEnumerator AnimateText()
    {
        if (rectTransform == null || tmpText == null)
        {
            Destroy(gameObject);
            yield break;
        }
        
        Vector3 endPos = startPos + moveOffset;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            
            // Apply movement curve
            float easedT = moveCurve.Evaluate(t);
            rectTransform.localPosition = Vector3.Lerp(startPos, endPos, easedT);
            
            // Handle fade out
            if (t > fadeStartTime)
            {
                float fadeT = (t - fadeStartTime) / (1f - fadeStartTime);
                tmpText.color = Color.Lerp(startColor, endColor, fadeT);
            }
            
            yield return null;
        }
        
        // Animation complete, destroy the object
        Destroy(gameObject);
    }
    
    // Optional: Public method to destroy the text immediately
    public void DestroyImmediately()
    {
        StopAllCoroutines();
        Destroy(gameObject);
    }
}