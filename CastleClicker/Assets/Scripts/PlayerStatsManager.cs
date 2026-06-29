using System;
using UnityEngine;

public class PlayerStatsManager
{
    // Temel týklama gücü (Upgrade edilebilir)
    public double BaseClickPower { get; private set; }

    // Ýleride "2 kat hasar/altýn buff'ý" gelirse diye çarpan eklemek çok kolaylaþýr
    public double ClickMultiplier { get; private set; }

    // Mevcut toplam týklama gücünü hesaplayan property
    public double CurrentClickPower => BaseClickPower * ClickMultiplier;

    // Stat deðiþtiðinde UI veya efektlerin tetiklenmesi için event
    public event Action OnStatsChanged;

    public PlayerStatsManager(double defaultClickPower = 1)
    {
        BaseClickPower = 500_250;
        ClickMultiplier = 1.0;
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
}
