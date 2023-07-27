using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeirdSpices{
    public class PlayerWeapon : Weapon
    {
        private void OnTriggerEnter2D(Collider2D other) {
            if(other.gameObject.CompareTag("Enemy")){
                Enemy player = other.gameObject.GetComponent<Enemy>();
                player.ReduceHealth(damage);
            }
        }
    }
}