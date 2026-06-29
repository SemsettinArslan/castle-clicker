using System;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] private List<UpgradeData> availableUpgrades;
    
    // Maps UpgradeType to current level
    private readonly Dictionary<UpgradeType, int> upgradeLevels = new Dictionary<UpgradeType, int>();

    public List<UpgradeData> AvailableUpgrades => availableUpgrades;

    public event Action<UpgradeType, int> OnUpgradeLeveledUp; // Type, newLevel

    private void Awake()
    {
        ServiceLocator.Register<UpgradeManager>(this);
    }

    private void Start()
    {
        InitializeUpgrades();
    }

    private void InitializeUpgrades()
    {
        if (availableUpgrades == null) return;
        foreach (var upgrade in availableUpgrades)
        {
            if (upgrade != null && upgrade.upgradeType != UpgradeType.None)
            {
                upgradeLevels[upgrade.upgradeType] = 0; // Starts at level 0
            }
        }
    }

    public int GetUpgradeLevel(UpgradeType upgradeType)
    {
        return upgradeLevels.TryGetValue(upgradeType, out int lvl) ? lvl : 0;
    }

    public double GetUpgradeCost(UpgradeData upgrade)
    {
        int currentLvl = GetUpgradeLevel(upgrade.upgradeType);
        return upgrade.baseCost * Math.Pow(upgrade.costMultiplier, currentLvl);
    }

    public bool TryBuyUpgrade(UpgradeType upgradeType)
    {
        if (availableUpgrades == null) return false;
        
        UpgradeData upgrade = availableUpgrades.Find(u => u != null && u.upgradeType == upgradeType);
        if (upgrade == null) return false;

        int currentLvl = GetUpgradeLevel(upgrade.upgradeType);
        if (upgrade.maxLevel > 0 && currentLvl >= upgrade.maxLevel) return false;

        double cost = GetUpgradeCost(upgrade);
        
        if (ServiceLocator.TryResolve<CurrencyManager>(out var currency))
        {
            if (currency.TrySpendGold(cost))
            {
                upgradeLevels[upgradeType] = currentLvl + 1;
                ApplyUpgradeEffect(upgrade);
                OnUpgradeLeveledUp?.Invoke(upgradeType, upgradeLevels[upgradeType]);
                return true;
            }
        }
        return false;
    }

    private void ApplyUpgradeEffect(UpgradeData upgrade)
    {
        if (ServiceLocator.TryResolve<PlayerStatsManager>(out var stats))
        {
            switch (upgrade.statType)
            {
                case StatType.ClickPower:
                    stats.IncreaseBaseClickPower(upgrade.valuePerLevel);
                    break;
                case StatType.PassiveGoldGen:
                    stats.IncreasePassiveGoldPower(upgrade.valuePerLevel);
                    break;
            }
        }
    }

    private void OnDestroy()
    {
        ServiceLocator.Unregister<UpgradeManager>();
    }
}
