using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using UnityEditorInternal.Profiling.Memory.Experimental;


public class IventorySlot : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler,IDropHandler,IPointerClickHandler
{
    
    [Header ("Crafting Deatail")]
    public GameObject[] reCipePrefubs;
    [Header("inventory Detail")]
    public IventoryCanvas iventory;
    [Header("Slot Detail")]
    public ItemSO item;
    public int stack;

    [Header("UI")]
    public Color emptyColor;
    public Color itemColor;
    public Image icons;
    public TextMeshProUGUI stackText;
    [Header("Drag and Drop")]
    public int siblingIndex;
    public int craftInts;
    public RectTransform draggable;
    protected Canvas canvas;
    protected CanvasGroup canvasGroup;

    void Start()
    {
        
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        siblingIndex = transform.GetSiblingIndex();
    }
     void Update()
    {
    
    }
    

    #region Drag and Drop Methods
    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;//บล็อคraycast
        transform.SetAsLastSibling();
        iventory.SetLayoutControlChiad(false);
        iventory.MakeThisToTopLayer(true);


    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        draggable.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1.0f;
        canvasGroup.blocksRaycasts = true;
        draggable.anchoredPosition = Vector2.zero;
        transform.SetSiblingIndex(siblingIndex);
        
        
    }
    
    
    public virtual void OnDrop(PointerEventData eventData)
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
            }
            

        }
        
        
        
    }
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            
            if (item == iventory.Empty_Item)
                return;
            iventory.OpenMini(eventData.position);
            iventory.SetRightClickSlot(this);//เปิดเมนู ออนตลิก
            
        }
        

    }
    
    public virtual void UseItem()
    {
        stack = Mathf.Clamp(stack - 1, 0, item.maxStack);
        if (stack > 0)
        
            checkShowText();
        
        else
        
            iventory.RemoveItem(this);
        
    }
    public void SwapSlot(IventorySlot newSlot)
    {
        ItemSO keepItem;
        int keepstack;
        keepItem = item;
        keepstack = stack;

        SetSwap(newSlot.item, newSlot.stack);

        newSlot.SetSwap(keepItem, keepstack);
    }
    public void SetSwap(ItemSO swapItem, int amount)
    {
        item = swapItem;
        stack = amount;
        icons.sprite = swapItem.icon;
        checkShowText();
    }
    public void MergeThisSlot(IventorySlot mergeSlot)
    {
        if (stack == item.maxStack || mergeSlot.stack == mergeSlot.item.maxStack)
        {
            SwapSlot(mergeSlot);
            return;
                
        }
        int ItemAmount = stack + mergeSlot.stack;

        int intInthisSlot = Mathf.Clamp(ItemAmount, 0, item.maxStack); //า itemAmout ว่าเกืน newItem มั้ย ท่าเกินตัดออก
        stack = intInthisSlot;

        checkShowText();
        int amountLeft = ItemAmount - intInthisSlot;//เช็คว่า ไอเทมเกินช่ิงมั้ย
        if (amountLeft > 0)//เกินเท่าไหร่
        {
            mergeSlot.SetThisSlot(mergeSlot.item, amountLeft);//ถ้าเหลือใส่ช่องเดิม
        }
        else
        {
            iventory.RemoveItem(mergeSlot);
        }
    }
    public void MergeThisSlot(ItemSO mergeItem, int mergeAmount)
    {
        item = mergeItem;
        icons.sprite = mergeItem.icon;
        int ItemAmount = stack + mergeAmount;

        int intInthisSlot = Mathf.Clamp(ItemAmount, 0, item.maxStack); //า itemAmout ว่าเกืน newItem มั้ย ท่าเกินตัดออก
        stack = intInthisSlot;

        checkShowText();
        int amountLeft = ItemAmount - intInthisSlot;//เช็คว่า ไอเทมเกินช่ิงมั้ย
        if (amountLeft > 0)//เกินเท่าไหร่
        {
            IventorySlot slot = iventory.IsEmptySlotLeft(mergeItem, this);
            if (slot == null)
            {
                iventory.DropItem(mergeItem, amountLeft);
                return;
            }
            else
            {
                slot.MergeThisSlot(mergeItem, amountLeft);//รีเคอซีพ
            }
        }
        else
        {
            
        }
    }




    #endregion
    public virtual void SetThisSlot(ItemSO newItem, int amount)
    {
        item = newItem;
        Debug.Log(icons.name);
        Debug.Log(newItem.icon);
        icons.sprite = newItem.icon;


        int ItemAmount = amount;//เก็บค่า amount ไว้กับ itemAmout

        int intInthisSlot = Mathf.Clamp(ItemAmount, 0, newItem.maxStack);// รับค่า itemAmout ว่าเกืน newItem มั้ย ท่าเกินตัดออก
        stack = intInthisSlot;

        checkShowText();
        int amountLeft = ItemAmount - intInthisSlot;
        if (amountLeft > 0)
        {
            IventorySlot slot = iventory.IsEmptySlotLeft(newItem, this);//check slot ว่าง
            if (slot == null)
            {
                return;
            }
            else
            {
                slot.SetThisSlot(newItem, amountLeft);
            }
        }
    }
    public void checkShowText()
    {
        UpdateColorSlot();
        stackText.text = stack.ToString();
        if (item.maxStack < 2)
        {
            stackText.gameObject.SetActive(false);
        }
        else
        {
            if (stack > 1)
                stackText.gameObject.SetActive(true);
            else
                stackText.gameObject.SetActive(false);
        }
    }
    public void UpdateColorSlot()
    {
        if (item == iventory.Empty_Item)
            icons.color = emptyColor;
        else
            icons.color = itemColor;
    }
    
    


}
