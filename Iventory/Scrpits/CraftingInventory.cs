using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingInventory : IventoryCanvas
{
    [Header("Crafting")]
    public IventoryCanvas mainInventory;
    public OutputSlot outputSlot;
    [Header("Recipe")]
    public CraftRecipe[] craftRecipes;
    void Start()
    {
        gridLayoutGroup = InventoryPanel.GetComponent<GridLayoutGroup>();
        CreateInventorylot();
    }

    public override void CreateInventorylot()
    {
        inventorySlot = new CraftingSlot[slotAmount];
        for(int i = 0; i < slotAmount; i++)
        {
            Transform slot = Instantiate(slotPrefab, InventoryPanel);
            CraftingSlot CaSlot = slot.GetComponent<CraftingSlot>();

            inventorySlot[i] = CaSlot;
            CaSlot.iventory = mainInventory; // inven เดิม = invenCraf
            CaSlot.craftInventory = this;
            CaSlot.ClearThisCraftSlot();
        }
    }
    public override void AddItem(ItemSO item, int amount)
    {

    }
    public void UseCraftMaterials()
    {
        foreach (CraftingSlot cSlot in inventorySlot)
        {
            if (cSlot.item != Empty_Item)
            {
                cSlot.UseItem();
            }
        }
        CheckAllCraftRecipe();
    }
    public bool CheckItemRecipe(CraftRecipe craftRecipe)
    {
        bool matchesTheRecipe = true;
        for(int i = 0; i < inventorySlot.Length; i++)
        {
            if(craftRecipe.recipeItems[i] == null && inventorySlot[i].item == Empty_Item)
                continue;
            else
            {
                if(inventorySlot[i].item != craftRecipe.recipeItems[i])
                {
                    matchesTheRecipe = false;
                    break;
                    
                }
            }
        }
        return matchesTheRecipe;
    }
    public void CheckAllCraftRecipe()
    {
        if(craftRecipes == null)
        {
            Debug.Log("No Recipe");
            return;
        }
        CraftRecipe currenRecipe = null;

        foreach (CraftRecipe craftRecipe in craftRecipes)
        {
            if(CheckItemRecipe(craftRecipe))
            {
                currenRecipe = craftRecipe;
                break;
            }
        }
        if(currenRecipe == null)
        {
            outputSlot.ClearThisCraftSlot();
        }
        else
        {
            outputSlot.SetThisSlot(currenRecipe.outputItem, currenRecipe.outputStack);
        }
    }
    public void ReturnAllToMainInventory()
    {
        foreach (CraftingSlot cSlot in inventorySlot)
        {
            if(cSlot.item != Empty_Item)
            {
                mainInventory.AddItem(cSlot.item, cSlot.stack);
                cSlot.ClearThisCraftSlot();
            }
        }
    }
    public void ReturnToMainInventory(ItemSO item, int amount)
    {
        mainInventory.AddItem(item, amount);
    }
    public  void OnDisable()
    {
        ReturnAllToMainInventory();
    }
        
    

    // Update is called once per frame

}
