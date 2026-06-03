using UnityEngine;

[CreateAssetMenu(fileName = "New Status Effect", menuName = "Card System/Effects/Modify Status")]
public class ModifyStatusEffect : CardEffect
{
    public int healthChange;
    public int sanityChange;
    public int suppliesChange;

    public override void ApplyEffect()
    {
        if (healthChange != 0) PlayerStats.Instance.ModifyHealth(healthChange);
        if (sanityChange != 0) PlayerStats.Instance.ModifySanity(sanityChange);
        if (suppliesChange != 0) PlayerStats.Instance.ModifySupplies(suppliesChange);
    }
}