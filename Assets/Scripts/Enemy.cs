using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeirdSpices {
    public class Enemy : Entity
    {
        [SerializeField] private float timeToWaitTillAttack = 0.5f;
        [SerializeField] private float moveSpeed;

        [SerializeField] private float distanceToFollow;
        [SerializeField] private float distanceToPlayer;            //new
        [SerializeField] private bool follow;

        [SerializeField] private float distanceToAttack = 2f;
        private Transform target;
        private Rigidbody2D rb;
        private Vector2 moveDirection;
        private Vector2 _force;
        private SpriteRenderer sr;
        private float lastAttackTime = 0f;
        private EnemySpawner enemySpawner;

        public GameObject _item;

        public Transform _itemTarget;

        [SerializeField] private bool followCoin;

        [SerializeField] private float distanceCoin;
        [SerializeField] private float distanceCoinAbsolute;
        [SerializeField] private float distanceCoinToFollow;

        private bool inTouch = false;
        private bool touched = false;

        public Transform waypoint;
        [SerializeField] private float distanceThresholdWaypoint;
        private bool runToWaypoint = false;
        private bool destroying = false;

        override public void Start()
        {
            rb = this.GetComponent<Rigidbody2D>();
            sr = this.GetComponent<SpriteRenderer>();
            enemySpawner = FindObjectOfType<EnemySpawner>();
            base.Start();          

        }

        void Awake() {
            target = GameObject.Find("Player").transform;
            _item = EnemySpawner.Instance.GetNextDropable();
            if (_item != null) { _itemTarget = _item.transform; Debug.Log("Encontre Item"); } //this

            waypoint = EnemySpawner.Instance.GetWaypoint();
        }
        /*
        void Update() {
            if(target){LookTowards(target);}    
        }*/

        void FixedUpdate()
        {

            if (_item == null)
            {
                //Debug.Log("Buscando Item");
                runToWaypoint = false;
                _item = EnemySpawner.Instance.GetNextDropable();
                if (_item != null) { _itemTarget = _item.transform; Debug.Log("Encontre item"); }
            }

            if (runToWaypoint)
            {
                float distanceToWaypoint = Vector2.Distance(transform.position, waypoint.position);
                if (distanceToWaypoint > distanceThresholdWaypoint)
                {
                    transform.position = Vector2.MoveTowards(transform.position, waypoint.position, moveSpeed * Time.deltaTime);
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

                if (((target.position - transform.position).magnitude < distanceToAttack) && Time.fixedTime - lastAttackTime > timeToWaitTillAttack)
                {
                    Attack();
                    base.getWeapon().FlipPositionX(!sr.flipX);
                }

                distanceToPlayer = target.position.x - transform.position.x;
                float sqrDistanceToPlayer = (target.position - transform.position).sqrMagnitude;
                float sqrDistanceToFollow = distanceToFollow * distanceToFollow;

                if (sqrDistanceToPlayer < sqrDistanceToFollow) // Si esta cerca del player activa follow
                {
                    follow = true;
                }
                else
                {
                    follow = false;
                }

                if (follow)
                {

                    if (((target.position - transform.position).magnitude < distanceToAttack) && Time.fixedTime - lastAttackTime > timeToWaitTillAttack)
                    {
                        Attack();
                        base.getWeapon().FlipPositionX(!sr.flipX);
                    }

                    if (distanceToPlayer < 0)
                    {
                        transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                    }
                    if (distanceToPlayer > 0)                                // Gira el cuerpo para derecha o izquierda
                    {
                        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                    }

                    transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
                    /*
                    _force = new Vector2(moveDirection.x, moveDirection.y) * moveSpeed;
                    if (_force != Vector2.zero)
                    {
                        Walk(_force);
                        sr.flipX = Mathf.Sign(_force.x) > 0;
                    }
                    */


                }
                else
                {
                    if (_itemTarget != null)
                    {
                        distanceCoin = _itemTarget.position.x - transform.position.x;
                        float sqrDistanceCoin = (_itemTarget.position - transform.position).sqrMagnitude;
                        float sqrDistanceCoinToFollow = distanceCoinToFollow * distanceCoinToFollow;

                        if (sqrDistanceCoin < sqrDistanceCoinToFollow) // Si esta cerca de la moneda activa followCoin
                        {
                            followCoin = true;
                        }
                        else
                        {
                            followCoin = false;
                        }

                        if (followCoin)
                        {
                            if (distanceCoin < 0)
                            {
                                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                            }
                            if (distanceCoin > 0)                                          // Gira el cuerpo para derecha o izquierda
                            {
                                transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                            }

                            transform.position = Vector2.MoveTowards(transform.position, _itemTarget.position, moveSpeed * Time.deltaTime);
                        }

                    }
                }

            }
        }

        IEnumerator DestroyItem()
        {
            destroying = true;
            Debug.Log("Destroying item");
            yield return new WaitForSeconds(1);
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
                    //inTouch = true;
                    touched = true;
                    StartCoroutine(PickObj());
                }
                //Debug.Log("Tocando");
            }
            else
            {
                //inTouch = false;  //FUNCIONA MAL LA COLISION
                //Debug.Log("No Tocando");
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
            Gizmos.DrawWireSphere(transform.position, distanceToFollow);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, distanceCoinToFollow);
            Gizmos.color= Color.blue;
            Gizmos.DrawWireCube(waypoint.position,new Vector3 (distanceThresholdWaypoint, distanceThresholdWaypoint, distanceThresholdWaypoint));
        }

    

    override protected void Die(){
            enemySpawner.EnemyDied();
            base.Die();
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

    }


}
