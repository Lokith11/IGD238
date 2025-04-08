using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject inventoryPanel;
    public GameObject craftingPanel;

    private void Update()
    {
        // if(Input.GetKeyDown(KeyCode.I))
        // {
        //     inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        // }
        if(Input.GetKeyDown(KeyCode.C))
        {
            craftingPanel.SetActive(!craftingPanel.activeSelf);
        }
    }
}
