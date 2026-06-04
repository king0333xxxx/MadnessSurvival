using UnityEngine;
using TMPro;

public class UIDayDisplay : MonoBehaviour
{
    public TextMeshProUGUI dayText;

    private void Start()
    {
        // Subscribe ke event
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnDayChanged += UpdateDayText;

            // Panggil sekali untuk sinkronisasi awal
            UpdateDayText(GameManager.Instance.currentDay);
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe saat hancur
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnDayChanged -= UpdateDayText;
        }
    }

    private void UpdateDayText(int day)
    {
        // Tambahkan embel-embel Endless jika mode tersebut aktif
        if (GameManager.Instance.isEndlessMode)
        {
            dayText.text = $"DAY {day} (ENDLESS)";
            dayText.color = Color.red; // Ganti warna agar terasa lebih menegangkan
        }
        else
        {
            dayText.text = $"DAY {day}";
            dayText.color = Color.white;
        }
    }
}