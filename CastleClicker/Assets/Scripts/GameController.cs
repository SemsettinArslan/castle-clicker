using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    public CurrencyManager Currency { get; private set; }
    public PlayerStatsManager Stats { get; private set; }

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

        ServiceLocator.Register<CurrencyManager>(Currency);
        ServiceLocator.Register<PlayerStatsManager>(Stats);
    }

    private void Update()
    {
        // Add passive gold continuously based on delta time
        if (Stats != null && Currency != null)
        {
            double passiveGold = Stats.CurrentPassiveGold * Time.deltaTime;
            if (passiveGold > 0)
            {
                Currency.AddGold(passiveGold);
            }
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            ServiceLocator.Unregister<CurrencyManager>();
            ServiceLocator.Unregister<PlayerStatsManager>();
        }
    }
}
