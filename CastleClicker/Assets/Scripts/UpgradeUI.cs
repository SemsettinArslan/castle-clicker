using UnityEngine;
using System.Collections.Generic;

public class UpgradeUI : MonoBehaviour
{
    [SerializeField] private UpgradeUIItem itemPrefab;
    [SerializeField] private Transform contentParent;

    private readonly List<UpgradeUIItem> spawnedItems = new List<UpgradeUIItem>();
    private UpgradeManager upgradeManager;

    private void Start()
    {
        Debug.Log("[UpgradeUI] Start called.");
        InitializeUI();
    }

    private void OnEnable()
    {
        GetUpgradeManager();
    }

    private void OnDisable()
    {
        if (upgradeManager != null)
        {
            upgradeManager.OnUpgradeLeveledUp -= RefreshUI;
            upgradeManager = null; // Clear reference to resolve freshly next enable
        }
    }

    private UpgradeManager GetUpgradeManager()
    {
        if (upgradeManager == null)
        {
            if (ServiceLocator.TryResolve<UpgradeManager>(out var manager))
            {
                upgradeManager = manager;
                upgradeManager.OnUpgradeLeveledUp += RefreshUI;
                Debug.Log("[UpgradeUI] Resolved UpgradeManager successfully.");
            }
        }
        return upgradeManager;
    }

    private void InitializeUI()
    {
        var manager = GetUpgradeManager();
        if (manager != null)
        {
            if (contentParent != null)
            {
                Debug.Log("[UpgradeUI] Clearing existing children in Content Parent.");
                foreach (Transform child in contentParent)
                {
                    Destroy(child.gameObject);
                }
            }
            else
            {
                Debug.LogError("[UpgradeUI] Content Parent is NULL!");
            }
            
            spawnedItems.Clear();

            var upgrades = manager.AvailableUpgrades;
            if (upgrades == null)
            {
                Debug.LogWarning("[UpgradeUI] manager.AvailableUpgrades is NULL!");
                return;
            }

            Debug.Log($"[UpgradeUI] Found {upgrades.Count} upgrades in manager.");

            foreach (var upgrade in upgrades)
            {
                if (upgrade == null)
                {
                    Debug.LogWarning("[UpgradeUI] An upgrade element in the list is NULL!");
                    continue;
                }
                if (itemPrefab == null)
                {
                    Debug.LogError("[UpgradeUI] Item Prefab is NULL!");
                    continue;
                }

                Debug.Log($"[UpgradeUI] Instantiating prefab for upgrade: {upgrade.title}");
                var item = Instantiate(itemPrefab, contentParent);
                item.Setup(upgrade, manager);
                spawnedItems.Add(item);
            }
        }
        else
        {
            Debug.LogError("[UpgradeUI] Failed to resolve UpgradeManager from ServiceLocator!");
        }
    }

    private void RefreshUI(UpgradeType upgradeType, int newLevel)
    {
        foreach (var item in spawnedItems)
        {
            if (item != null)
            {
                item.UpdateUI();
            }
        }
    }
}
