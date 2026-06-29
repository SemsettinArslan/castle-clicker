using UnityEngine;

[CreateAssetMenu(fileName = "NewSkill", menuName = "CastleClicker/Skill")]
public class SkillData : ScriptableObject
{
    public SkillType skillType;
    public string title;
    [TextArea(2, 4)] public string description;
    public Sprite icon;

    public float cooldown = 30f;
    public float duration = 10f; // 0 if instant effect
    public double goldCost = 0;

    public SkillEffect effect;
}
