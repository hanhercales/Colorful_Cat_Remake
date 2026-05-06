using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }
 
    public event Action OnInventoryChanged;
    public event Action OnEquipmentChanged;

    public int maxMainSlot = 20;
    
    public List<InventorySlot> content =  new List<InventorySlot>();
    
    public InventorySlot[] equipmentSlots = new InventorySlot[2];
    public int activeSlotIndex = 0;
    
    public InventorySlot consumableSlot;

    [SerializeField] private GameObject player;
    private AbilityHandler playerAbilityHandler;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        
        if(player != null)
            playerAbilityHandler = player.GetComponent<AbilityHandler>();

        InitializeInventory();
    }

    private void InitializeInventory()
    {
        for(int i = 0; i < maxMainSlot; i++)
            content.Add(new InventorySlot(null, 0));

        equipmentSlots[0] = new InventorySlot(null, 0);
        equipmentSlots[1] = new InventorySlot(null, 0);
        consumableSlot = new InventorySlot(null, 0);
    }

    public bool AddItem(Item itemToAdd, int amount = 1)
    {
        if(itemToAdd == null || amount <= 0) return false;

        int currentAmount = amount;
        
        if(!(itemToAdd is EquipmentItem))
        {
            amount = AddToStack(content, itemToAdd, amount);
        }

        if (amount > 0)
        {
            amount = AddNewItem(content, itemToAdd, amount);
        }
        
        if (amount < currentAmount)
        {
            OnInventoryChanged?.Invoke();
            return true;
        }
        
        return false;
    }

    private int AddToStack(List<InventorySlot> slots, Item itemToAdd, int amount = 1)
    {
        foreach (InventorySlot slot in slots)
        {
            if (!slot.IsEmpty() && slot.itemInSlot == itemToAdd)
            {
                int remainAmount = itemToAdd.maxStackSize - slot.itemQuantity;

                if (remainAmount > 0)
                {
                    if (amount <= remainAmount)
                    {
                        slot.AddQuantity(amount);
                        return 0;
                    }
                    else
                    {
                        slot.AddQuantity(remainAmount);
                        amount -=  remainAmount;
                    }
                }
            }
        }

        return amount;
    }

    private int AddNewItem(List<InventorySlot> slots, Item itemToAdd, int amount = 1)
    {
        foreach (InventorySlot slot in slots)
        {
            if (slot.IsEmpty())
            {
                slot.itemInSlot = itemToAdd;
                
                if(amount <= itemToAdd.maxStackSize)
                {
                    slot.AddQuantity(amount);
                    return 0;
                }
                else
                {
                    slot.itemQuantity = itemToAdd.maxStackSize;
                    amount -= itemToAdd.maxStackSize;
                }
            }
        }
        
        return amount;
    }

    public void EquipItem(int contentIndex)
    {
        InventorySlot slot = content[contentIndex];

        if (slot.IsEmpty()) return;

        if (slot.itemInSlot is EquipmentItem formItem)
        {
            SwapSlot(slot, equipmentSlots[activeSlotIndex]);
            
            if(playerAbilityHandler != null)
                playerAbilityHandler.EquipForm(formItem.form);
        }
        else if (slot.itemInSlot is ConsumableItem consumableItem)
        {
            if (!consumableSlot.IsEmpty() && consumableSlot.itemInSlot == consumableItem)
            {
                int remainAmount = consumableItem.maxStackSize - consumableSlot.itemQuantity;

                if (remainAmount > 0)
                {
                    if (slot.itemQuantity <= remainAmount)
                    {
                        consumableSlot.AddQuantity(slot.itemQuantity);
                        slot.ClearSlot();
                    }
                    else
                    {
                        consumableSlot.AddQuantity(remainAmount);
                        slot.RemoveQuantity(remainAmount);
                    }
                }
            }
            else
            {
                SwapSlot(slot, consumableSlot);
            }
        }
        
        OnInventoryChanged?.Invoke();
        OnEquipmentChanged?.Invoke();
    }

    private void SwapSlot(InventorySlot slot1, InventorySlot slot2)
    {
        Item tempItem = slot2.itemInSlot;
        int tempQuantity = slot2.itemQuantity;
        
        slot2.itemInSlot = slot1.itemInSlot;
        slot2.itemQuantity = slot1.itemQuantity;

        if (tempItem != null)
        {
            slot1.itemInSlot = tempItem;
            slot1.itemQuantity = tempQuantity;
        }
        else
        {
            slot1.ClearSlot();
        }
    }

    public void SwapActiveForm()
    {
        activeSlotIndex = activeSlotIndex == 0 ? 1 : 0;
        
        Item activeItem = equipmentSlots[activeSlotIndex].itemInSlot;

        if (activeItem != null && activeItem is EquipmentItem formItem)
        {
            if(playerAbilityHandler != null)
                playerAbilityHandler.EquipForm(formItem.form);
        }
        else
        {
            if (playerAbilityHandler != null) 
                playerAbilityHandler.EquipForm(null);
        }
        
        OnEquipmentChanged?.Invoke();
    }

    public void UseConsumable()
    {
        if (consumableSlot.IsEmpty() || player == null) return;

        if (consumableSlot.itemInSlot is ConsumableItem consumableItem)
        {
            bool used = consumableItem.ApplyEffect(player);

            if (used)
            {
                consumableSlot.RemoveQuantity(1);
                OnEquipmentChanged?.Invoke();
            }
        }
    }
}
