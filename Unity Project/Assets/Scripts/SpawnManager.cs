using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eEnemyWave
{
    Lane1,
    Lane2, 
    Lane3, 
    All,
};


public class SpawnManager : MonoBehaviour
{
    public GameObject boss;
    public static SpawnManager Instance;

    [System.Serializable]
    public struct EnemyWave
    {
        public int wave; // Wave index
        public eEnemyWave waveType; // wave Type
        public int waveDuration; // one round duration
        public int EnemyNum; // number of enemies to be spawned
    }
    [SerializeField]
    EnemySpawner spawner1, spawner2, spawner3;

    [SerializeField] private EnemyWave[] enemyWaves;
    int waveIndex = 0;

    public List<GameObject> spawnedEnemies;

    IEnumerator StartWave()
    {
        EnemyWave enemWave = enemyWaves[waveIndex];
        SpawnLane(enemWave.waveType, enemWave.EnemyNum);
        yield return new WaitForSeconds(enemWave.waveDuration);
        waveIndex++;
        if (waveIndex < enemWave.wave)
        {
            StartCoroutine(StartWave());
        } else {
            // if wave ended
            // TODO if enemy all dead, print win or lose
            switch(enemWave.waveType)
            {
                case eEnemyWave.Lane1:
                    if(spawner1.spawnEnd) {
                        spawner1.SpawnBoss(boss, enemWave.EnemyNum);   
                    }
                    break;
                case eEnemyWave.Lane2:
                    if(spawner2.spawnEnd) {
                        spawner2.SpawnBoss(boss, enemWave.EnemyNum);
                    }
                    break;
                case eEnemyWave.Lane3:
                    if(spawner3.spawnEnd) {
                        spawner3.SpawnBoss(boss, enemWave.EnemyNum); 
                    }
                    break;
                case eEnemyWave.All:
                    break;
            }
        }
    }

    public void SpawnLane(eEnemyWave waveType, int enemyNum)
    {
        switch(waveType)
        {
            case eEnemyWave.Lane1:
                spawner1.SetSpawner(enemyNum, 3);
                spawner1.SpawnEnemies();
                break;
            case eEnemyWave.Lane2:
                spawner2.SetSpawner(enemyNum, 3);
                spawner2.SpawnEnemies();
                break;
            case eEnemyWave.Lane3:
                spawner3.SetSpawner(enemyNum, 3);
                spawner3.SpawnEnemies();
                break;
            case eEnemyWave.All:
                spawner1.SetSpawner(enemyNum, 3);
                spawner2.SetSpawner(enemyNum, 3);
                spawner3.SetSpawner(enemyNum, 3);
                spawner1.SpawnEnemies();
                spawner2.SpawnEnemies();
                spawner3.SpawnEnemies();
                break;
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = GetComponent<SpawnManager>();
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
        spawner2.SpawnBoss(boss, 25);

        StartCoroutine(StartWave());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int getWave() {
        return waveIndex + 1;
    }

    public void setWave(int index) {
        waveIndex = index;
    }
}