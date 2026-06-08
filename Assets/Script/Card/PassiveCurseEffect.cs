using UnityEngine;

public abstract class PassiveCurseEffect : ScriptableObject
{
    // Fungsi ini akan dipanggil otomatis oleh GameManager di akhir giliran
    public abstract void ApplyPassiveEffect();
}