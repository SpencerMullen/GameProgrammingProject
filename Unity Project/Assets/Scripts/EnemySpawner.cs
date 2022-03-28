using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemy;
    public int[] enemyxpos = { -49, 10, 65 };
    public int enemyzpos = 170;

    [SerializeField]
    private int maxEnemyCount = 10; // how many enemies you are going to spawn
    int currentEnemyCount; // enemies count to keep track of

    [SerializeField]
    private float spawnInterval; // time it takes to spawn each enemies

    //public Transform leftSpawnPos, rightSpawnPos; // enemy spawn position, spawns in between two game objects

    void Start()
    {
        currentEnemyCount = 0;
        SpawnEnemies();
    }

    void SpawnEnemies()
    {
        StartCoroutine(StartSpawn());
    }

    IEnumerator StartSpawn()
    {
        Debug.Log("Enemy spawned");
        var enem = Instantiate(enemy, 
            new Vector3(enemyxpos[Random.Range(0,3)], 
                                    1.59f, 
                                    enemyzpos), Quaternion.identity);

        currentEnemyCount++;
        yield return new WaitForSeconds(spawnInterval);

        if (currentEnemyCount < maxEnemyCount)
        {
            StartCoroutine(StartSpawn());
        }
    }

}
