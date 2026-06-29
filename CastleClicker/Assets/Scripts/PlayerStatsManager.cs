using System;
using UnityEngine;

public class PlayerStatsManager
{
    // Temel tıklama gücü (Upgrade edilebilir)
    public double BaseClickPower { get; private set; }

    // Tıklama gücü çarpanı (Buff/Skill ile geçici artırılabilir)
    public double ClickMultiplier { get; private set; }

    // Mevcut toplam tıklama gücünü hesaplayan property
    public double CurrentClickPower => BaseClickPower * ClickMultiplier;

    // Pasif saniyelik altın gücü (Binalar/İşçiler ile artırılabilir)
    public double PassiveGoldPower { get; private set; }

    // Pasif saniyelik altın çarpanı
    public double PassiveGoldMultiplier { get; private set; }

    // Mevcut toplam saniyelik pasif geliri hesaplayan property
    public double CurrentPassiveGold => PassiveGoldPower * PassiveGoldMultiplier;

    // Stat değiştiğinde UI veya efektlerin tetiklenmesi için event
    public event Action OnStatsChanged;

    public PlayerStatsManager(double defaultClickPower = 1)
    {
        BaseClickPower = defaultClickPower;
        ClickMultiplier = 1.0;
        PassiveGoldPower = 0.0;
        PassiveGoldMultiplier = 1.0;
    }

    public void IncreaseBaseClickPower(double amount)
    {
        if (amount <= 0) return;
        BaseClickPower += amount;
        OnStatsChanged?.Invoke();
    }

    public void SetClickMultiplier(double multiplier)
    {
        if (multiplier < 0) return;
        ClickMultiplier = multiplier;
        OnStatsChanged?.Invoke();
    }

    public void IncreasePassiveGoldPower(double amount)
    {
        if (amount <= 0) return;
        PassiveGoldPower += amount;
        OnStatsChanged?.Invoke();
    }

    public void SetPassiveGoldMultiplier(double multiplier)
    {
        if (multiplier < 0) return;
        PassiveGoldMultiplier = multiplier;
        OnStatsChanged?.Invoke();
    }
}
