using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace WeirdSpices
{
    public class WaveManager : MonoBehaviour
    {
        [SerializeField] private List<Wave> waves;
        [SerializeField] private List<float> wavesTriggerPercentages;
        private List<Wave> done;
        private Wave currentWave = null;
        private float lastEnemySpawn, nextWaveTriggerPercentage = 0.0f;
        private int currentWaveQtyEnemyTypes, currentWaveQtySpawns, currentEnemiesSpawned, currentEnemiesKilled = 0;
        public static WaveManager Instance { get; private set; }

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Debug.Log("Más de un WaveManager en escena.");
            }
        }

        void Start()
        {
            done = new List<Wave>();
            lastEnemySpawn = Time.fixedTime;
            nextWaveTriggerPercentage = wavesTriggerPercentages[0];
            GameManager.Instance.LoadFlags(wavesTriggerPercentages);
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
            nextWaveTriggerPercentage = wavesTriggerPercentages[0];
        }

        public void CheckForWaveTrigger(float percentage){
            if(currentWave == null && percentage >= nextWaveTriggerPercentage){
                StartFirstWaveInList();
            }
        }
    }
}
