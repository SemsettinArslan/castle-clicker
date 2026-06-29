using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillInstance
{
    public SkillData Data { get; private set; }
    public float CooldownTimer { get; set; }
    public float DurationTimer { get; set; }
    public bool IsActive { get; set; }

    public SkillInstance(SkillData data)
    {
        Data = data;
        CooldownTimer = 0f;
        DurationTimer = 0f;
        IsActive = false;
    }
}

public class SkillManager : MonoBehaviour
{
    [SerializeField] private List<SkillData> startingSkills;

    private readonly Dictionary<SkillType, SkillInstance> skillInstances = new Dictionary<SkillType, SkillInstance>();

    public event Action<SkillType, float, float> OnSkillCooldownChanged; // Type, timer, maxCooldown
    public event Action<SkillType, bool> OnSkillStateChanged; // Type, isActive

    private void Awake()
    {
        Debug.Log("[SkillManager] Awake: Registering to ServiceLocator.");
        ServiceLocator.Register<SkillManager>(this);
    }

    private void Start()
    {
        InitializeSkills();
    }

    private void InitializeSkills()
    {
        if (startingSkills == null)
        {
            Debug.LogWarning("[SkillManager] Starting Skills list is null!");
            return;
        }

        Debug.Log($"[SkillManager] Initializing starting skills. Count: {startingSkills.Count}");
        foreach (var skill in startingSkills)
        {
            if (skill != null && skill.skillType != SkillType.None)
            {
                Debug.Log($"[SkillManager] Registering skill: {skill.title} (Type: {skill.skillType})");
                skillInstances[skill.skillType] = new SkillInstance(skill);
            }
            else
            {
                Debug.LogWarning("[SkillManager] Found a null or None-type skill in starting list.");
            }
        }
    }

    public SkillInstance GetSkill(SkillType skillType)
    {
        return skillInstances.TryGetValue(skillType, out var inst) ? inst : null;
    }

    private void Update()
    {
        var keys = new List<SkillType>(skillInstances.Keys);
        foreach (var key in keys)
        {
            var skill = skillInstances[key];

            if (skill.IsActive)
            {
                skill.DurationTimer -= Time.deltaTime;
                if (skill.DurationTimer <= 0)
                {
                    Debug.Log($"[SkillManager] Skill duration ended: {skill.Data.title}");
                    DeactivateSkill(skill);
                }
            }
            else if (skill.CooldownTimer > 0)
            {
                skill.CooldownTimer -= Time.deltaTime;
                OnSkillCooldownChanged?.Invoke(skill.Data.skillType, skill.CooldownTimer, skill.Data.cooldown);
            }
        }
    }

    public bool TryActivateSkill(SkillType skillType)
    {
        Debug.Log($"[SkillManager] TryActivateSkill requested for: {skillType}");
        
        var skill = GetSkill(skillType);
        if (skill == null)
        {
            Debug.LogError($"[SkillManager] Skill type {skillType} is not registered/unlocked!");
            return false;
        }

        if (skill.IsActive)
        {
            Debug.LogWarning($"[SkillManager] Cannot activate {skill.Data.title}: Already active.");
            return false;
        }

        if (skill.CooldownTimer > 0)
        {
            Debug.LogWarning($"[SkillManager] Cannot activate {skill.Data.title}: Cooldown active ({skill.CooldownTimer:0.0}s remaining).");
            return false;
        }

        if (ServiceLocator.TryResolve<CurrencyManager>(out var currency))
        {
            if (skill.Data.goldCost > 0 && !currency.TrySpendGold(skill.Data.goldCost))
            {
                Debug.LogWarning($"[SkillManager] Cannot activate {skill.Data.title}: Insufficient gold (Cost: {skill.Data.goldCost}, Current: {currency.Gold}).");
                return false;
            }
        }
        else
        {
            Debug.LogWarning("[SkillManager] CurrencyManager could not be resolved, proceeding with free activation.");
        }

        ActivateSkill(skill);
        return true;
    }

    private void ActivateSkill(SkillInstance skill)
    {
        Debug.Log($"[SkillManager] Activating skill: {skill.Data.title}");
        skill.IsActive = true;
        skill.DurationTimer = skill.Data.duration;
        skill.CooldownTimer = skill.Data.cooldown;

        if (skill.Data.effect != null)
        {
            Debug.Log($"[SkillManager] Applying skill effect for: {skill.Data.title}");
            skill.Data.effect.ApplyEffect();
        }
        else
        {
            Debug.LogWarning($"[SkillManager] Skill {skill.Data.title} has no assigned effect.");
        }

        OnSkillStateChanged?.Invoke(skill.Data.skillType, true);
        OnSkillCooldownChanged?.Invoke(skill.Data.skillType, skill.CooldownTimer, skill.Data.cooldown);
    }

    private void DeactivateSkill(SkillInstance skill)
    {
        Debug.Log($"[SkillManager] Deactivating skill: {skill.Data.title}");
        skill.IsActive = false;
        skill.DurationTimer = 0f;

        if (skill.Data.effect != null)
        {
            Debug.Log($"[SkillManager] Removing skill effect for: {skill.Data.title}");
            skill.Data.effect.RemoveEffect();
        }

        OnSkillStateChanged?.Invoke(skill.Data.skillType, false);
    }

    private void OnDestroy()
    {
        Debug.Log("[SkillManager] OnDestroy: Unregistering from ServiceLocator.");
        ServiceLocator.Unregister<SkillManager>();
    }
}
