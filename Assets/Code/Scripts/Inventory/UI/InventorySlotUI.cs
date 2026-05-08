using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI quantityText;
    //[SerializeField] private GameObject quantityTextPanel;
    [SerializeField] private Image clickBorder;
    
    public InventorySlot currentSlot { get; private set; }
    public int slotIndex { get; private set; }
    
    public void SetupSlotIndex(int index)
    {
        slotIndex = index;
    }

    public void UpdateSlot(InventorySlot slot)
    {
        currentSlot = slot;

        if (slot != null && !currentSlot.IsEmpty())
        {
            icon.sprite = slot.itemInSlot.itemIcon;
            icon.enabled = true;
            
            if(slot.itemQuantity > 1)
            {
                quantityText.text = slot.itemQuantity.ToString();
                quantityText.enabled = true;
            }
            else
                quantityText.enabled = false;
        }
        else
        {
            icon.sprite = null;
            icon.enabled = false;
            quantityText.text = "";
            quantityText.enabled = false;
        }
    }

    public void SetBorder(bool isClicked)
    {
        if(clickBorder != null)
            clickBorder.enabled = isClicked;
    }

    public void OnSlotClicked()
    {
        if (InventoryDisplay.Instance != null)
            InventoryDisplay.Instance.HandleSlotClick(this);
    }
}
