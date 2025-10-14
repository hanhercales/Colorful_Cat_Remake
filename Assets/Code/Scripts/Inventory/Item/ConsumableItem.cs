using UnityEngine;

[CreateAssetMenu(fileName = "NewConsumableItem", menuName = "Item/Consumable Item")]
public class ConsumableItem : Item
{
    public Effect itemEffect;

    public bool UseItem(PlayerStats stats)
    {
        if(stats == null || quantity <= 0) return false;
        stats.gameObject.GetComponent<EffectManager>().ApplyEffect(itemEffect);
        quantity--;
        return true;
    }
}
