using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeirdSpices{
    public abstract class Entity : MonoBehaviour
    {
        #region General Status
        [Header("General Status")]
        [SerializeField] private int healthPoints;
        [SerializeField] private int maxHealthPoints;
        [SerializeField] private float knockback;
        #endregion
        [SerializeField] protected Weapon weapon;
        [SerializeField] protected Rigidbody2D rb;
        [SerializeField] protected Animator an;


        public virtual void Start()
        {
        }

        public virtual void ReduceHP(int pointsToReduce){
            healthPoints -= pointsToReduce;
            an.SetTrigger("hit");
            if(healthPoints <=0){
                Die();
            }
        }

        public virtual void Heal(int pointsToHeal)
        {
            if (healthPoints < 10)
            {
                healthPoints += pointsToHeal;
            }
            
        }
        public virtual void AddHealthPoints(int pointsToAdd){
            if(healthPoints+pointsToAdd<= maxHealthPoints){
                healthPoints += pointsToAdd;
            }else{
                healthPoints = maxHealthPoints;
                Debug.Log("Ya tiene vida maxima (actual)");
            }

        }

        public void SetMaxHealthPoints(int maxHP){
            this.maxHealthPoints = maxHP;
        }

        public int GetMaxHealthPoints(int maxHP){
            return maxHealthPoints;
        }

        public bool IsOnMaxHealth(){
            return healthPoints >= maxHealthPoints;
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

        public void SetHP(int hp){
            this.healthPoints = hp;
        }

        public virtual void Knockback(Vector3 hitterPosition){

           transform.position = transform.position + (transform.position - hitterPosition).normalized *knockback;
           rb.velocity = Vector3.zero;
        }

    }
}