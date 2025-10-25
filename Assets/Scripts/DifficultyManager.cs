using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager Instance;

    public int DifficultyLevel = 0;
    public float EnemyHealthMultiplier = 4.0f;
    public int KillsToIncreaseDifficulty = 10;
    public int CurrentKillCount = 0;

    public float EnemyHealth
    {
        get { return DifficultyLevel * EnemyHealthMultiplier; }
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RegisterKill()
    {
        CurrentKillCount++;
        if (CurrentKillCount >= KillsToIncreaseDifficulty)
        {
            DifficultyLevel++;
            KillsToIncreaseDifficulty += 10;
            GameObject.Find("GameController").GetComponent<PlayerProgression>().ShowUpgradeOptions();
        }
    }
}
