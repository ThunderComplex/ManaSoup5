using UnityEngine;
using UnityEngine.UI;

public class PointCounter : MonoBehaviour
{
    public int currentPoints = 0;


    public TMPro.TextMeshProUGUI pointText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void AddPoints(int points)
    {
        currentPoints += points;
        pointText.text = "Points: " + currentPoints;
    }
}
