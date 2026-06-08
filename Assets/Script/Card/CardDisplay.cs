using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardDisplay : MonoBehaviour
{
    [Header("Data Reference")]
    public CardData cardData; // Drag ScriptableObject kartu ke sini nanti di Inspector

    [Header("UI References")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI costText;
    // public Image cardArt; // Jika ada Sprite Model

    private void Start()
    {
        // Update tampilan saat kartu muncul di tangan
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

        // Visual Polish: Jadikan teks harga warna merah jika harganya sedang naik karena kutukan
        if (actualCost > cardData.baseEnergyCost)
        {
            costText.color = Color.red;
        }
        else
        {
            costText.color = Color.magenta;
        }

        Image bgImage = GetComponent<Image>();
        if (cardData.cardType == CardType.Curse) bgImage.color = Color.magenta;
        else if (cardData.cardType == CardType.Action) bgImage.color = Color.cyan;
    }

    public void OnClickPlayCard()
    {
        if (cardData == null) return;

        // Gunakan GetActualEnergyCost() di sini juga
        if (PlayerStats.Instance.currentEnergy >= cardData.GetActualEnergyCost())
        {
            cardData.PlayCard();
            DeckManager.Instance.PlayCard(cardData);
            DeckManager.Instance.TriggerHandUpdate();

            bool isPositive = (cardData.cardType != CardType.Curse);
            // Panggil UI Avatar Manager
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