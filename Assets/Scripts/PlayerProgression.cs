using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerProgression : MonoBehaviour
{
    public List<GameObject> projectilePool = new List<GameObject>();
    private GameObject player;
    private List<GameObject> playerProjectilePool = new List<GameObject>();
    public List<string> upgradeTextList = new List<string>();

    public GameObject upgradePannel;

    //+1 blueOrb 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerProjectilePool = player.GetComponent<ShootBall>().projectilePool;
        upgradeTextList.Add("+1x BlueOrb");
        upgradeTextList.Add("+1x MagicOrb");
        upgradeTextList.Add("+1x YellowOrb");
        upgradeTextList.Add("+1x FireBall");

        upgradeTextList.Add("+5 dmg to BlueOrb");
        upgradeTextList.Add("+5 dmg to MagicOrb");
        upgradeTextList.Add("+5 dmg to YellowOrb");
        upgradeTextList.Add("+5 dmg to FireBall");
    }

    public void ShowUpgradeOptions()
    {
        // Pause the game

        // Show the upgrade panel
        upgradePannel.SetActive(true);
        // choose 3 random upgrades from the upgradeTextList
        List<string> randomUpgrades = new List<string>();
        for (int i = 0; i < 3; i++)
        {
            int randomIndex = Random.Range(0, upgradeTextList.Count);
            randomUpgrades.Add(upgradeTextList[randomIndex]);
            // get the button and set the text
            Transform buttonTransform = upgradePannel.transform.GetChild(i);
            buttonTransform.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = randomUpgrades[i];
            // remove previous listeners
            buttonTransform.GetComponent<Button>().onClick.RemoveAllListeners();
            // add listener to the button
            string upgradeText = randomUpgrades[i];
            buttonTransform.GetComponent<Button>().onClick.AddListener(() => ApplyUpgrade(upgradeText));
        }
        Time.timeScale = 0.01f;
    }

    public void ApplyUpgrade(string upgradeText)
    {
        if (upgradeText.StartsWith("+1x"))
        {
            // Handle adding a new projectile
            playerProjectilePool.Add(projectilePool.Find(p => p.name == upgradeText.Substring(4).Trim()));
        }
        else if (upgradeText.Contains("dmg"))
        {
            // Handle upgrading an existing projectile
            string orbType = upgradeText.Substring(10).Trim();
            Debug.Log("Upgrading " + orbType);
            GameObject existingProjectile = playerProjectilePool.Find(p => p.name == orbType);
            //if (existingProjectile != null) 
            existingProjectile.GetComponent<BallScript>().damageValue += 5;
            projectilePool.Find(p => p.name == orbType).GetComponent<BallScript>().damageValue += 5;
        }
        upgradePannel.SetActive(false);
        Time.timeScale = 1f;
    }
    public bool testUpgrades = false;

    // Update is called once per frame
    void Update()
    {
        if (testUpgrades)
        {
            testUpgrades = false;
            ShowUpgradeOptions();
        }
    }
}
