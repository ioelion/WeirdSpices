using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeirdSpices{
    public class Weapon : MonoBehaviour
    {
        [SerializeField]
        private int damage;
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        void OnCollisionEnter2D(Collision2D other)
        {
            if(other.gameObject.CompareTag("Enemy")){
                Enemy enemy = other.gameObject.GetComponent<Enemy>();
                enemy.ReduceHealth(damage);
            }else if(other.gameObject.CompareTag("Player")){
                Player player = other.gameObject.GetComponent<Player>();
                player.ReduceHealth(damage);
            }
        }
    }
}
