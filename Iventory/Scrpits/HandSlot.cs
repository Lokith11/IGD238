using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandSlot : IventorySlot
{
    public Transform handTransform; // จุดที่ไอเท็มจะปรากฏบนมือผู้เล่น
    public PlayerController playerController;
    private GameObject currentHeldItem;
    private Rigidbody heldItemRigidbody;
    private Collider heldItemCollider;
    public float throwForce = 30f;
    public float upwardForceFactor = 1f; 
    private string originalTag;

    void Start()
    {
        
    }
    private void Awake()
    {
        // ถ้าไม่ได้กำหนด handTransform ใน Inspector ให้ลองหา Player
        if (handTransform == null)
        {
            PlayerController player = FindObjectOfType<PlayerController>();
            if (player != null && player.holdArea != null)
            {
                handTransform = player.holdArea;
            }
            else
            {
                Debug.LogWarning("ไม่พบ Hold Area ใน PlayerController");
                handTransform = new GameObject("DefaultHandTransform").transform;
                handTransform.SetParent(Camera.main.transform); // หรือผู้เล่น
                handTransform.localPosition = new Vector3(0.5f, -0.5f, 1f);
            }
        }
    }

    public override void SetThisSlot(ItemSO newItem, int amount)
    {
        base.SetThisSlot(newItem, amount);
        
        if (currentHeldItem != null)
        {
            // คืนค่า Tag เดิมก่อนทำลาย
            if (!string.IsNullOrEmpty(originalTag))
            {
                currentHeldItem.tag = originalTag;
            }
            Destroy(currentHeldItem);
        }
        
        if (newItem != iventory.Empty_Item && newItem.gamePrefab != null)
        {
            currentHeldItem = Instantiate(newItem.gamePrefab, handTransform);
            currentHeldItem.transform.localPosition = Vector3.zero;
            currentHeldItem.transform.localRotation = Quaternion.identity;
            
            // เก็บค่า Tag เดิมและเปลี่ยนเป็น Bullet
            originalTag = currentHeldItem.tag;
            currentHeldItem.tag = "Bullet";
            
            DisablePhysics(currentHeldItem);
        }
    }

    public void UseItemInHand()
    {
        if (item != iventory.Empty_Item && item.gamePrefab != null)
        {
            // สร้างไอเท็มในโลกเกม (เช่นขว้างหรือวาง)
            GameObject spawnedItem = Instantiate(item.gamePrefab, handTransform.position, handTransform.rotation);
            
            // เปิดการทำงานของ Physics สำหรับไอเท็มที่ถูกใช้งาน
            EnablePhysics(spawnedItem);
            
            // ลดจำนวนไอเท็ม
            UseItem();
        }
    }

    private void DisablePhysics(GameObject itemObject)
    {
        // find Rigidbody and Collider
        heldItemRigidbody = itemObject.GetComponent<Rigidbody>();
        heldItemCollider = itemObject.GetComponent<Collider>();
        
        //close Physics
        if (heldItemRigidbody != null)
        {
            heldItemRigidbody.isKinematic = true;
            heldItemRigidbody.detectCollisions = false;
        }
        
        if (heldItemCollider != null)
        {
            heldItemCollider.enabled = false;
        }
    }

    private void EnablePhysics(GameObject itemObject)
    {
        originalTag = itemObject.tag;
        itemObject.tag = "Bullet";
        // find Rigidbody and Collider
        Rigidbody rb = itemObject.GetComponent<Rigidbody>();
        Collider col = itemObject.GetComponent<Collider>();
        
        // Open Physics 
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.detectCollisions = true;
             // ล็อกการเคลื่อนที่บนแกน Y
            rb.constraints = RigidbodyConstraints.FreezePositionY;

            // สามารถเพิ่มแรงขว้างได้ที่นี่ (ถ้าต้องการ)
            rb.AddForce(handTransform.forward * throwForce, ForceMode.Impulse);
        }
        
        if (col != null)
        {
            col.enabled = true;
        }
       
    }
    
}
