using System.Collections.Generic;
using UnityEngine;

public class UIHandContainer : MonoBehaviour
{
    [Header("Card Layout References")]
    public Transform handContainer;

    [Header("Card Template Prefabs")]
    public GameObject actionCardPrefab;
    public GameObject resourceCardPrefab;
    public GameObject upgradeCardPrefab;
    public GameObject curseCardPrefab;

    private void Start()
    {
        DeckManager.Instance.OnHandUpdated += RefreshHandVisuals;
        RefreshHandVisuals();
    }

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
            // Tentukan prefab berdasarkan tipe kartu data saat ini
            GameObject targetPrefab = GetPrefabByType(cardData.cardType);

            if (targetPrefab == null)
            {
                Debug.LogError($"Prefab untuk tipe {cardData.cardType} belum dimasukkan ke Inspector!");
                continue;
            }

            GameObject newCardObj = Instantiate(targetPrefab, handContainer);
            CardDisplay cardDisplay = newCardObj.GetComponent<CardDisplay>();

            if (cardDisplay != null)
            {
                cardDisplay.cardData = cardData;
                cardDisplay.UpdateCardVisuals();
            }
        }
    }

    // Fungsi pembantu untuk memetakan enum tipe kartu ke Game Object Prefab
    private GameObject GetPrefabByType(CardType type)
    {
        switch (type)
        {
            case CardType.Action: return actionCardPrefab;
            case CardType.Resource: return resourceCardPrefab;
            case CardType.Upgrade: return upgradeCardPrefab;
            case CardType.Curse: return curseCardPrefab;
            default: return actionCardPrefab;
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