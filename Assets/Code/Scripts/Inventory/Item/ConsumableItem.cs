using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewConsumableItem", menuName = "Items/Consumable Item")]
public class ConsumableItem : Item
{
    public List<Effect> effects;
    
    public bool ApplyEffect(GameObject target)
    {
        if (target == null || effects == null || effects.Count == 0) return false;

        bool used = false;
        
        foreach (Effect effect in effects)
        {
            if (effect.OnApply(target, target))
                used = true;
        }

        return used;
    }
}
