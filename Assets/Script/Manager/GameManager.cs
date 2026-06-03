using UnityEngine;

public enum TurnState { StartRun, PlayerTurn, ResolveEffects, EnvironmentTurn, UpgradeStage, GameOver }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public TurnState currentState;
    public int currentDay = 1;

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
            // Di dalam GameManager.cs fungsi ChangeState()
            case TurnState.StartRun:
                DeckManager.Instance.InitializeDeck(); // Inisialisasi deck di awal run
                ChangeState(TurnState.PlayerTurn);
                break;

            case TurnState.PlayerTurn:
                PlayerStats.Instance.currentEnergy = PlayerStats.Instance.maxEnergy;

                // Tarik 3 kartu di awal turn
                DeckManager.Instance.DrawCards(3);

                Debug.Log("Hari " + currentDay + ": Giliran Player.");
                break;
            case TurnState.EnvironmentTurn:
                HandleEnvironmentDrain();
                break;
            case TurnState.GameOver:
                Debug.Log("Game Over Screen");
                break;
        }
    }

    private void HandleEnvironmentDrain()
    {
        PlayerStats.Instance.ModifySupplies(-10);
        PlayerStats.Instance.ModifySanity(-5);

        currentDay++;

        // Jika hari ke-5, panggil RewardManager
        if (currentDay == 5)
        {
            ChangeState(TurnState.UpgradeStage);

            // PASTIKAN kamu mereferensikan RewardManager di GameManager
            // Misalnya dengan FindObjectOfType atau Singleton
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
            DeckManager.Instance.DiscardHand(); // Bersihkan tangan (Kecuali kartu Curse)
            ChangeState(TurnState.EnvironmentTurn);
        }
    }

    public void StartTurn() 
    { 


    }
}