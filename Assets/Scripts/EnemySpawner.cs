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
        private List<Enemy> currentEnemies;
        private int qtyCurrentEnemies = 0;
        private float timeLastSpawn;
        public static EnemySpawner Instance { get; private set; }

        public 

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

        void Update()
        {

            if(qtyCurrentEnemies < maxEnemies && Time.fixedTime - timeLastSpawn  > timeToWaitToSpawn){
                qtyCurrentEnemies++;
                Enemy enemy = Instantiate(enemies[Random.Range(0, enemies.Count)]);
                timeLastSpawn = Time.fixedTime;
            }
        }

        public void EnemyDied(){
            qtyCurrentEnemies--;
            timeLastSpawn = Time.fixedTime;
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
    }
}
