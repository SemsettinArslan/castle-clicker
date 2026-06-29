using UnityEngine;

[CreateAssetMenu(fileName = "GoldRushEffect", menuName = "CastleClicker/SkillEffects/GoldRush")]
public class GoldRushEffect : SkillEffect
{
    [SerializeField] private double multiplierIncrease = 1.0;
    
    public override void ApplyEffect()
    {
        if (ServiceLocator.TryResolve<PlayerStatsManager>(out var stats))
        {
            double oldMult = stats.ClickMultiplier;
            stats.SetClickMultiplier(oldMult + multiplierIncrease);
            Debug.Log($"[GoldRushEffect] Effect Applied: ClickMultiplier increased from {oldMult} to {stats.ClickMultiplier} (+{multiplierIncrease})");
        }
        else
        {
            Debug.LogError("[GoldRushEffect] ApplyEffect failed: PlayerStatsManager could not be resolved!");
        }
    }

    public override void RemoveEffect()
    {
        if (ServiceLocator.TryResolve<PlayerStatsManager>(out var stats))
        {
            double oldMult = stats.ClickMultiplier;
            stats.SetClickMultiplier(stats.ClickMultiplier - multiplierIncrease);
            Debug.Log($"[GoldRushEffect] Effect Removed: ClickMultiplier decreased from {oldMult} to {stats.ClickMultiplier} (-{multiplierIncrease})");
        }
        else
        {
            Debug.LogError("[GoldRushEffect] RemoveEffect failed: PlayerStatsManager could not be resolved!");
        }
    }
}
