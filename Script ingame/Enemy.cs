using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : Status
{
    public float attackRange = 1f;
    public float attackDelay = 1.5f; // ช่วงเวลาในการโจมตี
    public event Action<GameObject> OnDeath;


    public float currentHealth;
    public Slider enemiesHpBar;

    public GameObject floatingTextPerfab;


    private NavMeshAgent agent;
    private float distanceToPlayer;
    private bool canAttack = true;
    private GameObject target;
    private PlayerController player;


    void Start()
    {
        currentHealth = health;
        enemiesHpBar.gameObject.SetActive(false);
        //agent 
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = attackRange;
        //speed settings
        originalMoveSpeed = speed;
        agent.speed = speed;

        target = GameObject.FindGameObjectWithTag("Player");
        if (target != null)
        {
            player = target.GetComponent<PlayerController>(); // รับ component PlayerController
        }
    }

    void Update()
    {
       
        if (player != null) // ตรวจสอบว่า player มีอยู่หรือไม่
        {
            distanceToPlayer = Vector3.Distance(transform.position, target.transform.position); // ใช้ target.transform.position

            if (distanceToPlayer <= attackRange)
            {
                if (canAttack)
                {
                    StartCoroutine(AttackWithDelay());
                }
                agent.isStopped = true;
            }
            else
            {
                agent.isStopped = false;
                agent.SetDestination(target.transform.position); // ใช้ target.transform.position
            }
        }
        else
        {
            agent.isStopped = true; // หยุดการเคลื่อนที่หากไม่มี player
        }
    }
   

    IEnumerator AttackWithDelay()
    {
        canAttack = false;
        Attack();
        yield return new WaitForSeconds(attackDelay);
        canAttack = true;
    }
    public override void Slow(float slowAmount, float duration)
    {
        if (!isSlowed)
        {
            isSlowed = true;
            speed *= (1 - slowAmount);
            agent.speed = speed;
            StartCoroutine(ResetSpeed(duration));
        }
    }
    IEnumerator ResetSpeed(float duration)
    {
        yield return new WaitForSeconds(duration);
        speed = originalMoveSpeed;
        agent.speed = speed;
        isSlowed = false;
    }

    void Attack()
    {
        // ใส่โค้ดสำหรับการโจมตี เช่น ลดพลังชีวิตเป้าหมาย
        if (player != null)
        {
            player.TakeDamage(damage);
            //Debug.Log("Enemy attacked player for " + damage + " damage!");
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
    public override void TakeDamage(float damageAmount)
    {
        float damageReductionPercentage = defense / 100f;
        float damageNet = damageAmount * (1 - damageReductionPercentage);
        currentHealth -= damageNet;

        //FloatingText
        if (floatingTextPerfab && currentHealth >0)
        {
            ShowFloatingText(damageNet);
        }

        //HpBar Enemies
        if (enemiesHpBar != null)
        {
            enemiesHpBar.gameObject.SetActive(true);
        }
        UpdateHpBar();

        //Dead
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    void ShowFloatingText(float damageAmount)
    {
        var floatText = Instantiate(floatingTextPerfab, transform.position, Quaternion.identity);
        floatText.GetComponent<TMP_Text>().text = damageAmount.ToString();
    }
    public virtual void UpdateHpBar()
    {
        if (enemiesHpBar != null)
        {
            enemiesHpBar.value = (float)currentHealth / health;
        }
    }

    protected override void Die()
    {
        if (OnDeath != null)
        {
            OnDeath(gameObject);
        }
        base.Die();
    }
}
