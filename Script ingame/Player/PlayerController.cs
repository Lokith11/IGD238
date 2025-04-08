using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : Status
{
    [Header("Inventory")]
    public IventoryCanvas inventory;
    public PlayerController()
    {
        damage = 5;
        defense = 0;
    }

    [Header("Current Stat")]
    public int currentHealth;

    public float rotateSpeed = 100f;


    [Header("Pick Settings")]
    public Transform holdArea;
    public float pickRange = 10f;
    private GameObject grabbedObject;
    public bool isHold = false;


    private UIManager uIManager;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = health;
        uIManager = FindObjectOfType<UIManager>();
        inventory = FindObjectOfType<IventoryCanvas>();
        // กำหนด holdArea ให้กับ handSlot
        if (inventory != null && inventory.handSlot != null)
        {
            inventory.handSlot.handTransform = this.holdArea;
        }
        uIManager.LoadUIData();
        uIManager.UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && inventory.handSlot.item != inventory.Empty_Item)
        {
            inventory.handSlot.UseItemInHand();
        }
        Move();
        Rotate();
        Pick();
    }
    private void CheckQuickSlotInput()
    {
        for (int i = 0; i < inventory.quickSlots.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                UseQuickSlot(i);
            }
        }

    }
    private void UseQuickSlot(int index)
    {
        if (index < 0 || index >= inventory.quickSlots.Length) return;

        QuickSlot slot = inventory.quickSlots[index];
        if (slot.item != inventory.Empty_Item)
        {
            // ถ้ามีไอเทมใน HandSlot ให้สลับ
            if (inventory.handSlot.item != inventory.Empty_Item)
            {
                ItemSO tempItem = inventory.handSlot.item;
                int tempAmount = inventory.handSlot.stack;

                inventory.handSlot.SetThisSlot(slot.item, slot.stack);
                slot.SetThisSlot(tempItem, tempAmount);
            }
            else
            {
                // ใส่ไอเทมจาก QuickSlot ไปที่ HandSlot
                inventory.handSlot.SetThisSlot(slot.item, slot.stack);
                slot.SetThisSlot(inventory.Empty_Item, 0);
            }
        }
    }

    public override void TakeDamage(float damageAmount)
    {
        float damageReductionPercentage = defense / 100f;
        int damageNet = Mathf.RoundToInt(damageAmount * (1 - damageReductionPercentage));
        currentHealth -= damageNet;
        //Debug.Log(gameObject.name + " Take Damage : " + damageNet + " Now have HP :  " + currentHealth);
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
        uIManager.UpdateHealth(currentHealth);
        uIManager.UpdateHealthBar(currentHealth, health);
        if (currentHealth <= 0)
        {
            Die();
            uIManager.loseButton.SetActive(true);
        }
    }

    public void TakeHeal(int healAmount)
    {
        currentHealth += healAmount;
        if (currentHealth > health)
        {
            currentHealth = health;

        }
        uIManager.UpdateHealth(currentHealth);
        uIManager.UpdateHealthBar(currentHealth, health);
    }



    public void Move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 Move = new Vector3(h, 0, v);

        Move.Normalize();
        transform.Translate(Move * speed * Time.deltaTime, Space.World);
    }

    public void Rotate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit)) // ãªé Raycast á·¹ Plane.Raycast
        {
            Vector3 targetDirection = hit.point - transform.position;
            targetDirection.y = 0; // äÁèãËéµÑÇÅÐ¤ÃËÁØ¹¢Öé¹Å§
                                   // ÊÃéÒ§ Quaternion ÊÓËÃÑº¡ÒÃËÁØ¹ä»ÂÑ§·ÔÈ·Ò§à»éÒËÁÒÂ
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            // ËÁØ¹µÑÇÅÐ¤Ãä»ÂÑ§·ÔÈ·Ò§à»éÒËÁÒÂÍÂèÒ§ÃÒºÃ×è¹
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        }
    }
    public void Pick()
    {
        if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
        {
            if (grabbedObject == null && !isHold)
            {
                pickObject();

            }
            else if (Input.GetMouseButton(0) && grabbedObject != null)
            {
                ThrowObject();
            }
        }
    }


    void pickObject()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            float distance = Vector3.Distance(transform.position, hit.collider.transform.position);
            if (hit.collider.CompareTag("Pickable") && distance <= pickRange)
            {
                isHold = true;
                grabbedObject = hit.collider.gameObject;
                grabbedObject.transform.SetParent(holdArea);
                grabbedObject.transform.position = holdArea.position;
                grabbedObject.GetComponent<Rigidbody>().isKinematic = true;
                uIManager.UpdateDurability(grabbedObject.GetComponent<item>().currrentDurability);
                uIManager.itemDurability.gameObject.SetActive(true);
            }
        }
    }
    void DropObject() //ãªé Drop ¢Í§ã¹Á×Í 
    {
        grabbedObject.transform.SetParent(null);
        grabbedObject.GetComponent<Rigidbody>().isKinematic = false;
        grabbedObject = null;
    }

    void ThrowObject()
    {
        if (grabbedObject != null)
        {
            isHold = false;
            grabbedObject.GetComponent<Rigidbody>().isKinematic = false;
            grabbedObject.transform.SetParent(null);
            grabbedObject.GetComponent<item>().Throw(transform.forward);
            grabbedObject = null;
            uIManager.itemDurability.gameObject.SetActive(false);
        }
    }
}
