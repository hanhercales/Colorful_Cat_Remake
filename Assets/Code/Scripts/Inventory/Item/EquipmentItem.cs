using UnityEngine;

[CreateAssetMenu(fileName = "NewEquipmentItem", menuName = "Items/Equipment Item")]
public class EquipmentItem : Item
{
    public FormSO form;

    public bool EquipItem(GameObject target)
    {
        if(target == null || form == null) return false;

        if (target.TryGetComponent(out AbilityHandler abilityHandler))
        {
            if (abilityHandler.currentForm == form)
            {
                return false;
            }
            
            abilityHandler.EquipForm(form);
            
            return true;
        }
        
        return false;
    }

    public bool UnEquipItem(GameObject target)
    {
        if(target == null || form == null) return false;
        
        if (target.TryGetComponent(out AbilityHandler abilityHandler))
        {
            if (abilityHandler.currentForm == form)
            {
                abilityHandler.EquipForm(null);
                return true;
            }
        }
        
        return false;
    }
}
