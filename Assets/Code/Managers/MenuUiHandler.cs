using System.Collections.Generic;
using System.Linq;
using BrunoMikoski.AnimationSequencer;
using TMPro;
using UnityEngine;
public class MenuUiHandler : MonoBehaviour
{
    [SerializeField] TMP_Text moneyText;
    [SerializeField] UpgradeButton engineUpgradeButton, fuelUpgradeButton;
    [SerializeField] AnimationSequencer garagePlayerVehicleAnimationSequencer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Time.timeScale = 1;
        RefreshUI();
    }

    public void RefreshUI()
    {
        moneyText.text = MoneyVisualFormatter.Format(MoneyMan.CurrentMoneyAmount);

        List<UpgradeButton> upgradeButtons = FindObjectsByType<UpgradeButton>(FindObjectsInactive.Exclude, FindObjectsSortMode.None).ToList();
        upgradeButtons.ForEach(a => a.RefreshButtonInfo());
    }

    public void UpgradeAnimateGarageVehicle()
    {
        garagePlayerVehicleAnimationSequencer.Play();
    }
}
