using System.Collections;
using UnityEngine;

namespace WeirdSpices {
    public class Enemy : Entity
    {
        #region Attack
        [Header("Attack")]
        [SerializeField] private float distanceToAttack = 1f;
        [SerializeField] private float timeToWaitTillAttack = 0.5f;
        #endregion Attack
        #region Status
        [Header("Status")]
        [SerializeField] private float timeToRecoverFromStun = 0.5f;


        #endregion Status
      
        #region Movement
        [Header("Movement")]
        [SerializeField] private float moveSpeed;
        [SerializeField] private float distanceToFollowPlayer;
        [SerializeField] private bool follow;
        [SerializeField] private bool preferCoin;
        [SerializeField] private bool followItem;
        [SerializeField] private float distanceToFollowItem;
        [SerializeField] private float distanceThresholdWaypoint;
        [SerializeField] private bool runToWaypoint = false;
        public Transform waypoint;
        public GameObject _item;
        public Transform _itemTarget;
        #endregion Movement
        #region Animation
        [Header("Animation")]
        [SerializeField] private float growSpeedMultiplier = 0.2f;
        #endregion Animation
        private bool inTouch = false;
        private bool touched = false;
        private bool destroying = false;
        private bool isStunned = false;
        private bool targetInAttackRange = false;
        private float lastAttackTime = 0f;
        private float lastStunTime = 0f;
        private Vector2 moveDirection;
        private Vector2 _force;
        private Transform target;
        private SpriteRenderer sr;
        private EnemySpawner enemySpawner;

        override public void Start()
        {
            sr = this.GetComponent<SpriteRenderer>();
            base.Start();          
        }

        public void SetEnemySpawner(EnemySpawner enemySpawner){
            this.enemySpawner = enemySpawner;
        }

        void Awake() {
            target = GameObject.Find("Player").transform;
            _item = EnemySpawner.Instance.GetNextDropable();
            if (_item != null) { _itemTarget = _item.transform; Debug.Log("Encontre Item"); }
            waypoint = EnemySpawner.Instance.GetWaypoint();
            int x = Random.Range(1, 5);
            if (x == 0){preferCoin = true;}
            else { preferCoin = false; }
        }
        
        void Update() {

            if (runToWaypoint) { LookTowards(waypoint); }
            else
            {
                if (follow) { LookTowards(target); }
                else
                {
                    if (followItem) { LookTowards(_itemTarget); }

                    else { rb.velocity = Vector3.zero; rb.SetRotation(0); an.SetBool("walk", false); }
                }
            }
            
        }

        void FixedUpdate()
        {
            if(!isStunned){
                if (_item == null)
                {
                    runToWaypoint = false;
                    _item = EnemySpawner.Instance.GetNextDropable();
                    if (_item != null) { _itemTarget = _item.transform; Debug.Log("Encontre item"); }
                }

                if (runToWaypoint)
                {
                    followItem = false;
                    float distanceToWaypoint = Vector2.Distance(transform.position, waypoint.position);
                    if (distanceToWaypoint > distanceThresholdWaypoint)
                    {
                        Move();
                        _item.transform.position = Vector2.MoveTowards(_item.transform.position, transform.position, moveSpeed * Time.deltaTime);
                        
                    }
                    else
                    {
                        if (!destroying)
                        {
                            Debug.Log("Destroy");
                            StartCoroutine(DestroyItem());
                        }
                    }
                }
                else
                {
                    targetInAttackRange = TargetInAttackRange(target.position,distanceToAttack);
                    if (targetInAttackRange && Time.fixedTime - lastAttackTime > timeToWaitTillAttack)
                    {
                        Attack();
                        base.getWeapon().FlipPositionX(sr.flipX);
                        
                    }
                    if(!targetInAttackRange && ImNearTarget( target.position, distanceToFollowPlayer) )
                    {
                        follow = true;
                    }
                    else {  follow = false; }

                    if (!preferCoin)
                    {
                        if (follow)
                        {
                            Move();
                        }
                        else
                        {
                            if (_itemTarget != null)
                            {
                                if(ImNearTarget(_itemTarget.position, distanceToFollowItem))
                                {
                                    followItem = true;
                                }
                                else {  followItem = false; }

                                if (followItem)
                                {
                                    Move();                              
                                }

                            }
                        }                   
                    }
                    else
                    {
                        Debug.Log(preferCoin);
                    }

                }
            }else if (Time.fixedTime - lastStunTime > timeToRecoverFromStun){
                isStunned = false;
            }
        }

        private void Move()
        {
            _force = new Vector2(moveDirection.x, moveDirection.y) * moveSpeed;
            if (_force != Vector2.zero)
            {
                Walk(_force);
                sr.flipX = Mathf.Sign(_force.x) < 0;
            }
        }

        private bool ImNearTarget( Vector3 _targetPos, float maximumDistance)
        {
            float sqrDistanceToTarget = (_targetPos - transform.position).sqrMagnitude;
            float sqrMaximumDistance = maximumDistance * maximumDistance;
            return sqrDistanceToTarget < sqrMaximumDistance;
        }

        private bool TargetInAttackRange(Vector3 _targetPos, float minimumDistance){
            return (target.position - transform.position).magnitude < minimumDistance;
        }

        IEnumerator DestroyItem()
        {
            destroying = true;
            Debug.Log("Destroying item");
             //           Agregar animacion de destruyendo
            yield return new WaitForSeconds(2);
            if (_item != null)
            {
                Destroy(_item.gameObject);
                Debug.Log("Destroyed");
            }
            destroying = false;
            runToWaypoint = false;
            touched = false;
        }

        private void OnTriggerStay2D(Collider2D col)
        {

            if ((col.gameObject.tag.Equals("Coin")) || (col.gameObject.tag.Equals("Seed")) || (col.gameObject.tag.Equals("Food")))
            {
                inTouch = true;
                if (!touched)
                {
                    Debug.Log("Picking up");
                    touched = true;
                    StartCoroutine(PickObj());
                }
            }
            else
            {
                inTouch = false;  
            }

        }

        private void PickUp()
        {
            //Transform miTransform = _itemTarget.GetComponent<Transform>();
            _itemTarget.position = new Vector3(_itemTarget.position.x, (_itemTarget.position.y) + 1, _itemTarget.position.z);
            runToWaypoint = true;
            Debug.Log("Running to waypoint");
        }


        IEnumerator PickObj()
        {
            rb.velocity = Vector2.zero; //          Desactivar
            an.SetBool("walk", false); //           Agregar animacion de agarrando
            rb.SetRotation(0);
            yield return new WaitForSeconds(2);
            if (inTouch)
            {
                PickUp();
                Debug.Log("Obj Picked up");
            }
            else
            {
                Debug.Log("Obj not picked up");
                touched = false;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, distanceToFollowPlayer);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, distanceToFollowItem);
            Gizmos.color= Color.blue;
            Gizmos.DrawWireCube(waypoint.position,new Vector3 (distanceThresholdWaypoint, distanceThresholdWaypoint, distanceThresholdWaypoint));
        }
    

        override protected void Die(){
            enemySpawner.EnemyDied();
            base.Die();
        }

        private void LookTowards(Transform target){                         //Modifique esto para que me acepte un punto en particular
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

        private void OnTriggerStay2D(Collision2D other)
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
    }

}
