using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public int trapDamage = 10;
    public bool isHealing;
    public int heal = 10;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (isHealing)
            {
                player.TakeHeal(heal);
                Destroy(gameObject);
            }
            else
            {
                player.TakeDamage(trapDamage);
            }
        }
    }
}
