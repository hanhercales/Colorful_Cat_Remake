using UnityEngine;

public class InventoryTester : MonoBehaviour
{
    [Header("Kéo các file ScriptableObject dùng để test vào đây")]
    public EquipmentItem testFormRed;
    public EquipmentItem testFormBlue;
    public ConsumableItem testHealthPotion;

    void Update()
    {
        // --- 1. TEST NHẶT ĐỒ (Thêm vào túi chung) ---
        if (Input.GetKeyDown(KeyCode.Alpha1)) // Phím số 1
        {
            Inventory.Instance.AddItem(testFormRed, 1);
            Debug.Log("🛠 TEST: Đã nhặt Form Đỏ vào túi.");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) // Phím số 2
        {
            Inventory.Instance.AddItem(testFormBlue, 1);
            Debug.Log("🛠 TEST: Đã nhặt Form Xanh vào túi.");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) // Phím số 3
        {
            Inventory.Instance.AddItem(testHealthPotion, 50); 
            Debug.Log("🛠 TEST: Đã nhặt 50 Bình máu vào túi.");
        }

        // --- 2. TEST TRANG BỊ ĐỒ (Từ túi lên người) ---
        // Giả sử các món đồ bạn vừa nhặt sẽ nằm ở các ô đầu tiên: ô 0, 1, 2
        if (Input.GetKeyDown(KeyCode.Alpha4)) // Phím số 4
        {
            Inventory.Instance.EquipItem(0);
            Debug.Log("🛠 TEST: Ra lệnh trang bị đồ ở ô túi số 0.");
        }
        if (Input.GetKeyDown(KeyCode.Alpha5)) // Phím số 5
        {
            Inventory.Instance.EquipItem(1);
            Debug.Log("🛠 TEST: Ra lệnh trang bị đồ ở ô túi số 1.");
        }
        if (Input.GetKeyDown(KeyCode.Alpha6)) // Phím số 6
        {
            Inventory.Instance.EquipItem(2);
            Debug.Log("🛠 TEST: Ra lệnh trang bị đồ ở ô túi số 2.");
        }

        // --- 3. TEST GAMEPLAY CỐT LÕI (Swap Form & Quick Use) ---
        if (Input.GetKeyDown(KeyCode.Q)) // Phím Q
        {
            Debug.Log("🛠 TEST: Bấm nút Đổi Form (Swap)");
            Inventory.Instance.SwapActiveForm();
        }
        if (Input.GetKeyDown(KeyCode.E)) // Phím E
        {
            Debug.Log("🛠 TEST: Bấm nút Dùng Thuốc Nhanh");
            Inventory.Instance.UseConsumable();
        }

        // --- 4. IN RA TRẠNG THÁI HIỆN TẠI CỦA HỆ THỐNG ---
        if (Input.GetKeyDown(KeyCode.Space)) // Phím Space
        {
            PrintInventoryState();
        }
    }

    private void PrintInventoryState()
    {
        Debug.Log("<color=cyan>=== TRẠNG THÁI INVENTORY ===</color>");
        
        // In trạng thái Túi đồ
        string bagLog = "<b>[TÚI CHUNG]:</b>\n";
        bool isBagEmpty = true;
        for (int i = 0; i < Inventory.Instance.content.Count; i++)
        {
            var slot = Inventory.Instance.content[i];
            if (!slot.IsEmpty())
            {
                bagLog += $"  - Ô {i}: {slot.itemInSlot.name} (x{slot.itemQuantity})\n";
                isBagEmpty = false;
            }
        }
        if (isBagEmpty) bagLog += "  (Túi đang trống)\n";
        Debug.Log(bagLog);

        // In trạng thái Khu vực Trang bị
        var form0 = Inventory.Instance.equipmentSlots[0];
        var form1 = Inventory.Instance.equipmentSlots[1];
        int activeIndex = Inventory.Instance.activeSlotIndex;
        
        string equipLog = "<b>[KHU VỰC TRANG BỊ]:</b>\n";
        
        // Thêm dấu [*] để biểu thị Form đang được cầm trên tay
        string marker0 = (activeIndex == 0) ? "<color=green>[* ĐANG CẦM]</color>" : "";
        string marker1 = (activeIndex == 1) ? "<color=green>[* ĐANG CẦM]</color>" : "";

        equipLog += $"  - Form Slot 0 {marker0}: {(form0.IsEmpty() ? "Trống" : form0.itemInSlot.name)}\n";
        equipLog += $"  - Form Slot 1 {marker1}: {(form1.IsEmpty() ? "Trống" : form1.itemInSlot.name)}\n";
        
        var consumable = Inventory.Instance.consumableSlot;
        equipLog += $"  - Quick-Use Slot: {(consumable.IsEmpty() ? "Trống" : consumable.itemInSlot.name + " (x" + consumable.itemQuantity + ")")}\n";
        
        Debug.Log(equipLog);
        Debug.Log("<color=cyan>============================</color>");
    }
}