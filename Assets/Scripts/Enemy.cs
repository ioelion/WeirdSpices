using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeirdSpices{
    public class Enemy : Entity
    {
        [SerializeField] private float timeToWaitTillAttack = 0.5f;
        [SerializeField] private float moveSpeed;

        private Transform target;    
        private Rigidbody2D rb;
        private Vector2 moveDirection;
        private Vector2 _force;
        private SpriteRenderer sr;
        private float lastAttackTime = 0f;
        private EnemySpawner enemySpawner;

        override public void Start()
        {
            rb = this.GetComponent<Rigidbody2D>();
            sr = this.GetComponent<SpriteRenderer>();
            enemySpawner = FindObjectOfType<EnemySpawner>();
            base.Start();
            
        }

        void Awake() {
            target = GameObject.Find("Player").transform;
        }

        void Update() {
            if(target){
                Vector3 direction = (target.position - transform.position).normalized;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                rb.rotation = angle;
                moveDirection = direction;
            }    
        }

        void FixedUpdate()
        {
            if(target){
                _force = new Vector2(moveDirection.x, moveDirection.y) * moveSpeed;
                if (_force != Vector2.zero)
                {   
                    an.SetBool("walk", true);
                    rb.velocity = _force;
                    rb.SetRotation(0);
                    sr.flipX =Mathf.Sign(_force.x) > 0;
                }
                if(((target.position - transform.position).magnitude < 2f) && Time.fixedTime - lastAttackTime > timeToWaitTillAttack){
                    an.SetTrigger("attack");
                    base.getWeapon().gameObject.SetActive(true);
                    lastAttackTime = Time.fixedTime;
                    if(sr.flipX){
                        base.getWeapon().FlipPositionX();
                    }
                }
            }


        }

        override protected void Die(){
            enemySpawner.EnemyDied();
            base.Die();
        }
    }


}
