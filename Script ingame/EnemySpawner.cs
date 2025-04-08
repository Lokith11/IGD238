using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefab; // Prefab �ͧ�ѵ��
    public Transform[] spawnPoints; // ���˹� Spawn �ͧ�ѵ��
    public int minWaves = 3; // �ӹǹ Wave ��鹵��
    public int maxWaves = 5; // �ӹǹ Wave �٧�ش
    public int minEnemiesPerWave = 5; // �ӹǹ�ѵ�ٵ�� Wave ��鹵��
    public int maxEnemiesPerWave = 10; // �ӹǹ�ѵ�ٵ�� Wave �٧�ش
    public float timeBetweenWaves = 5f; // ����˹�ǧ�����ҧ Wave

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
                yield return new WaitForSeconds(1f); // ˹�ǧ���ҡ�� Spawn �ѵ�����е��
            }

            yield return new WaitForSeconds(timeBetweenWaves); // ˹�ǧ���������ҧ Wave
            yield return new WaitUntil(() => spawnedEnemies.Count == 0); // ������ѵ�������͹����� Wave ����
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
        // ���� event ������ѵ�ٵ��
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
        door.SetActive(true); // �Դ��ҹ��е�
    }
}