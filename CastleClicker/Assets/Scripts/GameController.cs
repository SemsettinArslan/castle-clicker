using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    public CurrencyManager Currency { get; private set; }
    public PlayerStatsManager Stats { get; private set; } // Yeni sistem eklendi

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitSystems();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitSystems()
    {
        Currency = new CurrencyManager(startingGold: 0);
        Stats = new PlayerStatsManager(defaultClickPower: 1); 
    }
}
