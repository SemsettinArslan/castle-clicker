using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ClickableCastle : MonoBehaviour, IClickable
{
    public void OnClick()
    {
        // Gücü istatistiklerden alýyoruz
        double currentPower = GameController.Instance.Stats.CurrentClickPower;

        // Altýný cüzdana ekliyoruz
        GameController.Instance.Currency.AddGold(currentPower);
    }
}