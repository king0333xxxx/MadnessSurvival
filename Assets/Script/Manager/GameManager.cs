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
            case TurnState.StartRun:
                // Setup deck awal, reset hari
                ChangeState(TurnState.PlayerTurn);
                break;
            case TurnState.PlayerTurn:
                PlayerStats.Instance.currentEnergy = PlayerStats.Instance.maxEnergy;
                // Draw kartu ke tangan disini nanti
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
        Debug.Log("Malam tiba... Kegelapan menguras statusmu.");
        // Efek ruangan (Malam biasa)
        PlayerStats.Instance.ModifySupplies(-10);
        PlayerStats.Instance.ModifySanity(-5);

        // Cek jika ganti hari
        currentDay++;

        // Pindah ke giliran player lagi atau masuk ke stage upgrade jika sudah hari ke-5
        if (currentDay == 5)
        {
            ChangeState(TurnState.UpgradeStage);
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
            ChangeState(TurnState.EnvironmentTurn);
        }
    }
}