using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeirdSpices{
    public abstract class Entity : MonoBehaviour
    {
        #region General Status
        [Header("General Status")]
        [SerializeField] private int hp;
        [SerializeField] private int maxHP;
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

        public virtual void ReduceHP(int pointsToReduce){
            hp -= pointsToReduce;
            an.SetTrigger("hit");
            if(hp <=0){
                Die();
            }
        }

        public virtual void AddHP(int pointsToAdd){
            if(hp+pointsToAdd<= maxHP){
                hp += pointsToAdd;
            }else{
                Debug.Log("Ya tiene vida maxima (actual)");
            }

        }

        public void SetMaxHP(int maxHP){
            this.maxHP = maxHP;
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

        public int GetHP(){
            return hp;
        }

        public void SetHP(int hp){
            this.hp = hp;
        }

        public virtual void Knockback(Vector3 hitterPosition){

           transform.position = transform.position + (transform.position - hitterPosition).normalized *knockback;
        }

    }
}