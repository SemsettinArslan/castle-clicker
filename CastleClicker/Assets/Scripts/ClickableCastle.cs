using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ClickableCastle : MonoBehaviour, IClickable
{
    public void OnClick()
    {
        // Gücü istatistiklerden alıyoruz
        double currentPower = ServiceLocator.Resolve<PlayerStatsManager>().CurrentClickPower;

        // Altını cüzdana ekliyoruz
        ServiceLocator.Resolve<CurrencyManager>().AddGold(currentPower);
    }
}
