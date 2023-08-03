using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace WeirdSpices{
    public class Player : Entity
    {       
            [Header("Parameters")]
            [SerializeField] private int movementSpeed;
            [SerializeField] private float timeToWaitTillGrab = 0.2f; 
            [SerializeField] private float timeToWaitTillRemove;
            [SerializeField] private KeyCode dropKey;
            [SerializeField] private KeyCode attackKey;
            [SerializeField] private KeyCode interactKey;
            [Header("Objects")]
            [SerializeField] private Animator animator;
            [SerializeField] private GameObject ingredientContainer;
            private Rigidbody2D rb;
            private SpriteRenderer sr;
            private bool hasItem = false;
            private bool hasSeed = false;
            private float lastItemDropTime;
            private float timeKeyToRemoveWasPressed;
            private bool isOnSoil = false;
            private FertileSoil fertileSoil;
            override public void Start()
            {
                rb = this.GetComponent<Rigidbody2D>();
                sr = this.GetComponent<SpriteRenderer>();
                base.Start();
            }

            void Update()
            {
                KeyDownActions();
                Move();
            }

            private void OnCollisionStay2D(Collision2D other)
            {
                if(other.gameObject.tag.Equals("SeedBox") && Input.GetKey(interactKey)){
                    other.gameObject.GetComponent<SeedBox>().DropSeed();
                }
            }

            void OnTriggerStay2D(Collider2D other)
            {
                
                if((other.tag.Equals("Seed") || other.tag.Equals("Food")) && Input.GetKey(interactKey) &&!hasItem && (Time.fixedTime - lastItemDropTime  > timeToWaitTillGrab))
                {
                    other.gameObject.transform.parent = ingredientContainer.transform;
                    other.transform.position = new Vector2(ingredientContainer.transform.position.x, ingredientContainer.transform.position.y + 1);
                    hasItem = true;
                    if(other.tag.Equals("Seed")){
                        hasSeed = true;
                    }
                    
                }
            }

            private void OnTriggerEnter2D(Collider2D other)
            {
                 if(other.tag.Equals("FertileSoil")){
                    fertileSoil = other.GetComponent<FertileSoil>();
                    isOnSoil = true;
                 }

                 if(other.tag.Equals("RequestCard") && hasItem && !hasSeed ){
                    other.gameObject.GetComponent<RequestCard>().ReceiveFood(ingredientContainer.transform.GetChild(0).gameObject);
                    Destroy(ingredientContainer.transform.GetChild(0).gameObject);
                    ingredientContainer.transform.DetachChildren();
                    hasItem = false;
                 }
            }

            private void OnTriggerExit2D(Collider2D other) {
                if(other.tag.Equals("FertileSoil")){
                    fertileSoil = null;
                    isOnSoil = false;
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
                
                if(hasItem){
                    if(Input.GetKeyDown(dropKey) && (Time.fixedTime - lastItemDropTime  > timeToWaitTillGrab)){
                        DropItem();
                    }
                }

                if(Input.GetKeyDown(attackKey)){
                    animator.SetTrigger("attack");
                    base.getWeapon().gameObject.SetActive(true);
                    if(sr.flipX){
                        base.getWeapon().FlipPositionX();
                    }
                }
                
                if(Input.GetKeyDown(interactKey) && hasSeed && isOnSoil){
                    fertileSoil.PlantSeed(ingredientContainer.transform.GetChild(0).gameObject);
                    DropItem();
                }
            }

            override protected void Die(){
                GameManager.Instance.LoseGame();
            }

            public override void ReduceHealth(int pointsToReduce)
            {
                base.ReduceHealth(pointsToReduce);
                GameManager.Instance.SetPlayerHp(base.GetHealthPoints());
            }

        private void DropItem()
        {
            Transform tfchildren = ingredientContainer.transform.GetChild(0);
            tfchildren.position = new Vector2(this.transform.position.x, this.transform.position.y);
            ingredientContainer.transform.DetachChildren();
            hasItem= false;
            hasSeed= false;
            lastItemDropTime = Time.fixedTime; 
        }

    }
}



