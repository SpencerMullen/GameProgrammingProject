using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public bool spawnEnd;

    [System.Serializable]
    public struct SpawnPos
    {
        public float leftX;
        public float rightX;
    }
    
    public Transform targetPoint;

    public GameObject enemy;
    //public int[] enemyxpos = { -49, 10, 65 };
    public SpawnPos[] enemyXPos;
    public int enemyzpos = 170;



    [SerializeField]
    private int maxEnemyCount = 10; // how many enemies you are going to spawn
    int currentEnemyCount; // enemies count to keep track of

    [SerializeField]
    private float spawnIntervalTime; // time it takes to spawn each enemies

    //public Transform leftSpawnPos, rightSpawnPos; // enemy spawn position, spawns in between two game objects

    void Start()
    {
        currentEnemyCount = 0;
        
    }

    public void SetSpawner(int enemyCount, int intervalTime)
    {
        currentEnemyCount = 0;
        maxEnemyCount = enemyCount;
        spawnIntervalTime = intervalTime; 
    }
    public void SpawnEnemies(EnemyWave enemWave)
    {
        StartCoroutine(StartSpawn(enemWave));
    }

    IEnumerator StartSpawn(EnemyWave enemWave)
    {
        //Debug.Log(gameObject.name + ": " + currentEnemyCount + " / " + maxEnemyCount);
        //if (currentEnemyCount+1 == maxEnemyCount && enemWave.spawnBoss)
        //{
            
        //    yield break;
        //}
        int ind = Random.Range(0, enemyXPos.Length);

        var enem = Instantiate(enemy, 
            new Vector3(Random.Range(enemyXPos[ind].leftX, enemyXPos[ind].rightX), 
                                    1.59f, 
                                    enemyzpos), Quaternion.identity);


        SpawnManager.Instance.spawnedEnemies.Add(enem.gameObject);
        enem.GetComponent<EnemyMovement>().SetTarget(targetPoint);

        currentEnemyCount++;
        yield return new WaitForSeconds(spawnIntervalTime);
        //Debug.Log(gameObject.name + ": " +currentEnemyCount +" / "+ maxEnemyCount);
        if (currentEnemyCount + 1 < maxEnemyCount)
        {
            StartCoroutine(StartSpawn(enemWave));
        }
        else 
        {
            SpawnBoss(enemWave.boss);
            spawnEnd = true;
        }

    }

    public void SpawnBoss(GameObject boss) {
        if (boss == null)
            return;
        //GameObject.FindGameObjectWithTag("SpawnManager").GetComponent<SpawnManager>().setWave(1);
        Debug.Log("Spawned boss");
        int ind = Random.Range(0, enemyXPos.Length);
        var enem = Instantiate(boss,
            new Vector3(Random.Range(enemyXPos[ind].leftX, enemyXPos[ind].rightX),
                                    1.59f,
                                    enemyzpos), Quaternion.identity);
        SpawnManager.Instance.spawnedEnemies.Add(enem.gameObject);
        enem.GetComponent<EnemyMovement>().SetTarget(targetPoint);
        currentEnemyCount++;
    }
}