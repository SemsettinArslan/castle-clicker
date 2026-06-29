using UnityEngine;

[CreateAssetMenu(fileName = "NewUpgrade", menuName = "CastleClicker/Upgrade")]
public class UpgradeData : ScriptableObject
{
    public UpgradeType upgradeType;
    public string title;
    [TextArea(2, 4)] public string description;
    public Sprite icon;
    
    public double baseCost = 10;
    public double costMultiplier = 1.15; // Exponential growth
    public int maxLevel = 0; // 0 for infinite

    public StatType statType;
    public double valuePerLevel = 1;
}
