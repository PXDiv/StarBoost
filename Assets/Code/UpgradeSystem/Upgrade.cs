using System;

[Serializable]
public class Upgrade
{
    public int upgradeCost;
    public float upgradeModifier;
}

public enum UpgradeType
{
    EngineUpgrade, FuelUpgrade, GunUpgrade, BoostUpgrade
}