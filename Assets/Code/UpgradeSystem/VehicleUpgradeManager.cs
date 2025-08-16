using UnityEngine;

public class VehicleUpgradeManager : MonoBehaviour
{
    [SerializeField] public Stat engineStat;
    [SerializeField] public Stat turnSpeedStat;
    [SerializeField] public Stat fuelStat;
    [SerializeField] public Stat fuelEfficiencyStat;
    [SerializeField] public Stat AccelerationStat;
    [SerializeField] PlayerSpaceship spaceship;

    void Start()
    {
        UpdatePlayerModifiers();
    }

    // Accessess the player and refreshes its Modifiers acc to the newer ones from the Serialized   Stats
    public void UpdatePlayerModifiers()
    {
        spaceship.SetUpgrades(
            engineModifier: engineStat.UpgradeModifier,
            fuelModifier: fuelStat.UpgradeModifier,
            turnSpeedModifier: turnSpeedStat.UpgradeModifier,
            fuelReductionModifier: fuelEfficiencyStat.UpgradeModifier
            );
        spaceship.SetVehicleMaxAttributes(maxEngineSpeed: engineStat.upgrades[engineStat.upgrades.Length - 1].upgradeModifier);
    }
}
