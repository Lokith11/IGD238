using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlot: IventorySlot
{
    public int slotIndex;
    private KeyCode[] quickSlotKeys = { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3 };

    void Update()
    {
        if (Input.GetKeyDown(quickSlotKeys[slotIndex]))
        {
            UseItemFromQuickSlot();
        }
    }

    public void UseItemFromQuickSlot()
    {
        if (item != iventory.Empty_Item)
        {
            // ใส่ไอเท็มไปที่ HandSlot
            iventory.handSlot.SetThisSlot(item, stack);
            
            // ลบไอเท็มออกจาก QuickSlot
            iventory.RemoveItem(this);
            
            // ลดจำนวนแทน
            // UseItem(); // ลดจำนวนไอเท็ม
            // if(stack <= 0) iventory.RemoveItem(this);
        }
    }

    
    public void SelectThisQuickSlot()
    {
        if (item != iventory.Empty_Item)
        {
            iventory.handSlot.SetThisSlot(item, stack);
        }
    }
    
}
