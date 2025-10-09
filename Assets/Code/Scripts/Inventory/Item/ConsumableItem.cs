using UnityEngine;

[CreateAssetMenu(fileName = "NewConsumableItem", menuName = "Item/Consumable Item")]
public class ConsumableItem : Item
{
    public enum ItemEffect
    {
        Heal,
        //any type of effect can be added if you want
    }
    
    public new ItemType itemType => ItemType.Consumable;
    public ItemEffect itemEffect;
    public int effectValue;
    public int price;

    public bool UseItem(PlayerStats stats)
    {
        if (itemEffect == ItemEffect.Heal)
            stats.gameObject.AddComponent<InstantHeal>().SetEffectValue(effectValue);
        //add more logic here if you want
        quantity--;
        return true;
    }
}
