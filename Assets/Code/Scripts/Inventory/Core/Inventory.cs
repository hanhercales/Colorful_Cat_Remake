using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public InventoryType inventoryType = InventoryType.Main;
    
    [SerializeField] private int size = 20;
    [SerializeField] private List<Item> content = new List<Item>();
    
    private event System.Action onInventoryChanged;

    public enum InventoryType
    {
        Main,
        Equipment
    }
    
    public bool AddItem(Item item, int amount = 1)
    {
        if(item == null) return false;
        if(content.Count >= size) return false;

        foreach (Item i in content)
        {
            if(i.itemID == item.itemID && i.quantity < i.maxQuantity - 1)
            {
                i.AddQuantity(amount);
                onInventoryChanged?.Invoke();
                return true;
            }
        }
        
        content.Add(item);
        onInventoryChanged?.Invoke();
        return true;
    }

    public bool RemoveItem(Item item, int amount = 1)
    {
        if(item == null) return false;

        foreach (Item i in content)
        {
            if(i.itemID == item.itemID)
            {
                item = i;
                break;
            }
        }
        
        if(item != null)
        {
            item.RemoveQuantity(amount);
            if(item.quantity <= 0)
                content.Remove(item);
            onInventoryChanged?.Invoke();
            return true;
        }
        
        return false;
    }

    public bool EquipItem(Item item)
    {
        if(item == null || item.isEquipped) return false;
        
        item.Equip();
        
        onInventoryChanged?.Invoke();
        return true;
    }
    
    public bool UnequipItem(Item item)
    {
        if(item == null || !item.isEquipped) return false;
        
        item.Unequip();
        
        onInventoryChanged?.Invoke();
        return true;
    }

    public bool SwapItem(Item item1, Item item2)
    {
        (item1, item2) = (item2, item1);
        
        onInventoryChanged?.Invoke();
        return true;
    }

    public List<Item> GetContent()
    {
        return content;
    }
}
