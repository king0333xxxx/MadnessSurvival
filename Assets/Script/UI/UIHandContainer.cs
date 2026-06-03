using System.Collections.Generic;
using UnityEngine;

public class UIHandContainer : MonoBehaviour
{
    [Header("References")]
    public GameObject cardPrefab;
    public Transform handContainer;

    // KITA PINDAHKAN SUBSCRIBE KE START
    private void Start()
    {
        // 1. Daftar ke event. Karena DeckManager pakai Awake, Instance-nya pasti sudah ada di sini.
        DeckManager.Instance.OnHandUpdated += RefreshHandVisuals;

        // 2. Langsung panggil render visual satu kali saat game dimulai.
        // Ini untuk menangani kasus jika GameManager sudah menarik kartu duluan sebelum skrip ini aktif.
        RefreshHandVisuals();
    }

    // KITA PINDAHKAN UNSUBSCRIBE KE ONDESTROY (Lebih aman untuk UI)
    private void OnDestroy()
    {
        if (DeckManager.Instance != null)
        {
            DeckManager.Instance.OnHandUpdated -= RefreshHandVisuals;
        }
    }

    private void RefreshHandVisuals()
    {
        ClearHand();

        List<CardData> currentHand = DeckManager.Instance.HandPile;

        foreach (CardData cardData in currentHand)
        {
            GameObject newCardObj = Instantiate(cardPrefab, handContainer);
            CardDisplay cardDisplay = newCardObj.GetComponent<CardDisplay>();

            if (cardDisplay != null)
            {
                cardDisplay.cardData = cardData;
                cardDisplay.UpdateCardVisuals();
            }
        }
    }

    private void ClearHand()
    {
        foreach (Transform child in handContainer)
        {
            Destroy(child.gameObject);
        }
    }
}