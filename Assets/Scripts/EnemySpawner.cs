using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    //config
    [SerializeField] List<WaveConfig> waveConfigs;
    [SerializeField] List<GameObject> enemyPrefabs;
    [SerializeField] bool loopWaves = false;
    //state


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnAllWaves());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator SpawnAllWaves()
    {
        do
        {
            foreach(WaveConfig w in waveConfigs)
            {
                yield return StartEnemyWaveSpawns(w);
            }
        } while (loopWaves);
    }

    private IEnumerator StartEnemyWaveSpawns(WaveConfig config)
    {
        var waypoints = config.GetWaypoints();
        //var index = (int)UnityEngine.Random.Range(0, enemyPrefabs.Count - 1);
        //var prefab = enemyPrefabs[index];
        for(int i = 0; i < config.NumberOfEnemies; i++)
        {
            var enemy = Instantiate(config.EnemyPrefab, waypoints[0].position, Quaternion.identity);
            enemy.GetComponent<EnemyPathing>().SetConfig(config);
            yield return new WaitForSeconds(config.TimeBetweenSpawns);
        }
    }

}
