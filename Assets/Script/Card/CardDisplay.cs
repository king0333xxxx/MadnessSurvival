using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardDisplay : MonoBehaviour
{
    [Header("Data Reference")]
    public CardData cardData;

    [Header("UI References")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI costText;
    public Image cardArt; 

    private void Start()
    {
        if (cardData != null)
        {
            UpdateCardVisuals();
        }
    }

    public void UpdateCardVisuals()
    {
        nameText.text = cardData.cardName;
        descriptionText.text = cardData.cardDescription;

        int actualCost = cardData.GetActualEnergyCost();
        costText.text = actualCost.ToString();

        // Visual Polish: Warna teks cost berubah berdasarkan penalti
        if (actualCost > cardData.baseEnergyCost)
        {
            costText.color = Color.red; // Harga naik karena kutukan
        }
        else
        {
            costText.color = Color.white; // Harga normal (Sesuaikan dengan warna desainmu)
        }

        // Sinkronisasi gambar art jika ada
        if (cardArt != null && cardData.cardArt != null)
        {
            cardArt.sprite = cardData.cardArt;
        }
    }

    public void OnClickPlayCard()
    {
        if (cardData == null) return;

        if (PlayerStats.Instance.currentEnergy >= cardData.GetActualEnergyCost())
        {
            cardData.PlayCard();
            DeckManager.Instance.PlayCard(cardData);
            DeckManager.Instance.TriggerHandUpdate();

            bool isPositive = (cardData.cardType != CardType.Curse);
            if (UIAvatarManager.Instance != null)
            {
                UIAvatarManager.Instance.ShowCardReaction(isPositive);
            }
        }
        else
        {
            Debug.LogWarning("Energy tidak cukup!");
        }
    }
}