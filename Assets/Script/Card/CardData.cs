using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card System/Card Data")]
public class CardData : ScriptableObject
{
    public string cardName;
    [TextArea] public string cardDescription;
    public CardType cardType;

    public int baseEnergyCost; // Ganti nama dari energyCost jadi baseEnergyCost
    public Sprite cardArt;

    [Header("Play Effects (Aktif jika di-klik/Purge)")]
    public List<CardEffect> effects;

    [Header("Curse Effects")]
    public List<PassiveCurseEffect> passiveEffects; 
    public bool causesEnergyPenalty;

   
    [Tooltip("Mengurangi Max Health HANYA selama kartu ini ada di tangan")]
    public int auraMaxHealthPenalty;

    // Fungsi pintar untuk menghitung harga asli + penalti kutukan
    public int GetActualEnergyCost()
    {
        int finalCost = baseEnergyCost;

        // Jika ini adalah kartu kutukan, harganya tidak ikut naik (biar pemain tetap bisa buang)
        if (cardType == CardType.Curse) return finalCost;

        // Cek apakah di tangan pemain ada kartu Curse yang menyebabkan penalti
        if (DeckManager.Instance != null && DeckManager.Instance.HandPile != null)
        {
            foreach (CardData card in DeckManager.Instance.HandPile)
            {
                if (card.cardType == CardType.Curse && card.causesEnergyPenalty)
                {
                    finalCost += 1; // Harga nambah +1 untuk setiap kutukan jenis ini di tangan
                }
            }
        }
        return finalCost;
    }

    public void PlayCard()
    {
        int actualCost = GetActualEnergyCost();

        if (PlayerStats.Instance.currentEnergy >= actualCost)
        {
            PlayerStats.Instance.ModifyEnergy(-actualCost);
            foreach (var effect in effects)
            {
                effect.ApplyEffect();
            }
        }
    }
}