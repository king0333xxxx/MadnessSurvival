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
    // public Image cardArt; // Buka komen ini jika kamu sudah punya desain gambar

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
        costText.text = cardData.energyCost.ToString();

        // Ganti warna background kartu berdasarkan tipe (opsional, untuk visual clarity)
        Image bgImage = GetComponent<Image>();
        if (cardData.cardType == CardType.Curse) bgImage.color = Color.magenta;
        else if (cardData.cardType == CardType.Action) bgImage.color = Color.cyan;
    }

    // Fungsi ini akan dipanggil saat kartu di-klik
    public void OnClickPlayCard()
    {
        if (cardData == null) return;

        // Cek apakah Energy cukup
        if (PlayerStats.Instance.currentEnergy >= cardData.energyCost)
        {
            cardData.PlayCard(); // Eksekusi efek status pada player

            DeckManager.Instance.PlayCard(cardData); // Kabari deck manager bahwa kartu ini sudah pindah ke discard
        }
        else
        {
            Debug.LogWarning("Energy tidak cukup untuk memainkan " + cardData.cardName);
            // Kamu bisa tambahkan animasi UI bergetar atau teks merah di sini nanti
        }
    }
}