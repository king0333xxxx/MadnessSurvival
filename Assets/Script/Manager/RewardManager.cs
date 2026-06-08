using System.Collections.Generic;
using UnityEngine;

public class RewardManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject rewardPanel;
    public Transform rewardContainer;

    [Header("Reward Template Prefabs")]
    public GameObject actionRewardPrefab;
    public GameObject resourceRewardPrefab;
    public GameObject upgradeRewardPrefab;
    public GameObject curseRewardPrefab;

    [Header("Reward Pool")]
    public List<CardData> possibleRewards;

    private void Start()
    {
        rewardPanel.SetActive(false);
    }

    public void ShowRewards()
    {
        rewardPanel.SetActive(true);
        ClearRewards();

        List<CardData> rolledRewards = GetRandomRewards(3);

        foreach (CardData rewardCard in rolledRewards)
        {
            // Ambil prefab yang sesuai untuk tampilan layar reward
            GameObject targetPrefab = GetPrefabByType(rewardCard.cardType);

            if (targetPrefab == null) continue;

            GameObject newRewardObj = Instantiate(targetPrefab, rewardContainer);

            UIRewardCard uiReward = newRewardObj.AddComponent<UIRewardCard>();
            uiReward.Initialize(rewardCard, this);
        }
    }

    private GameObject GetPrefabByType(CardType type)
    {
        switch (type)
        {
            case CardType.Action: return actionRewardPrefab;
            case CardType.Resource: return resourceRewardPrefab;
            case CardType.Upgrade: return upgradeRewardPrefab;
            case CardType.Curse: return curseRewardPrefab;
            default: return actionRewardPrefab;
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

    public void OnRewardSelected(CardData selectedCard)
    {
        if (selectedCard.cardType == CardType.Upgrade)
        {
            selectedCard.PlayCard();
            Debug.Log("Player memilih Upgrade Stat!");
        }
        else
        {
            DeckManager.Instance.AddCardToDeck(selectedCard);
        }

        rewardPanel.SetActive(false);
        ClearRewards();
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