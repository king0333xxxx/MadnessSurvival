using System; // Tambahkan ini di paling atas untuk Action
using System.Collections.Generic;
using UnityEngine;

public enum TurnState { StartRun, PlayerTurn, ResolveEffects, EnvironmentTurn, UpgradeStage, GameOver }

public class GameManager : MonoBehaviour
{
    [Header("Event System")]
    public DailyEventData normalDayEvent; 
    public List<DailyEventData> possibleSpecialEvents;

    public static GameManager Instance { get; private set; }
    public TurnState currentState;

    public int currentDay = 1;
    public bool isEndlessMode = false; // Penanda apakah mode endless aktif

    // Event untuk UI Day Text
    public event Action<int> OnDayChanged;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ChangeState(TurnState.StartRun);
    }

    public void ChangeState(TurnState newState)
    {
        currentState = newState;
        switch (currentState)
        {
            case TurnState.StartRun:
                DeckManager.Instance.InitializeDeck();
                currentDay = 1;
                isEndlessMode = false;
                OnDayChanged?.Invoke(currentDay); // Panggil event update UI pertama kali
                ChangeState(TurnState.PlayerTurn);
                break;

            case TurnState.PlayerTurn:
                PlayerStats.Instance.currentEnergy = PlayerStats.Instance.maxEnergy;
                DeckManager.Instance.DrawCards(PlayerStats.Instance.cardsDrawnPerTurn);
                break;

            case TurnState.EnvironmentTurn:
                HandleEnvironmentDrain();
                break;

            case TurnState.GameOver:
                Debug.Log("Game Over / Win Screen");
                break;
        }
    }

    private void HandleEnvironmentDrain()
    {
        int nextDay = currentDay + 1;
        DailyEventData eventForTomorrow = normalDayEvent; // Set default ke hari biasa

        // Kocok dan cari apakah ada event spesial yang terpicu untuk besok
        foreach (DailyEventData specialEvent in possibleSpecialEvents)
        {
            if (specialEvent.CanTrigger(nextDay))
            {
                eventForTomorrow = specialEvent;
                break; // Jika ada 1 event yang tembus syarat, ambil itu dan stop pengecekan
            }
        }

        // Serahkan ke UI untuk dianimasikan. 
        
        UIDayTransition.Instance.ShowDaySummary(eventForTomorrow, nextDay);
    }

    public void EndTurn()
    {
        if (currentState != TurnState.PlayerTurn) return;

        // 1. EKSEKUSI EFEK KUTUKAN DI TANGAN TERLEBIH DAHULU
        foreach (CardData card in DeckManager.Instance.HandPile)
        {
            if (card.cardType == CardType.Curse && card.passiveEffects != null)
            {
                foreach (PassiveCurseEffect passive in card.passiveEffects)
                {
                    passive.ApplyPassiveEffect();
                }
            }
        }

        // 2. Bersihkan tangan (Kartu non-curse masuk ke discard pile)
        DeckManager.Instance.DiscardHand();

        // 3. Pindah ke giliran musuh / lingkungan
        ChangeState(TurnState.EnvironmentTurn);
    }

    // FUNGSI Dipanggil oleh UIDayTransition setelah panel layar hitam ditutup
    public void CompleteEnvironmentTurn()
    {
        // CEK MENANG (Sama seperti logika sebelumnya)
        if (currentDay == 10 && !isEndlessMode)
        {
            ChangeState(TurnState.GameOver);
            UIGameOver.Instance.ShowGameOver(true);
            return;
        }

        // Jika tidak menang, update angka hari
        currentDay++;
        OnDayChanged?.Invoke(currentDay);

        // Pengecekan Upgrade Stage (Reward)
        if (currentDay % 5 == 0)
        {
            ChangeState(TurnState.UpgradeStage);
            FindObjectOfType<RewardManager>().ShowRewards();
        }
        else
        {
            ChangeState(TurnState.PlayerTurn);
        }
    }

    // Fungsi dipanggil oleh UI Game Over saat tombol "Try Endless" diklik
    public void StartEndlessMode()
    {
        isEndlessMode = true;

        // Pindah hari ke-11
        currentDay++;
        OnDayChanged?.Invoke(currentDay);

        // Lanjut permainan
        ChangeState(TurnState.PlayerTurn);
    }
}