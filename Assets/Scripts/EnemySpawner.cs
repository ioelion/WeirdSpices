using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

namespace WeirdSpices{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private List<GameObject> enemies;
        [SerializeField] private List<GameObject> waypoints;
        public List<GameObject> items;
        [SerializeField] private int maxEnemies;
        [SerializeField] private float timeToWaitToSpawn;
        private int currentEnemies = 0;
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

            if(currentEnemies < maxEnemies && Time.fixedTime - timeLastSpawn  > timeToWaitToSpawn){
                currentEnemies++;
                /*
                GameObject clone;
                Enemy enemy;
                clone = Instantiate(enemies[Random.Range(0, enemies.Count)]);
                if (items.Count > 0)
                {
                    enemy = clone.GetComponent<Enemy>();
                    int r = Random.Range(0, items.Count);
                    enemy._item = G;
                    enemy._itemTarget = items[r].transform;
                    enemy.waypoint = waypoints[Random.Range(0,waypoints.Count)].transform;
                }            */
                Instantiate(enemies[Random.Range(0, enemies.Count)]);
                timeLastSpawn = Time.fixedTime;
            }
        }

        public void EnemyDied(){
            currentEnemies--;
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
    }
}
