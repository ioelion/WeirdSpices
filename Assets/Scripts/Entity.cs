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

        protected Animator an;

        public virtual void Start()
        {
            an = this.GetComponent<Animator>();
        }

        public virtual Weapon getWeapon(){
            return weapon;
        }

        public virtual void ReduceHealth(int pointsToReduce){
            hp -= pointsToReduce;
            an.SetTrigger("hit");
            if(hp <=0){
                Die();
            }
        }

        protected virtual void Die(){
            Destroy(this.gameObject);
        }
    }
}