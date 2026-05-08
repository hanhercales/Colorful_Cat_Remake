using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryDisplay : MonoBehaviour
{
    public static InventoryDisplay Instance { get; private set; }
    
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Transform slotParent;
    
    [SerializeField] private InventorySlotUI[] equipSlotsUI = new InventorySlotUI[2];
    [SerializeField] private InventorySlotUI consumeSlotUI;
    
    private List<InventorySlotUI> contentSlots = new List<InventorySlotUI>();
    
    private InventorySlotUI selectedSlot = null;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        SetupUI();

        if (Inventory.Instance != null)
        {
            Inventory.Instance.OnInventoryChanged += UpdateInventoryUI;
            Inventory.Instance.OnEquipmentChanged += UpdateEquipmentUI;
        }

        UpdateInventoryUI();
        UpdateEquipmentUI();
    }

    private void OnDestroy()
    {
        if (Inventory.Instance != null)
        {
            Inventory.Instance.OnInventoryChanged -= UpdateInventoryUI;
            Inventory.Instance.OnEquipmentChanged -= UpdateEquipmentUI;
        }
    }

    private void OnEnable()
    {
        if (Inventory.Instance != null)
        {
            UpdateInventoryUI();
            UpdateEquipmentUI();
        }
        ClearSelection();
    }

    private void SetupUI()
    {
        foreach (Transform child in slotParent) Destroy(child.gameObject);
        contentSlots.Clear();

        int maxSlots = Inventory.Instance.maxMainSlot;
        for (int i = 0; i < maxSlots; i++)
        {
            GameObject slotGO = Instantiate(slotPrefab, slotParent);
            InventorySlotUI slotUI = slotGO.GetComponent<InventorySlotUI>();

            if (slotUI != null)
            {
                slotUI.SetupSlotIndex(i);
                contentSlots.Add(slotUI);
                
                Button slotButton = slotUI.GetComponent<Button>();
                if(slotButton == null) slotButton = slotGO.AddComponent<Button>();
                slotButton.onClick.AddListener(slotUI.OnSlotClicked);
            }
        }
    }

    public void UpdateInventoryUI()
    {
        var content = Inventory.Instance.content;
        for(int i =  0; i < contentSlots.Count; i++)
            contentSlots[i].UpdateSlot(content[i]);
    }

    public void UpdateEquipmentUI()
    {
        equipSlotsUI[0].UpdateSlot(Inventory.Instance.equipmentSlots[0]);
        equipSlotsUI[1].UpdateSlot(Inventory.Instance.equipmentSlots[1]);
        consumeSlotUI.UpdateSlot(Inventory.Instance.consumableSlot);

        bool isActivatedSlot = Inventory.Instance.activeSlotIndex == 0;
        equipSlotsUI[0].SetBorder(isActivatedSlot);
        equipSlotsUI[1].SetBorder(!isActivatedSlot);
    }

    public void HandleSlotClick(InventorySlotUI clickedSlot)
    {
        if (selectedSlot == null)
        {
            if (!clickedSlot.currentSlot.IsEmpty())
            {
                selectedSlot = clickedSlot;
                selectedSlot.SetBorder(true);
            }
        }
        else if (selectedSlot == clickedSlot)
            ClearSelection();
        else
        {
            Inventory.Instance.MoveItem(selectedSlot.currentSlot, clickedSlot.currentSlot);
            ClearSelection();
        }
    }

    private void ClearSelection()
    {
        if (selectedSlot != null)
        {
            selectedSlot.SetBorder(false);
            selectedSlot = null;
        }
    }
}
