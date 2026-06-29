using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeUIItem : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private Button buyButton;

    private UpgradeData upgradeData;
    private UpgradeManager upgradeManager;

    public void Setup(UpgradeData data, UpgradeManager manager)
    {
        upgradeData = data;
        upgradeManager = manager;
        if (buyButton != null)
        {
            buyButton.onClick.RemoveAllListeners();
            buyButton.onClick.AddListener(OnBuyClicked);
        }
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (upgradeData == null || upgradeManager == null) return;

        if (titleText != null) titleText.text = upgradeData.title;
        
        int level = upgradeManager.GetUpgradeLevel(upgradeData.upgradeType);
        if (levelText != null) levelText.text = $"Lvl: {level}";

        double cost = upgradeManager.GetUpgradeCost(upgradeData);
        if (costText != null) costText.text = $"Cost: {FormatCost(cost)}";
        
        if (buyButton != null)
        {
            if (upgradeData.maxLevel > 0 && level >= upgradeData.maxLevel)
            {
                if (costText != null) costText.text = "MAX";
                buyButton.interactable = false;
            }
            else
            {
                buyButton.interactable = true;
            }
        }

        if (iconImage != null && upgradeData.icon != null)
        {
            iconImage.sprite = upgradeData.icon;
        }
    }

    private void OnBuyClicked()
    {
        if (upgradeManager.TryBuyUpgrade(upgradeData.upgradeType))
        {
            UpdateUI();
        }
    }

    private string FormatCost(double value)
    {
        if (value < 1000) return value.ToString("0");
        int exp = (int)(System.Math.Log10(value) / 3);
        string[] suffixes = { "", "K", "M", "B", "T", "Qa", "Qi", "Sx", "Sp" };
        if (exp >= suffixes.Length) exp = suffixes.Length - 1;
        double num = value / System.Math.Pow(10, exp * 3);
        return num.ToString("0.##") + suffixes[exp];
    }
}
