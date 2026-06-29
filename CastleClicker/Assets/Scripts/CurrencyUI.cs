using UnityEngine;
using TMPro;

public class CurrencyUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goldText;

    private readonly string[] suffixes = { "", "K", "M", "B", "T", "Qa", "Qi", "Sx", "Sp", "Oc", "No", "Dc" };

    private void OnEnable()
    {
        SubscribeToEvents();
    }

    private void Start()
    {
        SubscribeToEvents();
    }

    private void OnDisable()
    {
        UnsubscribeFromEvents();
    }

    private void SubscribeToEvents()
    {
        if (ServiceLocator.TryResolve<CurrencyManager>(out var currency))
        {
            currency.OnGoldChanged -= UpdateGoldText;
            currency.OnGoldChanged += UpdateGoldText;
            UpdateGoldText(currency.Gold);
        }
    }

    private void UnsubscribeFromEvents()
    {
        if (ServiceLocator.TryResolve<CurrencyManager>(out var currency))
        {
            currency.OnGoldChanged -= UpdateGoldText;
        }
    }

    private void UpdateGoldText(double currentGold)
    {
        if (goldText != null)
        {
            goldText.text = $"Gold: {CurrencyFormatNumber(currentGold)}";
        }
    }

    private string CurrencyFormatNumber(double value)
    {
        if (value < 1000) return value.ToString("0");
        int exp = (int)(System.Math.Log10(value) / 3);
        if (exp >= suffixes.Length) exp = suffixes.Length - 1;
        double num = value / System.Math.Pow(10, exp * 3);
        return num.ToString("0.##") + suffixes[exp];
    }
}
