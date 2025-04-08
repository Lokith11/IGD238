using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[CreateAssetMenu(fileName = "New Recipe" , menuName = "Crafting/Create New Recipe" , order = 0)]
public class CraftRecipe : ScriptableObject
{
    [Header("Recipe")]
    public ItemSO[] recipeItems = new ItemSO[9];
    [Header("Output Item")]
    public ItemSO outputItem;
    public int outputStack = 1;

    
}
