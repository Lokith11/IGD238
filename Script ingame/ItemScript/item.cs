using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class item : MonoBehaviour
{
    private PlayerController player;

    public float throwForce = 10f;
    public int damageAmount = 10;

    public int maxDurability = 5;
    public int currrentDurability;

    private UIManager uIManager;
    private Rigidbody rb;

    private bool isThrow;

    public Material highlightMaterial;
    private Material originalMaterial;
    private Renderer itemRenderer;
    private float pickUpRange;


    void Start()
    {
        isThrow = false;
        currrentDurability = maxDurability;
        rb = GetComponent<Rigidbody>();
        uIManager = FindObjectOfType<UIManager>();

        itemRenderer = GetComponentInChildren<Renderer>();
        originalMaterial = itemRenderer.material;

        GameObject playerGameObject = GameObject.FindGameObjectWithTag("Player"); //หา player
        if (playerGameObject != null)
        {
            player = playerGameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                pickUpRange = player.pickRange;
                Debug.Log(pickUpRange);
            }
        }
       
    }

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit) && hit.collider.gameObject == gameObject)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position); // ค่าระยะจาก GameObject ที่ hit 
            if (distance <= pickUpRange && player.isHold == false)
            {
                itemRenderer.material = highlightMaterial;
            }
            else
            {
                itemRenderer.material = originalMaterial;
            }
        }
        else
        {
            itemRenderer.material = originalMaterial;
        }
    }
     


    public void Throw(Vector3 direction)
    {
        isThrow = true;
        rb.AddForce(direction * throwForce, ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && isThrow) // ตรวจสอบว่าชนกับศัตรู
        {
           itemEffect(collision.gameObject); 
        }
        isThrow = false;
    }

    public virtual void itemEffect(GameObject target)
    {
        Enemy enemy = target.GetComponent<Enemy>(); // เรียกใช้ฟังก์ชันรับดาเมจของศัตรู
        if (enemy != null) {
            enemy.TakeDamage(damageAmount); 
            currrentDurability--;
            uIManager.UpdateDurability(currrentDurability);
            if (currrentDurability <= 0)
            {
                currrentDurability = 0;
                Destroy(gameObject);
                uIManager.itemDurability.gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            uIManager.pressItemText.SetActive(true);
            if (gameObject == null)
            {
                uIManager.pressItemText.SetActive(false);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            uIManager.pressItemText.SetActive(false);
        }
    }
}
