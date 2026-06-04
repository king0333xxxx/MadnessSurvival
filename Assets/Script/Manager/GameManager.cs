using System; // Tambahkan ini di paling atas untuk Action
using UnityEngine;

public enum TurnState { StartRun, PlayerTurn, ResolveEffects, EnvironmentTurn, UpgradeStage, GameOver }

public class GameManager : MonoBehaviour
{
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
                DeckManager.Instance.DrawCards(3);
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
        PlayerStats.Instance.ModifySupplies(-10);
        PlayerStats.Instance.ModifySanity(-5);

        // CEK MENANG: Trigger HANYA di akhir hari ke-10 dan jika BUKAN mode endless
        if (currentDay == 10 && !isEndlessMode)
        {
            ChangeState(TurnState.GameOver);
            UIGameOver.Instance.ShowGameOver(true);
            return;
        }

        // Jika tidak menang (atau sedang endless), lanjut hari berikutnya
        currentDay++;
        OnDayChanged?.Invoke(currentDay); // Beritahu UI bahwa hari ganti

        // Tetap berikan Upgrade Screen setiap kelipatan 5 hari (Hari 5, 10, 15, dst.)
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

    public void EndTurn()
    {
        if (currentState == TurnState.PlayerTurn)
        {
            DeckManager.Instance.DiscardHand();
            ChangeState(TurnState.EnvironmentTurn);
        }
    }

    // Fungsi baru ini akan dipanggil oleh UI Game Over saat tombol "Try Endless" diklik
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