using UnityEngine;

[System.Serializable]
public class InventorySlot
{
    public Item itemInSlot;
    public int itemQuantity;
    
    public InventorySlot(Item newItem, int newQuantity)
    {
        itemInSlot = newItem;
        itemQuantity = newQuantity;
    }

    public bool IsEmpty()
    {
        return itemQuantity <= 0 ||  itemInSlot == null;
    }

    public void ClearSlot()
    {
        itemQuantity = 0;
        itemInSlot = null;
    }

    public void AddQuantity(int newQuantity)
    {
        if(itemInSlot != null)
            itemQuantity += newQuantity;
    }

    public void RemoveQuantity(int newQuantity)
    {
        if (itemInSlot != null)
            itemQuantity -= newQuantity;
        
        if (itemQuantity <= 0) ClearSlot();
    }
}
