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
        [SerializeField] private GameObject inventory;
        private Rigidbody2D rb;
        private SpriteRenderer sr;
        private GameObject itemInInventory;
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
            if(other.gameObject.tag.Equals("SeedBox") && Input.GetKey(attackKey)){
                other.gameObject.GetComponent<SeedBox>().DropSeed();
            }
        }

        void OnTriggerStay2D(Collider2D other)
        {
            
            if(Input.GetKey(interactKey) && (Time.fixedTime - lastItemDropTime  > timeToWaitTillGrab))
            {
                if(other.tag.Equals("Seed")){
                    DropItem();
                    PickUpItem(other.gameObject);
                }
                else if(other.tag.Equals("Food")){
                    DropItem();
                    PickUpItem(other.gameObject);
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.tag.Equals("FertileSoil")){
                fertileSoil = other.GetComponent<FertileSoil>();
                isOnSoil = true;
            }

            if(other.tag.Equals("RequestCard") && itemInInventory && itemInInventory.tag.Equals("Food")){
                other.gameObject.GetComponent<RequestCard>().ReceiveFood(inventory.transform.GetChild(0).gameObject);
                Destroy(inventory.transform.GetChild(0).gameObject, 1f);
                DropItem();
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
            
            if(Input.GetKeyDown(dropKey) && itemInInventory && (Time.fixedTime - lastItemDropTime  > timeToWaitTillGrab) ){
                DropItem();
            }

            if(Input.GetKeyDown(attackKey)){
                Attack();
            }
            
            if(Input.GetKeyDown(interactKey) && itemInInventory && itemInInventory.tag.Equals("Seed") && isOnSoil){
                fertileSoil.PlantSeed(inventory.transform.GetChild(0).gameObject);
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
            if(itemInInventory != null){
                Transform tfchildren = inventory.transform.GetChild(0);
                tfchildren.position = new Vector2(this.transform.position.x, this.transform.position.y);
                inventory.transform.DetachChildren();
                itemInInventory = null;
                lastItemDropTime = Time.fixedTime; 
            }
        }


        private void PickUpItem(GameObject newItem){
            itemInInventory = newItem;
            newItem.gameObject.transform.parent = inventory.transform;
            newItem.transform.position = new Vector2(inventory.transform.position.x, inventory.transform.position.y + 1);
        }

        private void Attack(){
            animator.SetTrigger("attack");
            base.getWeapon().gameObject.SetActive(true);
            if(sr.flipX) base.getWeapon().FlipPositionX();
        }
    }
}



