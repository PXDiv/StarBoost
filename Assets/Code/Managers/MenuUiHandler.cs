using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class MenuUiHandler : MonoBehaviour
{
    [SerializeField] TMP_Text moneyText;
    [SerializeField] UpgradeButton engineUpgradeButton, fuelUpgradeButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RefreshUI();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RefreshUI()
    {
        moneyText.text = MoneyMan.CurrentMoneyAmount.ToString("$000");

        List<UpgradeButton> upgradeButtons = FindObjectsByType<UpgradeButton>(FindObjectsInactive.Exclude, FindObjectsSortMode.None).ToList();
        upgradeButtons.ForEach(a => a.RefreshButtonInfo());
    }
}
