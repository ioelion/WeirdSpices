using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace WeirdSpices
{
    public class WaveManager : MonoBehaviour
    {
        [SerializeField] private List<Wave> waves;
        [SerializeField] private List<float> wavesTriggerPercentages;
        private Dictionary<Wave,float> wavesDoneWithPercentages;
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
            wavesDoneWithPercentages = new Dictionary<Wave,float>();
            lastEnemySpawn = Time.fixedTime;
            nextWaveTriggerPercentage = wavesTriggerPercentages[0];
            GameManager.Instance.LoadFlags(wavesTriggerPercentages);
        }

        void FixedUpdate()
        {
            if(currentWave != null && currentEnemiesSpawned < currentWave.qtyToSpawn && Time.fixedTime - lastEnemySpawn > currentWave.timeBetweenEnemies){
                EnemySpawner.Instance.SpawnWaveEnemy(currentWave.enemies[Random.Range(0,currentWaveQtyEnemyTypes)], currentWave.spawnPoints[Random.Range(0, currentWaveQtySpawns)].position);
                currentEnemiesSpawned++;
                lastEnemySpawn = Time.fixedTime;
            }
        }
        public void StartFirstWaveInList(){
            currentWave = waves[0];
            currentWaveQtyEnemyTypes = currentWave.enemies.Count;
            currentWaveQtySpawns = currentWave.spawnPoints.Count;
            lastEnemySpawn = Time.fixedTime;
            Debug.Log("Wave has started");
        }

        public void EndWave(){
            wavesDoneWithPercentages.Add(currentWave,wavesTriggerPercentages[0]);
            waves.Remove(currentWave);
            wavesTriggerPercentages.Remove(wavesTriggerPercentages[0]);
            currentWave = null;
            currentEnemiesSpawned = 0;
            currentEnemiesKilled = 0;
            nextWaveTriggerPercentage = wavesTriggerPercentages[0];
            GameManager.Instance.WaveEnd();
            Debug.Log("Wave has ended");
        }

        public bool CheckForWaveTrigger(float percentage){
            if(currentWave == null && percentage >= nextWaveTriggerPercentage){
                StartFirstWaveInList();
                return true;
            }
            return false;
        }

        public void EnemyFromWaveWasKilled(){
            currentEnemiesKilled++;
            Debug.Log("Killed enemies from wave: " + currentEnemiesKilled + " .Remaining: " + (currentWave.qtyToSpawn - currentEnemiesKilled));
            if(currentEnemiesKilled == currentWave.qtyToSpawn){
                EndWave();                
            }
        }
    }
}
