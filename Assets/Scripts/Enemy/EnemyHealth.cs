using UnityEngine;
using UnityEngine.UI;
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

            DifficultyManager.Instance.RegisterKill();
            PoolingSystem.Instance.ReturnObject(gameObject);
        }
        else
        {
            //get shake component and shake the enemy
            Shake shake = GetComponent<Shake>();
            if (shake != null)
            {
                shake.ShakeObject();
            }
        }
    }

    private void ShowDamageText(float damage)
    {
        if (damageTextPrefab != null && canvas != null)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
            GameObject damageTextObj = Instantiate(damageTextPrefab, canvas.transform);

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
            RectTransform rectTransform = damageTextObj.GetComponent<RectTransform>();
            TMPro.TextMeshProUGUI tmpText = damageTextObj.GetComponent<TMPro.TextMeshProUGUI>();
            rectTransform.localPosition = canvasPos;
            tmpText.text = "-" + damage.ToString("F0");
            activeDamageTexts++;
            activeDamageTextObjects.Add(damageTextObj);
        }
    }

    void OnEnable()
    {
        maxHealth += DifficultyManager.Instance.EnemyHealth;
        currentHealth = maxHealth;
    }


    // Update is called once per frame
    void Update()
    {

    }
}
