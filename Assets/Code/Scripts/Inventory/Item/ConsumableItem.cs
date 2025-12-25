using UnityEngine;

[CreateAssetMenu(fileName = "NewConsumableItem", menuName = "Item/Consumable Item")]
public class ConsumableItem : Item
{
    public Effect effect;
    
    public bool UseItem(GameObject target)
    {
        if(target == null || quantity <= 0) return false;
        if(effect.OnApply(target, target))
        {
            quantity--;
            return true;
        }
        return false;
    }
}
