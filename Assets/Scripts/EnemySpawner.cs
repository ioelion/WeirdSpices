using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeirdSpices{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private List<GameObject> enemies;
        [SerializeField] private int maxEnemies;
        [SerializeField] private float timeToWaitToSpawn;
        private int currentEnemies = 0;
        private float timeLastSpawn;
        public static EnemySpawner Instance { get; private set; }    

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            } else {
                Debug.Log("Más de un EnemySpawner en escena.");
            }
        }

        void Update()
        {
            if(currentEnemies < maxEnemies && Time.fixedTime - timeLastSpawn  > timeToWaitToSpawn){
                currentEnemies++;
                Instantiate(enemies[Random.Range(0, enemies.Count)]);
                timeLastSpawn = Time.fixedTime;
            }
        }

        public void EnemyDied(){
            currentEnemies--;
            timeLastSpawn = Time.fixedTime;
        }

    }
}
