using UnityEngine;
using UnityEngine.Serialization;

public class Item : ScriptableObject
{
    public enum ItemType
    {
        Consumable,
        Equipment
    }
    
    public string itemID;
    public string itemName;
    public Sprite itemIcon;
    public ItemType itemType;
    public string itemDescription;
    public int maxQuantity;
    public int quantity;
    public bool isEquipped;
    public string targetInventoryName;

    public bool Equip()
    {
        if(isEquipped) return false;
        
        Inventory targetInventory = GameObject.Find(targetInventoryName).GetComponent<Inventory>();

        foreach (Item i in targetInventory.GetContent())
        {
            if(i.isEquipped && i.itemType == this.itemType)
                return false;
        }
        
        isEquipped = true;
        return true;
    }

    public bool Unequip()
    {
        if(!isEquipped) return false;
        
        isEquipped = false;
        return true;
    }
    
    public void AddQuantity(int amount)
    {
        quantity += amount;
    }

    public void RemoveQuantity(int amount)
    {
        quantity -= amount;
        if (quantity < 0)
        {
            quantity = 0;
        }
    }
}
