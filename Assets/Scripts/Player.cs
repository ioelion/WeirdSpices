using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeirdSpices{
    public class Player : Entity
    {
        [SerializeField]
        private Animator animator;

        [SerializeField]
        private int movementSpeed;

        private Rigidbody2D rb;

        private SpriteRenderer sr;

        [SerializeField]
        private GameObject ingredientContainer;

        private bool hasIngredient = false;

        [SerializeField]
        private float timeToWaitTillGrab = 0.5f;

        private float lastItemDropTime = 0f;

        [SerializeField]
        private GameManager gameManager;
        

        
        override public void Start()
        {
            rb = this.GetComponent<Rigidbody2D>();
            sr = this.GetComponent<SpriteRenderer>();
            base.Start();
        }

        // Update is called once per frame
        void Update()
        {
            KeyDownActions();
            Move();
        }


        void OnTriggerStay2D(Collider2D other)
        {
            if(other.tag.Equals("Ingredient") && Input.GetKey(KeyCode.Q) &&!hasIngredient && (Time.fixedTime - lastItemDropTime  > timeToWaitTillGrab))
            {
                other.gameObject.transform.parent = ingredientContainer.transform;
                other.transform.position = new Vector2(ingredientContainer.transform.position.x, ingredientContainer.transform.position.y + 1);
                hasIngredient = true;
            }

            if(other.tag.Equals("Grow"))
        }

        private void Move(){
            float _x = Input.GetAxis("Horizontal") * movementSpeed;
            float _y = Input.GetAxis("Vertical") * movementSpeed;
            Vector2 _force = new Vector2(_x, _y);

            if (_force != Vector2.zero)
            {
                animator.SetBool("walk", true);
                rb.velocity = _force;
                sr.flipX = Mathf.Sign(_force.x) < 0;
            }
            else
            {
                animator.SetBool("walk", false);
                rb.velocity = Vector2.zero;

            }
        }

        private void KeyDownActions(){
            
            if(hasIngredient){
                if(Input.GetKeyDown(KeyCode.Q) && (Time.fixedTime - lastItemDropTime  > timeToWaitTillGrab)){
                    Transform tfchildren = ingredientContainer.transform.GetChild(0);
                    tfchildren.position = new Vector2(this.transform.position.x, this.transform.position.y);
                    ingredientContainer.transform.DetachChildren();
                    hasIngredient = false;
                    lastItemDropTime = Time.fixedTime; 
                }

            }

            if(Input.GetKeyDown(KeyCode.Space)){
                animator.SetTrigger("attack");
                base.getWeapon().gameObject.SetActive(true);
                if(sr.flipX){
                    base.getWeapon().FlipPositionX();
                }
            }
        }

      override protected void Die(){
            Debug.Log("he morido");
            gameManager.EndGame();
        }

        public override void ReduceHealth(int pointsToReduce)
        {
            base.ReduceHealth(pointsToReduce);
            gameManager.SetPlayerHp(base.GetHealth());
        }
    }
}

