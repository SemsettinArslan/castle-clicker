using System;
using UnityEngine;

public class CurrencyManager
{
    public double Gold { get; private set; }
    public event Action<double> OnGoldChanged;

    public CurrencyManager(double startingGold = 0)
    {
        Gold = startingGold;
    }

    public void AddGold(double amount)
    {
        if (amount <= 0) return;
        Gold += amount;
        OnGoldChanged?.Invoke(Gold);
    }

    public bool TrySpendGold(double amount)
    {
        if (amount <= 0 || Gold < amount) return false;

        Gold -= amount;
        OnGoldChanged?.Invoke(Gold);
        return true;
    }
}
