using UnityEngine;
using UnityEngine.UI;

public class UIRewardCard : MonoBehaviour
{
    private CardData myData;
    private RewardManager manager;

    public void Initialize(CardData data, RewardManager managerRef)
    {
        myData = data;
        manager = managerRef;

        // Manfaatkan CardDisplay yang sudah ada di prefab untuk menampilkan visualnya
        CardDisplay display = GetComponent<CardDisplay>();
        if (display != null)
        {
            display.cardData = data;
            display.UpdateCardVisuals();
        }

        // Timpa fungsi tombolnya agar tidak menjalankan fungsi "Play" biasa,
        // melainkan mengirim data ke RewardManager
        Button btn = GetComponent<Button>();
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(SelectThisReward);
    }

    private void SelectThisReward()
    {
        manager.OnRewardSelected(myData);
    }
}