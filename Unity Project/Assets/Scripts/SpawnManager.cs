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

[System.Serializable]
public struct EnemyWave
{
    public int wave; // Wave index
    public eEnemyWave waveType; // wave Type
    public int waveDuration; // one round duration
    public int EnemyNum; // number of enemies to be spawned
    public GameObject boss;
    public bool spawnBoss;
}


public class SpawnManager : MonoBehaviour
{
    public GameObject boss;
    public static SpawnManager Instance;

    
    [SerializeField]
    EnemySpawner spawner1, spawner2, spawner3;

    [SerializeField] private EnemyWave[] enemyWaves;
    int waveIndex = 0;

    public List<GameObject> spawnedEnemies;

    IEnumerator StartWave()
    {
        EnemyWave enemWave = enemyWaves[waveIndex];
        enemWave.spawnBoss = enemWave.boss != null;
        SpawnLane(enemWave);
        waveIndex++;
        yield return new WaitForSeconds(enemWave.waveDuration);

        if (waveIndex < enemyWaves.Length)
        {
            StartCoroutine(StartWave());
        } else {
            // if all wave ended
            // just continue;
        }
       
    }

    public void SpawnLane(EnemyWave wave)
    {
        switch(wave.waveType)
        {
            case eEnemyWave.Lane1:
                spawner1.SetSpawner(wave.EnemyNum, 1); //init spawner
                spawner1.SpawnEnemies(wave);
                break;
            case eEnemyWave.Lane2:
                spawner2.SetSpawner(wave.EnemyNum, 1);
                spawner2.SpawnEnemies(wave);
                break;
            case eEnemyWave.Lane3:
                spawner3.SetSpawner(wave.EnemyNum, 1);
                spawner3.SpawnEnemies(wave);
                break;
            case eEnemyWave.All:
                spawner1.SetSpawner(wave.EnemyNum, 1);
                spawner2.SetSpawner(wave.EnemyNum, 1);
                spawner3.SetSpawner(wave.EnemyNum, 1);
                spawner1.SpawnEnemies(wave);
                spawner2.SpawnEnemies(wave);
                spawner3.SpawnEnemies(wave);
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