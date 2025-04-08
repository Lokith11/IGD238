using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OutputSlot : CraftingSlot
{
    void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        siblingIndex = transform.GetSiblingIndex();
        checkShowText();
    } 
    public override void OnBeginDrag(PointerEventData eventData)
    {
        return;
    }

    public override void OnDrag(PointerEventData eventData)
    {
        return;
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        return;
    }
    
    
    public override void OnDrop(PointerEventData eventData)
    {
       return;
    }
    public override void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            
            if (item == iventory.Empty_Item)
                return;
            iventory.AddItem(item, stack);
            craftInventory.UseCraftMaterials();
            
        }
        

    }
    public override void SetThisSlot(ItemSO newItem, int amount)
    {
        item = newItem;
        Debug.Log(icons.name);
        Debug.Log(newItem.icon);
        icons.sprite = newItem.icon;


        int ItemAmount = amount;//เก็บค่า amount ไว้กับ itemAmout

        int intInthisSlot = Mathf.Clamp(ItemAmount, 0, newItem.maxStack);// รับค่า itemAmout ว่าเกืน newItem มั้ย ท่าเกินตัดออก
        stack = intInthisSlot;

        checkShowText();
    }
    
    

    
}
