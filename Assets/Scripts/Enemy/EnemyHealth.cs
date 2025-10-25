using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public GameObject damageTextPrefab;
    private Canvas canvas;
    private int activeDamageTexts = 0; // Track number of active damage texts
    private List<GameObject> activeDamageTextObjects = new List<GameObject>(); // Track active damage text objects
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        // Find the Canvas in the scene
        canvas = FindFirstObjectByType<Canvas>();
        if (canvas == null)
        {
            Debug.LogWarning("No Canvas found in the scene for damage text display");
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        
        // Show damage text
        ShowDamageText(damage);
        
        if (currentHealth <= 0)
        {
            // Find GameController and add points to pointCounter
            PointCounter pointCounter = FindFirstObjectByType<PointCounter>();
            if (pointCounter != null)
            {
                pointCounter.AddPoints(Mathf.RoundToInt(maxHealth / 2)); // Add points
            }

            // Clean up any active damage texts before destroying this enemy
            CleanupDamageTexts();
            Destroy(gameObject);
        }
    }
    
    private void ShowDamageText(float damage)
    {
        if (damageTextPrefab != null && canvas != null)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);  
            GameObject damageTextObj = Instantiate(damageTextPrefab, canvas.transform);
            RectTransform rectTransform = damageTextObj.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                // Convert screen position to canvas position
                Vector2 canvasPos;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    canvas.transform as RectTransform,
                    screenPos,
                    canvas.worldCamera,
                    out canvasPos
                );
                // Add offset based on number of active damage texts to prevent overlap
                float offsetX = (activeDamageTexts % 3) * 30f - 30f; // Spread horizontally
                float offsetY = (activeDamageTexts / 3) * 40f; // Stack vertically after 3 texts
                canvasPos.x += offsetX;
                canvasPos.y += offsetY;
                
                rectTransform.localPosition = canvasPos;
            }
            TMPro.TextMeshProUGUI tmpText = damageTextObj.GetComponent<TMPro.TextMeshProUGUI>();
            tmpText.text = "-" + damage.ToString("F0");
            activeDamageTexts++;
            activeDamageTextObjects.Add(damageTextObj);
            AnimateDamageText(damageTextObj, 1.0f);
        }
    }
    
    private void AnimateDamageText(GameObject damageText, float duration)
    {
        StartCoroutine(AnimateDamageTextCoroutine(damageText, duration));
    }
    
    private IEnumerator AnimateDamageTextCoroutine(GameObject damageText, float duration)
    {
        if (damageText == null) 
        {
            activeDamageTexts = Mathf.Max(0, activeDamageTexts - 1);
            yield break;
        }
        
        RectTransform rectTransform = damageText.GetComponent<RectTransform>();
        TMPro.TextMeshProUGUI tmpText = damageText.GetComponent<TMPro.TextMeshProUGUI>();
        
        if (rectTransform == null || tmpText == null) 
        {
            // Decrement counter and cleanup
            RemoveDamageTextFromTracking(damageText);
            if (damageText != null) Destroy(damageText);
            yield break;
        }
        
        Vector3 startPos = rectTransform.localPosition;
        Vector3 endPos = startPos + new Vector3(0, 200f, 0);
        Color startColor = tmpText.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);
        float elapsed = 0f;
        while (elapsed < duration && damageText != null && this != null)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            // Ease out quad curve
            float easedT = 1f - (1f - t) * (1f - t);
            // Animate position
            rectTransform.localPosition = Vector3.Lerp(startPos, endPos, easedT);
            // Animate fade out (start fading after 20% of duration)
            if (t > 0.2f)
            {
                float fadeT = (t - 0.2f) / 0.8f;
                tmpText.color = Color.Lerp(startColor, endColor, fadeT);
            }
            
            yield return null;
        }
        
        // Remove from tracking and cleanup
        RemoveDamageTextFromTracking(damageText);
        if (damageText != null)
        {
            Destroy(damageText);
        }
    }
    
    private void RemoveDamageTextFromTracking(GameObject damageText)
    {
        activeDamageTexts = Mathf.Max(0, activeDamageTexts - 1);
        if (activeDamageTextObjects.Contains(damageText))
        {
            activeDamageTextObjects.Remove(damageText);
        }
    }
    
    private void CleanupDamageTexts()
    {
        // Destroy all active damage text objects when this enemy is destroyed
        for (int i = activeDamageTextObjects.Count - 1; i >= 0; i--)
        {
            if (activeDamageTextObjects[i] != null)
            {
                Destroy(activeDamageTextObjects[i]);
            }
        }
        activeDamageTextObjects.Clear();
        activeDamageTexts = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
