using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class UpgradeButton : MonoBehaviour
{
    [SerializeField] Image upgradeIcon;
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text levelText;
    [SerializeField] TMP_Text costText;
    [SerializeField] Slider levelSlider;
    [SerializeField] Stat attachedStat;
    [SerializeField] AudioClip playClip;

    SFXPlayer sfxPlayer;
    Ease ease;

    public void Start()
    {
        RefreshButtonInfo();
        sfxPlayer = FindFirstObjectByType<SFXPlayer>();
        GetComponent<Button>().onClick.AddListener(() => sfxPlayer.PlaySFX(playClip));
    }

    public void UpgradeStat()
    {
        bool didUpgrade = attachedStat.TryUpgradeLevel();
        MenuUiHandler menuUiHandler = FindAnyObjectByType<MenuUiHandler>();

        if (didUpgrade)
        {
            menuUiHandler.UpgradeAnimateGarageVehicle();
        }
        else
        {
            transform.DOShakePosition(0.5f, strength: new Vector2(50, 0), vibrato: 10);
        }
        RefreshButtonInfo();
        menuUiHandler.RefreshUI();
    }

    public void SetProperties(StatInfo statInfo)
    {
        nameText.text = statInfo.name;

        levelText.text = "Level: " + statInfo.level + "/" + statInfo.maxLevel;

        levelSlider.maxValue = statInfo.maxLevel;
        levelSlider.value = statInfo.level;

        costText.text = statInfo.canUpgradeNext ? "$" + MoneyVisualFormatter.Format(statInfo.upgradeCost) : "Max";
        upgradeIcon.sprite = statInfo.upgradeImage;
        GetComponent<Button>().interactable = statInfo.canUpgradeNext;
    }

    public void RefreshButtonInfo() => SetProperties(attachedStat.GetCurrentStatInfo());
}


