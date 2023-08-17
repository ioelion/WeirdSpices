using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeirdSpices{
    public abstract class Entity : MonoBehaviour
    {
        #region General Status
        [Header("General Status")]
        [SerializeField] private int healthPoints;
        [SerializeField] private float knockback;
        #endregion
        [SerializeField] protected Weapon weapon;
        [SerializeField] protected Rigidbody2D rb;
        protected Animator an;


        public virtual void Start()
        {
            an = this.GetComponent<Animator>();
            rb = this.GetComponent<Rigidbody2D>();
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

        public virtual void Knockback(Vector3 hitterPosition){

           transform.position = transform.position + (transform.position - hitterPosition).normalized *knockback;
        }

    }
}