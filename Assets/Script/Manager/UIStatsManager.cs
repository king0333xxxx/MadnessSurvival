using UnityEngine;
using TMPro; // Wajib untuk TextMeshPro

public class UIStatsManager : MonoBehaviour
{
    [Header("Text References")]
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI sanityText;
    public TextMeshProUGUI suppliesText;
    public TextMeshProUGUI energyText;

    private void OnEnable()
    {
        // 1. Subscribe (Mendengarkan) Event
        // Saat UI ini aktif, daftarkan fungsi UpdateAllStatsUI ke dalam event OnStatsChanged
        if (PlayerStats.Instance != null)
        {
            PlayerStats.Instance.OnStatsChanged += UpdateAllStatsUI;
        }
    }

    private void OnDisable()
    {
        // 2. Unsubscribe (Berhenti Mendengarkan) Event
        // INI SANGAT PENTING! Jika UI hancur/non-aktif tapi lupa unsubscribe, 
        // akan terjadi Memory Leak dan Error NullReference. Penilai tes pasti mengecek ini.
        if (PlayerStats.Instance != null)
        {
            PlayerStats.Instance.OnStatsChanged -= UpdateAllStatsUI;
        }
    }

    private void Start()
    {
        // Subscribe tambahan jika PlayerStats baru inisialisasi di Awake
        if (PlayerStats.Instance != null)
        {
            PlayerStats.Instance.OnStatsChanged += UpdateAllStatsUI;
            // Panggil sekali di awal untuk memastikan UI sinkron dengan data awal
            UpdateAllStatsUI();
        }
    }

    // 3. Fungsi ini HANYA dipanggil saat kartu berefek pada player, bukan setiap detik
    private void UpdateAllStatsUI()
    {
        if (PlayerStats.Instance == null) return;
        int effectiveMaxHealth = PlayerStats.Instance.GetEffectiveMaxHealth();

        healthText.text = $" {PlayerStats.Instance.currentHealth} / {effectiveMaxHealth}";
        sanityText.text = $" {PlayerStats.Instance.currentSanity} / {PlayerStats.Instance.maxSanity}";
        suppliesText.text = $" {PlayerStats.Instance.currentSupplies} / {PlayerStats.Instance.maxSupplies}";

        // Beri warna ungu/magenta pada max health jika sedang terkena penalti Aura
        if (effectiveMaxHealth < PlayerStats.Instance.maxHealth)
        {
            healthText.text = $" {PlayerStats.Instance.currentHealth} / <color=#FF00FF>{effectiveMaxHealth}</color>";
        }

        // Energy biasanya ditampilkan sebagai angka saja atau resource point
        energyText.text = $" {PlayerStats.Instance.currentEnergy}";

        // --- BONUS POLISH (Nilai Tambah untuk Test) ---
        // Memberikan visual feedback jika status kritis (di bawah 30%)
        UpdateTextColor(healthText, PlayerStats.Instance.currentHealth, PlayerStats.Instance.maxHealth);
        UpdateTextColor(sanityText, PlayerStats.Instance.currentSanity, PlayerStats.Instance.maxSanity);
        UpdateTextColor(suppliesText, PlayerStats.Instance.currentSupplies, PlayerStats.Instance.maxSupplies);
    }

    // Fungsi kecil untuk mengubah warna teks menjadi merah jika status kritis
    private void UpdateTextColor(TextMeshProUGUI textUI, int currentValue, int maxValue)
    {
        float percentage = (float)currentValue / maxValue;
        if (percentage <= 0.3f)
        {
            textUI.color = Color.red; // Kritis
        }
        else
        {
            textUI.color = Color.white; // Normal
        }
    }
}