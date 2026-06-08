using System.Collections;
using UnityEngine;
using TMPro;

public class UIDayTransition : MonoBehaviour
{
    public static UIDayTransition Instance { get; private set; }

    [Header("UI References")]
    public GameObject transitionPanel;
    public TextMeshProUGUI narrativeText;

    private void Awake()
    {
        Instance = this;
        transitionPanel.SetActive(false);
    }

    // Fungsi yang akan dipanggil oleh GameManager
    public void ShowDaySummary(DailyEventData resolvedEvent, int nextDay)
    {
        StartCoroutine(TransitionRoutine(resolvedEvent, nextDay));
    }

    private IEnumerator TransitionRoutine(DailyEventData resolvedEvent, int nextDay)
    {
        // 1. Munculkan panel
        transitionPanel.SetActive(true);
        narrativeText.text = "Kamu mengakhiri hari ini...";

        // Tunggu 1.5 detik agar pemain membaca
        yield return new WaitForSeconds(1.5f);

        // --- PEMBUATAN TEKS EFEK (DYNAMIC STRING) ---
        // Kita gunakan <size=80%> agar teks efek sedikit lebih kecil dari teks narasi
        string effectSummary = "\n\n<size=80%>";

        // Cek satu per satu, hanya tampilkan jika nilainya tidak 0
        if (resolvedEvent.healthDrain != 0)
            effectSummary += $"<color=#FF3333>Health -{resolvedEvent.healthDrain}</color>\n";

        if (resolvedEvent.sanityDrain != 0)
            effectSummary += $"<color=#CC66FF>Sanity -{resolvedEvent.sanityDrain}</color>\n";

        if (resolvedEvent.suppliesDrain != 0)
            effectSummary += $"<color=#FFCC00>Supplies -{resolvedEvent.suppliesDrain}</color>\n";

        effectSummary += "</size>";
        // --------------------------------------------

        // 2. Tampilkan teks narasi digabung dengan teks efek
        narrativeText.text = $"Memasuki Hari ke-{nextDay}...\n\n{resolvedEvent.eventDescription}{effectSummary}";

        // Eksekusi efek status secara internal ke data player
        resolvedEvent.ApplyEventEffect();

        // Tunggu 3.5 detik agar pemain bisa membaca narasi dan melihat angka yang berkurang
        yield return new WaitForSeconds(3.5f);

        // 3. Tutup panel dan kembalikan kendali ke GameManager
        transitionPanel.SetActive(false);

        // Beritahu GameManager bahwa animasi selesai
        GameManager.Instance.CompleteEnvironmentTurn();
    }
}