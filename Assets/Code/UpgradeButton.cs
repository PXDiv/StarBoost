using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    [SerializeField] Image upgradeIcon;
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text levelText;
    [SerializeField] TMP_Text costText;
    [SerializeField] Slider levelSlider;
    [SerializeField] Stat attachedStat;

    public void Start()
    {
        RefreshButtonInfo();
    }

    public void UpgradeStat()
    {
        attachedStat.TryUpgradeLevel();
        RefreshButtonInfo();
        FindAnyObjectByType<MenuUiHandler>().RefreshUI();
    }

    public void SetProperties(StatInfo statInfo)
    {
        nameText.text = statInfo.name;

        levelText.text = "Level: " + statInfo.level + "/" + statInfo.maxLevel;

        levelSlider.maxValue = statInfo.maxLevel;
        levelSlider.value = statInfo.level;

        costText.text = statInfo.canUpgradeNext ? "$" + statInfo.upgradeCost : "Max";
        upgradeIcon.sprite = statInfo.upgradeImage;
        GetComponent<Button>().interactable = statInfo.canUpgradeNext;
    }

    public void RefreshButtonInfo() => SetProperties(attachedStat.GetCurrentStatInfo());
}


