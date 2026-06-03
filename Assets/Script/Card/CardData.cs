using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card System/Card Data")]
public class CardData : ScriptableObject
{
    public string cardName;
    [TextArea] public string cardDescription;
    public CardType cardType;
    public int energyCost;
    public Sprite cardArt;

    // List efek yang akan dieksekusi saat kartu dimainkan
    public List<CardEffect> effects;

    public void PlayCard()
    {
        if (PlayerStats.Instance.currentEnergy >= energyCost)
        {
            PlayerStats.Instance.ModifyEnergy(-energyCost);
            foreach (var effect in effects)
            {
                effect.ApplyEffect();
            }
        }
        else
        {
            Debug.Log("Energy tidak cukup!");
        }
    }
}