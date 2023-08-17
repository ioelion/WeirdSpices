using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeirdSpices{
    public class EnemyWeapon : Weapon
    {
        private void OnTriggerEnter2D(Collider2D other) {
            if(other.gameObject.CompareTag("Player")){
                Player player = other.gameObject.GetComponent<Player>();
                player.ReduceHealth(damage);
                player.Knockback(this.gameObject.gameObject.transform.position);
            }
        }
    }
}