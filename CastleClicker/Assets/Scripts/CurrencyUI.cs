using UnityEngine;
using TMPro;
public class CurrencyUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goldText;

    private void OnEnable()
    {
        // GameController henüz uyanmadýysa hata vermemesi için güvenli kontrol
        if (GameController.Instance != null && GameController.Instance.Currency != null)
        {
            GameController.Instance.Currency.OnGoldChanged += UpdateGoldText;
            UpdateGoldText(GameController.Instance.Currency.Gold);
        }
    }

    private void Start()
    {
        // Ýlk açýlýþta dinlemeyi garantiye alalým
        GameController.Instance.Currency.OnGoldChanged += UpdateGoldText;
        UpdateGoldText(GameController.Instance.Currency.Gold);
    }

    private void OnDisable()
    {
        if (GameController.Instance != null && GameController.Instance.Currency != null)
        {
            GameController.Instance.Currency.OnGoldChanged -= UpdateGoldText;
        }
    }

    private void UpdateGoldText(double currentGold)
    {
        goldText.text = $"Gold: {CurrencyFormatNumber(currentGold)}";
    }

    private string CurrencyFormatNumber(double value)
    {
        if (value >= 1_000_000_000)
            return (value / 1_000_000_000).ToString("0.##") + "B";

        if (value >= 1_000_000)
            return (value / 1_000_000).ToString("0.##") + "M";

        if (value >= 1_000)
            return (value / 1_000).ToString("0.##") + "K";

        return value.ToString("0");
    }
}