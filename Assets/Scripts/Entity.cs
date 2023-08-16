using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeirdSpices{
    public abstract class Entity : MonoBehaviour
    {
        [SerializeField] private Weapon weapon;

        [SerializeField] private int healthPoints;

        protected Animator an;

        public virtual void Start()
        {
            an = this.GetComponent<Animator>();
        }

        public virtual void ReduceHealth(int pointsToReduce){
            healthPoints -= pointsToReduce;
            an.SetTrigger("hit");
            if(healthPoints <=0){
                Die();
            }
        }

        protected virtual void Die(){
            Destroy(this.gameObject);
            if (this.CompareTag("Enemy"))
            {
                GameManager.Instance.CreateCoin(transform.position);
                GameManager.Instance.CreateCoin(transform.position); 
            }
        }

        public Weapon getWeapon(){
            return weapon;
        }

        public int GetHealthPoints(){
            return healthPoints;
        }

    }
}