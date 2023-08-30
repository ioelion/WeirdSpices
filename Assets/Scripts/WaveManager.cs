using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace WeirdSpices
{
    public class WaveManager : MonoBehaviour
    {
        [SerializeField] private List<Wave> waves;
        private List<Wave> done;
        private Wave currentWave = null;
        private float lastEnemySpawn;
        private float currentEnemiesSpawned = 0;
        private int currentWaveQtyEnemyTypes, currentWaveQtySpawns;

        void Start()
        {
            done = new List<Wave>();
            lastEnemySpawn = Time.fixedTime;
            StartFirstWaveInList();
        }

        void FixedUpdate()
        {
            if(currentWave != null && currentEnemiesSpawned < currentWave.qtyToSpawn && Time.fixedTime - lastEnemySpawn > currentWave.timeBetweenEnemies){
                EnemySpawner.Instance.Spawn(currentWave.enemies[Random.Range(0,currentWaveQtyEnemyTypes)], currentWave.spawnPoints[Random.Range(0, currentWaveQtySpawns)].position);
                currentEnemiesSpawned++;
                lastEnemySpawn = Time.fixedTime;
            }
        }
        public void StartFirstWaveInList(){
            currentWave = waves[0];
            currentWaveQtyEnemyTypes = currentWave.enemies.Count;
            currentWaveQtySpawns = currentWave.spawnPoints.Count;
            lastEnemySpawn = Time.fixedTime;
        }

        public void EndWave(){
            done.Add(currentWave);
            waves.Remove(currentWave);
            currentWave = null;
            currentEnemiesSpawned = 0;
        }

    }
}
