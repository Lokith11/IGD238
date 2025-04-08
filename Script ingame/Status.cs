
using System.Collections;
using UnityEngine;

public class Status : MonoBehaviour
{
    [Header("Base Stat")]
    public int health = 100;
    //Speed
    public float speed = 5f;
    public float originalMoveSpeed;
    public bool isSlowed = false;

    public int damage = 10;
    public int defense = 10; // %

    void Start()
    {
        originalMoveSpeed = speed;
    }
    public virtual void TakeDamage(float damageAmount)
    {
        float damageReductionPercentage = defense / 100f;
        int damageNet = Mathf.RoundToInt(damageAmount * (1 - damageReductionPercentage)); 
        health -= damageNet;
        Debug.Log(gameObject.name + " Take Damage : " + damageNet + " Now have HP :  " + health);
        if (health <= 0)
        {
            Die();
        }
    }

    public virtual void Slow(float slowAmount, float duration)
    {
        if (!isSlowed)
        {
            isSlowed = true;
            speed *= (1 - slowAmount);
            StartCoroutine(ResetSpeed(duration));
        }
    }
    
    IEnumerator ResetSpeed(float duration) 
    {
        yield return new WaitForSeconds(duration);
        speed = originalMoveSpeed;
        isSlowed = false;
    }

    protected virtual void Die()
    {
        Debug.Log(gameObject.name + " died!");
        Destroy(gameObject);
    }
}
