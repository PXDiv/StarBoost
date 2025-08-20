using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Stats", menuName = "Scriptable Objects/Stats")]
public class Stat : ScriptableObject
{
    public int id;
    public Sprite icon;
    public string upgradeName;
    [SerializeField] int currentLevelIndex;
    public int CurrentLevel { get { return currentLevelIndex + 1; } }
    public Upgrade[] upgrades;

    [Header("Debug")]
    float upgradeModifier; public float UpgradeModifier { get { RefreshVariables(); return upgradeModifier; } }
    public bool canUpgrade = true;

    //internal Values
    int costToUpgrade; public int CostToUpgrade { get { RefreshVariables(); return costToUpgrade; } }


    //Sets its own Variables
    public void RefreshVariables()
    {
        LoadStat();
        int nextLevelIndex = currentLevelIndex + 1;

        if (upgrades.Length > nextLevelIndex)
        {
            costToUpgrade = upgrades[currentLevelIndex + 1].upgradeCost;
            canUpgrade = true;

        }
        else { costToUpgrade = 0; canUpgrade = false; }
        upgradeModifier = upgrades[currentLevelIndex].upgradeModifier;
    }

    public void SaveStat()
    {
        var key = upgradeName + id + "Stat";
        PlayerPrefs.SetInt(key, currentLevelIndex);
        Debug.Log("Saved Stat " + upgradeName + id + "Stat " + currentLevelIndex);
    }

    public void LoadStat()
    {
        var key = upgradeName + id + "Stat";
        currentLevelIndex = (PlayerPrefs.GetInt(key, 0));
    }

    public int GetMaxLevel()
    {
        return upgrades.Length;
    }

    public void SetLevel(int level)
    {
        currentLevelIndex = level;
        SaveStat();
        RefreshVariables();
    }

    private void UpgradeStat()
    {
        SetLevel(currentLevelIndex + 1);
        RefreshVariables();
    }

    public bool TryUpgrade()
    {
        int nextlevel = currentLevelIndex + 1;
        var currentMoneyAmount = MoneyMan.CurrentMoneyAmount;

        RefreshVariables();

        if (!canUpgrade)
        {
            Debug.Log(upgradeName + " Max Level Already Reached");
            return false;
        }

        if (CostToUpgrade > currentMoneyAmount || nextlevel > upgrades.Length - 1)
        { Debug.Log(upgradeName + " Money not enough"); ; return false; }

        MoneyMan.ReduceMoney(CostToUpgrade);
        UpgradeStat();
        return true;
    }

    public bool TryUpgradeLevel()
    {
        var didUpgrade = TryUpgrade();

        if (didUpgrade)
        {
            Debug.Log($"upgraded {this.upgradeName} to Level {this.CurrentLevel}");
            if (FindAnyObjectByType<VehicleUpgradeManager>() != null)
                FindAnyObjectByType<VehicleUpgradeManager>().UpdatePlayerModifiers();
            return true;
        }
        else
        {
            Debug.Log("Cannot Upgrade");
            return false;
        }

    }

    public StatInfo GetCurrentStatInfo()
    {
        RefreshVariables();

        return new StatInfo
        {
            name = upgradeName,
            level = CurrentLevel,
            maxLevel = GetMaxLevel(),
            upgradeCost = costToUpgrade,
            canUpgradeNext = canUpgrade,
            upgradeImage = icon,
        };
    }

}

public struct StatInfo
{
    public string name;
    public int level;
    public int maxLevel;
    public int upgradeCost;
    public bool canUpgradeNext;
    public Sprite upgradeImage;
}