using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeirdSpices{
    public class PlayerWeapon : Weapon
    {
        private void OnTriggerEnter2D(Collider2D other) {
            if(other.gameObject.CompareTag("Enemy")){
                Enemy enemy = other.gameObject.GetComponent<Enemy>();
                enemy.ReduceHP(damage);
                enemy.Knockback(this.gameObject.gameObject.transform.position);
            }
        }
    }
}