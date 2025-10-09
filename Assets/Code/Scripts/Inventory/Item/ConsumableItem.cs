using UnityEngine;

[CreateAssetMenu(fileName = "NewConsumableItem", menuName = "Item/Consumable Item")]
public class ConsumableItem : Item
{
    public enum ItemEffect
    {
        Heal,
    }
    
    public new ItemType itemType => ItemType.Consumable;
    public ItemEffect itemEffect;
    public int effectValue;

    public bool UseItem(PlayerStats stats)
    {
        if (itemEffect == ItemEffect.Heal)
            stats.gameObject.AddComponent<InstantHeal>().SetEffectValue(effectValue);
        quantity--;
        return true;
    }
}
