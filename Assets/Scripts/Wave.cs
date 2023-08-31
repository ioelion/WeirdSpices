using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace WeirdSpices
{
    public class Wave : MonoBehaviour
    {
        public List<Enemy> enemies;
        public List<Transform> spawnPoints;
        public int qtyToSpawn;
        public float timeBetweenEnemies;
    }
}
