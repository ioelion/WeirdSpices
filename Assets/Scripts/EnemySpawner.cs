using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeirdSpices{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] List<GameObject> enemies;
        [SerializeField] int maxEnemies;
        [SerializeField] private float timeToWaitToSpawn;
        int currentEnemies;
        private float timeLastSpawn;

        void Start()
        {
            currentEnemies = 0;
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
