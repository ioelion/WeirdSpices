using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeirdSpices{
    public class EnemyWeapon : Weapon
    {
        private void OnTriggerEnter2D(Collider2D other) {
            if(other.gameObject.CompareTag("PlayerHitbox")){
                Player player = other.gameObject.GetComponentInParent<Player>();
                player.ReduceHP(damage);
                player.Knockback(this.gameObject.gameObject.transform.position);
            }
        }
    }
}