using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace WeirdSpices{
    public class Player : Entity
    {       
            
            //[Header("Warehouse Storage Rack")]
            [SerializeField] private Animator animator;
            
            [SerializeField] private int movementSpeed;
            
            [SerializeField] private GameObject ingredientContainer;

            [SerializeField] private float timeToWaitTillGrab = 0.2f;

            [SerializeField] private GameManager gameManager;

            [SerializeField]
            private float timeToWaitTillRemove = 1f;

            [SerializeField]
            private Soil soil;

            private Rigidbody2D rb;

            private SpriteRenderer sr;

            //TODO yoelpedemonte modificar hasSeed a hasItem asignando a cada item una clase diferente y forma de distinguirlo
            private bool hasSeed = false;

            private float lastSeedDropTime = 0f;
            private float timeKeyToRemoveWasPressed = 0f;
            
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

            private void OnCollisionStay2D(Collision2D other)
            {
                if(other.gameObject.tag.Equals("SeedBox") && Input.GetKey(KeyCode.F)){
                    SeedPickUp seedPickUp = other.gameObject.GetComponent<SeedPickUp>();
                    seedPickUp.DropSeed();
                }
            }

            void OnTriggerStay2D(Collider2D other)
            {
                if(other.tag.Equals("Ingredient") && Input.GetKey(KeyCode.F) &&!hasSeed && (Time.fixedTime - lastSeedDropTime  > timeToWaitTillGrab))
                {
                    other.gameObject.transform.parent = ingredientContainer.transform;
                    other.transform.position = new Vector2(ingredientContainer.transform.position.x, ingredientContainer.transform.position.y + 1);
                    hasSeed = true;
                }
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
                
                if(hasSeed){
                    if(Input.GetKeyDown(KeyCode.Q) && (Time.fixedTime - lastSeedDropTime  > timeToWaitTillGrab)){
                        DropSeed();
                    }
                }

                if(Input.GetKeyDown(KeyCode.Space)){
                    animator.SetTrigger("attack");
                    base.getWeapon().gameObject.SetActive(true);
                    if(sr.flipX){
                        base.getWeapon().FlipPositionX();
                    }
                }
                if(soil.IsOnSoil(this.transform.position)){
                    if(Input.GetKeyDown(KeyCode.F)){
                            timeKeyToRemoveWasPressed = Time.fixedTime;
                            if(hasSeed){
                                soil.PlantSeed(this.transform.position, ingredientContainer.transform.GetChild(0).gameObject);
                                DropSeed();
                            }
                        }
                    else if(Input.GetKey(KeyCode.F)){
                        if(Time.fixedTime - timeKeyToRemoveWasPressed >  timeToWaitTillRemove){
                            soil.RemoveSeed(this.transform.position);    
                        }else if(hasSeed){
                            soil.PlantSeed(this.transform.position, ingredientContainer.transform.GetChild(0).gameObject);
                            DropSeed();
                        }else{
                            soil.IrrigateSoil(this.transform.position);
                        }

                    }
                }

            }

            override protected void Die(){
                gameManager.EndGame();
            }

            public override void ReduceHealth(int pointsToReduce)
            {
                base.ReduceHealth(pointsToReduce);
                gameManager.SetPlayerHp(base.GetHealth());
            }

        private void DropSeed()
        {
            Transform tfchildren = ingredientContainer.transform.GetChild(0);
            tfchildren.position = new Vector2(this.transform.position.x, this.transform.position.y);
            ingredientContainer.transform.DetachChildren();
            hasSeed = false;
            lastSeedDropTime = Time.fixedTime; 
        }
    }
}



