using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeirdSpices{
    public abstract class Entity : MonoBehaviour
    {
        [SerializeField]
        private Weapon weapon;

        [SerializeField]
        private int hp;

        public virtual Weapon getWeapon(){
            return weapon;
        }

        public virtual void ReduceHealth(int pointsToReduce){
            hp -= pointsToReduce;
            if(hp <=0){
                Destroy(this.gameObject);
            }
        }

        protected virtual void Die(){
            Destroy(this.gameObject);
        }
    }
}