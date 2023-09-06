using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeirdSpices{
    public class Enemy : Entity
    {
        [SerializeField] private float timeToWaitTillAttack = 0.5f;
        [SerializeField] private float moveSpeed;
        [SerializeField] private float distanceToAttack = 2f;
        [SerializeField] private float distanceToFollow = 20f;
        [SerializeField] private List<Dropable> drops;

        #region Status
        [Header("Status")]
        [SerializeField] private float timeToRecoverFromStun = 0.5f;
        public bool fromWave = false;

        #endregion Status


        #region Animation
        [Header("Animation")]
        [SerializeField] private float growSpeedMultiplier = 0.2f;
        #endregion Animation
        private Vector2 moveDirection;
        private Vector2 _force;
        private SpriteRenderer sr;
        private float lastAttackTime, lastStunTime = 0f;
        private bool isStunned = false;
        private EnemySpawner enemySpawner;
        private Transform target;


        override public void Start()
        {
            rb = this.GetComponent<Rigidbody2D>();
            sr = this.GetComponent<SpriteRenderer>();
            enemySpawner = FindObjectOfType<EnemySpawner>();
            base.Start();
            target = Player.Instance.transform;
            
        }


        void FixedUpdate()
        {
            if(!isStunned && Time.fixedTime - lastAttackTime > timeToWaitTillAttack)
            {
                LookTowards(target.transform);
                _force = new Vector2(moveDirection.x, moveDirection.y) * moveSpeed;
                Walk(_force);
                sr.flipX =Mathf.Sign(_force.x) < 0;
                
                if(((target.position - transform.position).magnitude < distanceToAttack) && Time.fixedTime - lastAttackTime > timeToWaitTillAttack){
                    Attack();
                    base.getWeapon().FlipPositionX(sr.flipX);
                }
            }else{
                rb.velocity = Vector2.zero;   
                an.SetBool("walk", false);
            }
        }

        override protected void Die(){
            enemySpawner.EnemyDied(this);
            base.Die();
            RandomDrop();
        }

        private void LookTowards(Transform target){
            Vector3 direction = (target.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            rb.rotation = angle;
            moveDirection = direction;
        }

        private void Walk(Vector2 force){
            an.SetBool("walk", true);
            rb.velocity = _force;
            rb.SetRotation(0);
        }

        private void Attack(){
            an.SetTrigger("attack");
            base.getWeapon().gameObject.SetActive(true);
            lastAttackTime = Time.fixedTime;
        }

        public override void ReduceHP(int pointsToReduce)
        {
            base.ReduceHP(pointsToReduce);
            lastAttackTime = Time.fixedTime;
            StopWalking();
            GetStunned();
        }

        private void StopWalking(){
            an.SetBool("walk", false);
            rb.velocity = Vector2.zero;
        }

        private void GetStunned(){
            GetStunned(timeToRecoverFromStun);
        }
        private void GetStunned(float timeToRecover){
            isStunned = true;
            lastStunTime = Time.fixedTime + timeToRecover;
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if(other.gameObject.CompareTag("PlayerHitbox")){
                Player player = other.gameObject.GetComponentInParent<Player>();
                player.Knockback(this.transform.position);
                player.ReduceHP(weapon.GetDamage());
            }
        }

        public void PlayGrowAnimation(){
            an.SetFloat("growSpeed",growSpeedMultiplier);
            an.SetTrigger("grow");
            GetStunned(an.GetCurrentAnimatorClipInfo(0)[0].clip.length);

        }

        private void RandomDrop(){

            foreach(Dropable dropable in drops){
                if(Random.Range(0, 100) < dropable.dropChance){
                    Vector2 position = new Vector2(transform.position.x + Random.Range(-1f,1f), transform.position.y + Random.Range(-1f,1f));
                    Instantiate(dropable, position, Quaternion.identity);
                }
            }
        }
    }



}
