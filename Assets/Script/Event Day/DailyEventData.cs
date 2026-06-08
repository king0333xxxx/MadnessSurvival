using UnityEngine;

[CreateAssetMenu(fileName = "New Daily Event", menuName = "Event System/Daily Event")]
public class DailyEventData : ScriptableObject
{
    public string eventName;
    [TextArea] public string eventDescription; // Teks yang muncul di panel summary

    [Header("Trigger Conditions")]
    public bool isDefaultNormalDay; // Centang HANYA untuk event Normal Day

    [Tooltip("Apakah event ini muncul tiap kelipatan hari tertentu?")]
    public bool triggerOnSpecificInterval;
    public int intervalDays = 3; // Contoh: FullMoon tiap 3 hari

    [Tooltip("Apakah event ini murni probabilitas/gacha?")]
    public bool triggerOnRandomChance;
    [Range(0f, 100f)] public float probability = 20f; // Contoh: 20% kemungkinan

    [Header("Event Effects (Drain)")]
    public int sanityDrain = 5;
    public int suppliesDrain = 10;
    public int healthDrain = 0;

    // Fungsi untuk mengecek apakah event ini terpilih untuk besok
    public bool CanTrigger(int nextDay)
    {
        if (isDefaultNormalDay) return false; // Normal day selalu jadi cadangan terakhir

        if (triggerOnSpecificInterval && nextDay % intervalDays == 0) return true;

        if (triggerOnRandomChance)
        {
            float randomValue = Random.Range(0f, 100f);
            if (randomValue <= probability) return true;
        }

        return false;
    }

    // Fungsi untuk menerapkan efek event ke player
    public void ApplyEventEffect()
    {
        if (sanityDrain != 0) PlayerStats.Instance.ModifySanity(-sanityDrain);
        if (suppliesDrain != 0) PlayerStats.Instance.ModifySupplies(-suppliesDrain);
        if (healthDrain != 0) PlayerStats.Instance.ModifyHealth(-healthDrain);

        Debug.Log($"Event Dieksekusi: {eventName}");
    }
}