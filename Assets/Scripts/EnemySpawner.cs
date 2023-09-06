using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeirdSpices{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private List<Enemy> enemies;
        [SerializeField] private List<GameObject> waypoints;
        [SerializeField] private int maxEnemies;
        [SerializeField] private float timeToWaitToSpawn;
        [SerializeField] private List<Transform> spawnPositions;
        private List<Enemy> currentEnemies;
        private int qtyCurrentEnemies, qtySpawnPositions, qtyEnemiesTypes = 0;
        private float timeLastSpawn;
        public static EnemySpawner Instance { get; private set; }
        private bool spawning = true;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            } else {
                Debug.Log("Más de un EnemySpawner en escena.");
            }
            timeLastSpawn = Time.fixedTime;
        }

        void Start()
        {
            qtySpawnPositions = spawnPositions.Count;
            qtyEnemiesTypes = enemies.Count;
        }

        void Update()
        {

            if(spawning && qtyCurrentEnemies < maxEnemies && Time.fixedTime - timeLastSpawn  > timeToWaitToSpawn){
                qtyCurrentEnemies++;
                Enemy enemy = Instantiate(enemies[Random.Range(0,qtyEnemiesTypes)], spawnPositions[Random.Range(0, qtySpawnPositions)].position,Quaternion.identity);
                timeLastSpawn = Time.fixedTime;
            }
        }

        public void EnemyDied(Enemy enemy){
            qtyCurrentEnemies--;
            timeLastSpawn = Time.fixedTime;
            if(enemy.fromWave) WaveManager.Instance.EnemyFromWaveWasKilled();
        }



        public GameObject GetNextDropable()
        {
            if (GameManager.Instance.RandomParentlessActiveDropable() != null)
            {
                return GameManager.Instance.RandomParentlessActiveDropable().gameObject;
            }
            else { return null; }
        }

        public Transform GetWaypoint()
        {
            Transform waypoint = waypoints[Random.Range(0, waypoints.Count)].transform;
            return waypoint;
        }

        public void SpawnGrowingEnemy(string name, Vector2 position){
            foreach(Enemy enemy in enemies){
                Debug.Log("enemyObjectname: " +enemy.gameObject.name + " | name: " + name);
                if(enemy.gameObject.name == name){
                    Spawn(enemy,position).PlayGrowAnimation();
                }
            }

        }

        public Enemy Spawn(Enemy enemy, Vector2 position){
            Enemy enemySpawned = Instantiate(enemy.gameObject, position, Quaternion.identity).GetComponent<Enemy>();
            return enemySpawned;
        }

        public Enemy SpawnWaveEnemy(Enemy enemy, Vector2 position){
            Enemy enemySpawned = Spawn(enemy,position);
            enemySpawned.fromWave = true;
            return enemy;
        }

        public void isSpawning(bool spawning){
            this.spawning = spawning;
        }

    }
}
