using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using UnityEditorInternal.Profiling.Memory.Experimental;

public class CraftingSlot : IventorySlot
{
    [Header("Craft Inventory")]
    public CraftingInventory craftInventory;

    private void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        siblingIndex = transform.GetSiblingIndex();
        checkShowText();
    }
    public void ClearThisCraftSlot()
    {
        SetThisSlot(craftInventory.Empty_Item,0);
    }
    public override void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;//บล็อคraycast
        transform.SetAsLastSibling();
        
        craftInventory.SetLayoutControlChiad(false);
        iventory.MakeThisToTopLayer(false);

    }
    public override void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            IventorySlot slot = eventData.pointerDrag.GetComponent<IventorySlot>();

            if (slot != null)
            {
               
                if (slot.item == item)
                {
                    MergeThisSlot(slot);
                    
                }
                
                
                else
                {
                    SwapSlot(slot);
                }
                craftInventory.CheckAllCraftRecipe();
            }
            

        }  
    }
    public override void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            
            if (item == iventory.Empty_Item)
                return;
            // inven open minicanvas
            craftInventory.ReturnToMainInventory(item, stack);
            ClearThisCraftSlot();
            craftInventory.CheckAllCraftRecipe();
            
        }
        
        

    }
    public override void UseItem()
    {
        stack = Mathf.Clamp(stack -1, 0, item.maxStack);
        if(stack == 0)
        {
            ClearThisCraftSlot();
        }
        else
        {
            checkShowText();
        }
    }
}

    // public int slotIndex; // ระบุว่าเป็นช่องไหนของ CraftingSlot




    // public override void SetThisSlot(ItemSO newItem, int amount)
    // {
    //     base.SetThisSlot(newItem, amount);

    // }

