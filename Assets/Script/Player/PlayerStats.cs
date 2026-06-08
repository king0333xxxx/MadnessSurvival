using System;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }

    [Header("Current Stats")]
    public int currentHealth;
    public int currentSanity;
    public int currentSupplies;
    public int currentEnergy;

    [Header("Max Stats")]
    public int maxHealth = 100;
    public int maxSanity = 100;
    public int maxSupplies = 100;
    public int maxEnergy = 3; // AP per turn
    public int cardsDrawnPerTurn = 3; // JUMLAH TARIKAN KARTU DEFAULT

    // Event untuk di-subscribe oleh UI nanti
    public event Action OnStatsChanged;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        ResetStats();

        // Daftarkan fungsi agar saat tangan berubah (kartu ditarik/dibuang), status ikut dicek ulang
        if (DeckManager.Instance != null)
        {
            DeckManager.Instance.OnHandUpdated += SyncAuraEffects;
        }
    }

    private void OnDestroy()
    {
        if (DeckManager.Instance != null)
        {
            DeckManager.Instance.OnHandUpdated -= SyncAuraEffects;
        }
    }

    public void ResetStats()
    {
        currentHealth = maxHealth;
        currentSanity = maxSanity;
        currentSupplies = maxSupplies;
        currentEnergy = maxEnergy;
        OnStatsChanged?.Invoke();
    }

    // FUNGSI : Dynamic Getter untuk Aura
    public int GetEffectiveMaxHealth()
    {
        int totalPenalty = 0;

        // Hitung total penalti dari semua kartu yang ada di tangan saat ini
        if (DeckManager.Instance != null && DeckManager.Instance.HandPile != null)
        {
            foreach (CardData card in DeckManager.Instance.HandPile)
            {
                totalPenalty += card.auraMaxHealthPenalty;
                // jika ada 2 kartu yang sama, otomatis nambah 2 kali (Stacking!)
            }
        }

        // Pastikan batas darah tidak tembus ke angka minus (minimal 1)
        return Mathf.Max(1, maxHealth - totalPenalty);
    }

    // FUNGSI : Dipanggil tiap kali kartu ditarik atau dibuang
    private void SyncAuraEffects()
    {
        int effectiveMax = GetEffectiveMaxHealth();

        // Jika darah saat ini lebih besar dari batas Aura yang baru, pangkas darahnya
        if (currentHealth > effectiveMax)
        {
            currentHealth = effectiveMax;
        }

        // Beritahu UI untuk update teksnya
        OnStatsChanged?.Invoke();
    }

    // --- FUNGSI CURRENT STATS --- //
    public void ModifyHealth(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        OnStatsChanged?.Invoke();
        if (currentHealth <= 0) HandleDefeat();
    }

    public void ModifySanity(int amount)
    {
        currentSanity = Mathf.Clamp(currentSanity + amount, 0, maxSanity);
        OnStatsChanged?.Invoke();
    }

    public void ModifySupplies(int amount)
    {
        currentSupplies = Mathf.Clamp(currentSupplies + amount, 0, maxSupplies);
        OnStatsChanged?.Invoke();
    }

    public void ModifyEnergy(int amount)
    {
        currentEnergy = Mathf.Clamp(currentEnergy + amount, 0, maxEnergy);
        OnStatsChanged?.Invoke();
    }

    // --- FUNGSI MAX STATS UPGRADE --- //
    public void ModifyMaxHealth(int amount)
    {
        maxHealth += amount;
        currentHealth += amount; // Pemain langsung mendapat darah saat upgrade max
        OnStatsChanged?.Invoke();
    }

    public void ModifyMaxSanity(int amount)
    {
        maxSanity += amount;
        currentSanity += amount;
        OnStatsChanged?.Invoke();
    }

    public void ModifyMaxSupplies(int amount)
    {
        maxSupplies += amount;
        currentSupplies += amount;
        OnStatsChanged?.Invoke();
    }

    public void ModifyMaxEnergy(int amount)
    {
        maxEnergy += amount;
        OnStatsChanged?.Invoke();
    }

    public void ModifyCardsDrawnPerTurn(int amount)
    {
        cardsDrawnPerTurn += amount;
    }

    private void HandleDefeat()
    {
        Debug.Log("Game Over: Player Mati!");
        GameManager.Instance.ChangeState(TurnState.GameOver);
        UIGameOver.Instance.ShowGameOver(false);
    }
}