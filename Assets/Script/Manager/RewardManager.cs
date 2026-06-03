using System.Collections.Generic;
using UnityEngine;

public class RewardManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject rewardPanel; // Panel yang menutupi layar saat Upgrade Stage
    public Transform rewardContainer; // Tempat kartu pilihan di-spawn (pakai Horizontal Layout)
    public GameObject rewardCardPrefab; // Prefab UI khusus untuk reward (bisa pakai CardPrefab yang sama)

    [Header("Reward Pool (Semua kemungkinan hadiah)")]
    public List<CardData> possibleRewards; // Isi dengan berbagai macam CardData dari Inspector

    private void Start()
    {
        rewardPanel.SetActive(false); // Sembunyikan panel di awal game
    }

    // Dipanggil oleh GameManager saat masuk TurnState.UpgradeStage
    public void ShowRewards()
    {
        rewardPanel.SetActive(true);
        ClearRewards();

        // Ambil 3 kartu acak dari pool
        List<CardData> rolledRewards = GetRandomRewards(3);

        foreach (CardData rewardCard in rolledRewards)
        {
            GameObject newRewardObj = Instantiate(rewardCardPrefab, rewardContainer);

            // Kita butuh script khusus untuk tombol reward, kita buat di Langkah 4
            UIRewardCard uiReward = newRewardObj.AddComponent<UIRewardCard>();
            uiReward.Initialize(rewardCard, this);
        }
    }

    private List<CardData> GetRandomRewards(int count)
    {
        List<CardData> results = new List<CardData>();
        for (int i = 0; i < count; i++)
        {
            int randomIndex = Random.Range(0, possibleRewards.Count);
            results.Add(possibleRewards[randomIndex]);
        }
        return results;
    }

    // Dipanggil saat player mengklik salah satu kartu hadiah
    public void OnRewardSelected(CardData selectedCard)
    {
        if (selectedCard.cardType == CardType.Upgrade)
        {
            // Jika ini kartu Upgrade pasif, langsung eksekusi efeknya (misal +1 Max Energy)
            // HANYA eksekusi, TIDAK ditambahkan ke dek
            selectedCard.PlayCard();
            Debug.Log("Player memilih Upgrade Stat!");
        }
        else
        {
            // Jika ini Action/Resource, tambahkan fisik kartunya ke dalam dek
            DeckManager.Instance.AddCardToDeck(selectedCard);
        }

        // Tutup panel reward
        rewardPanel.SetActive(false);
        ClearRewards();

        // Beritahu GameManager untuk lanjut ke hari berikutnya
        GameManager.Instance.ChangeState(TurnState.PlayerTurn);
    }

    private void ClearRewards()
    {
        foreach (Transform child in rewardContainer)
        {
            Destroy(child.gameObject);
        }
    }
}