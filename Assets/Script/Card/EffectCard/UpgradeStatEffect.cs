using UnityEngine;

[CreateAssetMenu(fileName = "New Upgrade Effect", menuName = "Card System/Effects/Upgrade Status")]
public class UpgradeStatEffect : CardEffect
{
    [Header("Permanent Upgrades")]
    public int maxHealthIncrease;
    public int maxSanityIncrease;
    public int maxSuppliesIncrease;
    public int maxEnergyIncrease;
    public int cardsDrawnIncrease; // Berapa kartu tambahan yang ditarik per giliran

    public override void ApplyEffect()
    {
        if (maxHealthIncrease != 0) PlayerStats.Instance.ModifyMaxHealth(maxHealthIncrease);
        if (maxSanityIncrease != 0) PlayerStats.Instance.ModifyMaxSanity(maxSanityIncrease);
        if (maxSuppliesIncrease != 0) PlayerStats.Instance.ModifyMaxSupplies(maxSuppliesIncrease);
        if (maxEnergyIncrease != 0) PlayerStats.Instance.ModifyMaxEnergy(maxEnergyIncrease);
        if (cardsDrawnIncrease != 0) PlayerStats.Instance.ModifyCardsDrawnPerTurn(cardsDrawnIncrease);

        Debug.Log("Upgrade Permanen Diterapkan!");
    }
}