using UnityEngine;

[CreateAssetMenu(fileName = "New Passive Curse", menuName = "Card System/Passive Effects/Drain Status")]
public class DrainStatusPassive : PassiveCurseEffect
{
    [Header("Current Status Drain per Turn")]
    public int healthDrain;
    public int sanityDrain;
    public int suppliesDrain;

    [Header("Max Status Drain per Turn (BRUTAL)")]
    public int maxHealthDrain;
    public int maxSanityDrain;

    public override void ApplyPassiveEffect()
    {
        // Kurangi status saat ini (Gunakan nilai minus karena fungsi aslinya menjumlahkan)
        if (healthDrain != 0) PlayerStats.Instance.ModifyHealth(-healthDrain);
        if (sanityDrain != 0) PlayerStats.Instance.ModifySanity(-sanityDrain);
        if (suppliesDrain != 0) PlayerStats.Instance.ModifySupplies(-suppliesDrain);

        // Kurangi batas maksimal status
        if (maxHealthDrain != 0) PlayerStats.Instance.ModifyMaxHealth(-maxHealthDrain);
        if (maxSanityDrain != 0) PlayerStats.Instance.ModifyMaxSanity(-maxSanityDrain);

        Debug.Log("Aaargh! Kutukan di tangan melukaimu!");
    }
}