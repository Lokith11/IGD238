using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ItemOBJ : MonoBehaviour
{
    public ItemSO item;
    public int amount = 1;
    public TextMeshProUGUI amountText;
    public int IDs;
    private Rigidbody rb; 
    

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.Log(" No have Rigi " + gameObject.name);
        }
    }
    public void SetAmount(int newAmount)
    {
        amount = newAmount;
        amountText.text = amount.ToString();
    }
    public void RandomAmount()
    {
        amount = Random.Range(1, item.maxStack + 1);
        amountText.text = amount.ToString();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().inventory.AddItem(item, amount);
            Destroy(gameObject);

        }
        if (other.CompareTag("ground"))
        {
            if (rb != null)
            {
                rb.isKinematic = true;
                rb.velocity = Vector3.zero; 
                rb.angularVelocity = Vector3.zero;
            }
        }
        // if (other.CompareTag("Enemy"))
        // {
        //     Destroy(gameObject);
        // }
        if (gameObject.CompareTag("Bullet") && other.CompareTag("ground"))//จริงๆ ต้องใส่เป็น Enemy 
        {
            Debug.Log("hit");
            Destroy(gameObject);
        }
        else
        {
            Debug.Log(" Notting Hit ");
        }
    }
    // private void OnCollisionEnter(Collision other) 
    // {
    //     if (gameObject.CompareTag("Bullet") && other.gameObject.CompareTag("ground"))
    //     {
    //         Debug.Log("hit");
    //         Destroy(gameObject);
    //     }
    //     else
    //     {
    //         Debug.Log(" Notting Hit ");
    //     }
    // }

    
}
