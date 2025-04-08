using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefab; // Prefab ของศัตรู
    public Transform[] spawnPoints; // ตำแหน่ง Spawn ของศัตรู
    public int minWaves = 3; // จำนวน Wave ขั้นต่ำ
    public int maxWaves = 5; // จำนวน Wave สูงสุด
    public int minEnemiesPerWave = 5; // จำนวนศัตรูต่อ Wave ขั้นต่ำ
    public int maxEnemiesPerWave = 10; // จำนวนศัตรูต่อ Wave สูงสุด
    public float timeBetweenWaves = 5f; // เวลาหน่วงระหว่าง Wave

    public PlayerSaved playerData;
    public GameObject door;

    private int currentWave = 0;
    private int totalWaves;
    private List<GameObject> spawnedEnemies = new List<GameObject>();

    void Start()
    {
        totalWaves = Random.Range(minWaves, maxWaves + 1);
        StartCoroutine(SpawnWaves());
        door.SetActive(false);
    }

    IEnumerator SpawnWaves()
    {
        while (currentWave < totalWaves)
        {
            currentWave++;
            int enemiesInWave = Random.Range(minEnemiesPerWave, maxEnemiesPerWave + 1);

            for (int i = 0; i < enemiesInWave; i++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(1f); // หน่วงเวลาการ Spawn ศัตรูแต่ละตัว
            }

            yield return new WaitForSeconds(timeBetweenWaves); // หน่วงเวลาระหว่าง Wave
            yield return new WaitUntil(() => spawnedEnemies.Count == 0); // รอให้ศัตรูหมดก่อนเริ่ม Wave ต่อไป
        }
        playerData.roomClear++;
        ActivateDoor();
    }

    void SpawnEnemy()
    {
        int spawnPointIndex = Random.Range(0, spawnPoints.Length);
        int RandomEnemy = Random.Range(0, enemyPrefab.Length);
        GameObject newEnemy = Instantiate(enemyPrefab[RandomEnemy], spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
        spawnedEnemies.Add(newEnemy);
        // เพิ่ม event เมื่อศัตรูตาย
        Enemy enemyHealth = newEnemy.GetComponent<Enemy>();
        if (enemyHealth != null)
        {
            enemyHealth.OnDeath += EnemyDied;
        }
    }

    void EnemyDied(GameObject enemy)
    {
        spawnedEnemies.Remove(enemy);
    }

    void ActivateDoor()
    {
        door.SetActive(true); // เปิดใช้งานประตู
    }
}