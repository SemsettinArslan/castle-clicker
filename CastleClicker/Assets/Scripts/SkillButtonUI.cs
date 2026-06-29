using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillButtonUI : MonoBehaviour
{
    [SerializeField] private SkillType skillType;
    [SerializeField] private Button actionButton;
    [SerializeField] private Image cooldownImage; // Radial fill image
    [SerializeField] private TextMeshProUGUI cooldownText;

    private SkillManager skillManager;

    private void Start()
    {
        Debug.Log($"[SkillButtonUI] Start for skill type: {skillType}");
        
        // Defensive check in case OnEnable failed due to execution order
        GetSkillManager();

        if (actionButton != null)
        {
            actionButton.onClick.RemoveAllListeners();
            actionButton.onClick.AddListener(OnButtonClicked);
        }
        else
        {
            Debug.LogError($"[SkillButtonUI] Action Button is not assigned on GameObject {gameObject.name}!");
        }
    }

    private void OnEnable()
    {
        GetSkillManager();
    }

    private void OnDisable()
    {
        if (skillManager != null)
        {
            skillManager.OnSkillCooldownChanged -= UpdateCooldownUI;
            skillManager.OnSkillStateChanged -= UpdateSkillStateUI;
            skillManager = null; // Clear reference to resolve freshly next enable
        }
    }

    private SkillManager GetSkillManager()
    {
        if (skillManager == null)
        {
            if (ServiceLocator.TryResolve<SkillManager>(out var manager))
            {
                Debug.Log($"[SkillButtonUI] Resolved SkillManager for skill: {skillType}");
                skillManager = manager;
                skillManager.OnSkillCooldownChanged += UpdateCooldownUI;
                skillManager.OnSkillStateChanged += UpdateSkillStateUI;
            }
        }
        return skillManager;
    }

    private void OnButtonClicked()
    {
        Debug.Log($"[SkillButtonUI] Button clicked for: {skillType}. Requesting activation.");
        
        var manager = GetSkillManager();
        if (manager != null)
        {
            bool success = manager.TryActivateSkill(skillType);
            Debug.Log($"[SkillButtonUI] Activation request result for {skillType}: {success}");
        }
        else
        {
            Debug.LogError($"[SkillButtonUI] Cannot trigger skill {skillType}: SkillManager is null!");
        }
    }

    private void UpdateCooldownUI(SkillType type, float timer, float maxCooldown)
    {
        if (type != skillType) return;

        if (timer > 0)
        {
            if (cooldownImage != null)
            {
                cooldownImage.fillAmount = timer / maxCooldown;
                cooldownImage.enabled = true;
            }
            if (cooldownText != null)
            {
                cooldownText.text = timer.ToString("0.0");
                cooldownText.enabled = true;
            }
            if (actionButton != null)
            {
                actionButton.interactable = false;
            }
        }
        else
        {
            if (cooldownImage != null) cooldownImage.enabled = false;
            if (cooldownText != null) cooldownText.enabled = false;
            if (actionButton != null) actionButton.interactable = true;
        }
    }

    private void UpdateSkillStateUI(SkillType type, bool isActive)
    {
        if (type != skillType) return;

        Debug.Log($"[SkillButtonUI] Skill {skillType} state updated. IsActive: {isActive}");
        if (isActive)
        {
            if (actionButton != null)
            {
                actionButton.image.color = Color.yellow; // Highlight active state
            }
        }
        else
        {
            if (actionButton != null)
            {
                actionButton.image.color = Color.white; // Reset normal state
            }
        }
    }
}
