using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IventoryCanvas : MonoBehaviour
{
    
    [Header("Quick Slots")]
    public int quickSlotAmount = 3;
    public QuickSlot[] quickSlots;
    public Transform quickSlotPanel;
    public GameObject qucikSlotPrefab;
    
    [Header("Hand Slot")]
    public HandSlot handSlot;
    public GameObject handSlots;
    public Transform handSlotPanel;
    [Header("Inventory")]
    public ItemSO Empty_Item;
    public Transform slotPrefab;
    public Transform InventoryPanel;
    protected GridLayoutGroup gridLayoutGroup;
    [Space(5)]
    public int slotAmount = 16;
    public IventorySlot[] inventorySlot;
    [Header("Mini Canvas")]
    public RectTransform miniCanvas;
    public int carftInt;
    [SerializeField] protected IventorySlot rigthClickSlot;

    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            // หา Transform ที่เป็นจุดมือของผู้เล่น
            Transform handTransform = player.transform.Find("HandTransform");
            if (handTransform != null)
            {
                handSlot.handTransform = handTransform;
            }
            else
            {
                Debug.LogWarning("ใช้  HoldArea ผู้เล่นแทน");
                handSlot.handTransform = player.transform;
            }
        }
        gridLayoutGroup = InventoryPanel.GetComponent<GridLayoutGroup>();
        CreateInventorylot();
        CreateQuickSlots();
        CreateHandSlot();
        
        
    }
    public void Update()
    {
       
    }
    #region Slot Creation Methods
    
    public void CreateQuickSlots()
    {
        quickSlots = new QuickSlot[quickSlotAmount];
        for (int i = 0; i < quickSlotAmount; i++)
        {
            GameObject slot = Instantiate(qucikSlotPrefab, quickSlotPanel);
            QuickSlot quickSlot = slot.gameObject.GetComponent<QuickSlot>();
            
            quickSlots[i] = quickSlot;
            quickSlot.iventory = this;
            quickSlot.slotIndex = i;
            quickSlot.SetThisSlot(Empty_Item, 0);
        }
    }
    
    public void CreateHandSlot()
    {
        GameObject slot = Instantiate(handSlots, handSlotPanel);
        handSlot = slot.gameObject.GetComponent<HandSlot>();
        handSlot.iventory = this;
        handSlot.SetThisSlot(Empty_Item, 0);
    }
    #endregion
    #region Inventory Methods

    public void MakeThisToTopLayer(bool toTop)
    {
        transform.GetComponent<Canvas>().sortingOrder = toTop ? 1 : -1;
    }
    public virtual void AddItem(ItemSO item, int amount)
    {
       //ถ้าเป็นไอเทม ID003 ให้พยายามใส่ QuickSlot ก่อน
        if (item.ids == 3)
        {
            QuickSlot emptyQuickSlot = GetEmptyQuickSlot();
            if (emptyQuickSlot != null)
            {
                emptyQuickSlot.MergeThisSlot(item, amount);
                return;
            }
        }
        if (carftInt == 3)
        {

        }
        
        
        IventorySlot slot = IsEmptySlotLeft(item);
        if (slot == null)
        {
            DropItem(item, amount);
            return;
        }
        slot.MergeThisSlot(item, amount);
    }
    public QuickSlot GetEmptyQuickSlot()
    {
        foreach (QuickSlot slot in quickSlots)
        {
            if (slot.item == Empty_Item)
                return slot;
        }
        return null;
    }
    
    

    public void UseItem()//onclik ฟังชั้น
    {
        rigthClickSlot.UseItem();
        OnFinishMini();
    }
    public void DropItem()
    {
        Itemspawner.Instance.SpawnItem(rigthClickSlot.item, rigthClickSlot.stack);
        DestroyItem();
    }
    public void DropItem(ItemSO item, int amount)
    {
        Itemspawner.Instance.DropSpawnItem(item, amount);
    }
    public void DestroyItem()
    {
        rigthClickSlot.SetThisSlot(Empty_Item, 0);
        OnFinishMini();
    }

    public void RemoveItem(IventorySlot slot)
    {
        slot.SetThisSlot(Empty_Item, 0);
    }
    public void SortItem(bool Ascending = true)
    {

    }
    public virtual void CreateInventorylot()
    {
        inventorySlot = new IventorySlot[slotAmount];
        for (int i = 0; i < slotAmount; i++)
        {
            Transform slot = Instantiate(slotPrefab, InventoryPanel);// gเสก slot ออกมาใน panel
            IventorySlot inveSlot = slot.GetComponent<IventorySlot>();

            inventorySlot[i] = inveSlot;
            inveSlot.iventory = this;
            inveSlot.SetThisSlot(Empty_Item, 0);
        }
    }
    public IventorySlot  IsEmptySlotLeft(ItemSO itemChecker = null, IventorySlot itemslot = null)
    {
        if (inventorySlot == null) return null; // ป้องกันข้อผิดพลาด

        IventorySlot firstEmptySlot = null;
        foreach (IventorySlot slot in inventorySlot)
        {
            if (slot == itemslot)
                continue;
            if (slot.item == itemChecker && slot.stack < slot.item.maxStack)
            {
                return slot;
            }
            else if (slot.item == Empty_Item && firstEmptySlot == null)
            {
                firstEmptySlot = slot;
            }
        }
        return firstEmptySlot;
    }

    public void SetLayoutControlChiad(bool isControlled)
    {
        gridLayoutGroup.enabled = isControlled;
    }
    #endregion
    #region

    public void SetRightClickSlot(IventorySlot slot)
    {
        rigthClickSlot = slot;
    }
    public void OpenMini(Vector2 clickPosition)
    {
        miniCanvas.position = clickPosition;
        miniCanvas.gameObject.SetActive(true);
    }
    public void OnFinishMini()
    {
        rigthClickSlot = null;
        miniCanvas.gameObject.SetActive(false);
    }

    #endregion
}
