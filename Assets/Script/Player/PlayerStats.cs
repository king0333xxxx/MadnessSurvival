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
    }

    public void ResetStats()
    {
        currentHealth = maxHealth;
        currentSanity = maxSanity;
        currentSupplies = maxSupplies;
        currentEnergy = maxEnergy;
        OnStatsChanged?.Invoke();
    }

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

    private void HandleDefeat()
    {
        Debug.Log("Game Over: Player Mati!");
        // Triger event Game Over disini
    }
}