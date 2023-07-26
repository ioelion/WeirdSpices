using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeirdSpices{
    public class Enemy : Entity
    {
        private Transform target;    
        private Rigidbody2D rb;

        [SerializeField]
        private float moveSpeed;

        private Vector2 moveDirection;
        private Vector2 _force;
        private SpriteRenderer sr;
        private Animator an;
        

        // Start is called before the first frame update
        void Start()
        {
            rb = this.GetComponent<Rigidbody2D>();
            sr = this.GetComponent<SpriteRenderer>();
            an = this.GetComponent<Animator>();
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

        // Update is called once per frame
        void FixedUpdate()
        {
            if(target){
                _force = new Vector2(moveDirection.x, moveDirection.y) * moveSpeed;
                if (_force != Vector2.zero)
                {   
                    an.SetBool("playerWalk", true);
                    rb.velocity = _force;
                    rb.SetRotation(0);
                    sr.flipX = Mathf.Sign(_force.x) > 0;
                }
                /*else
                {
                    an.SetBool("playerWalk", false);
                    rb.velocity = Vector2.zero;
                }*/

                if((target.position - transform.position).magnitude < 1f){
                    an.SetTrigger("playerAttack");

                }
            }

        }
    }


}
