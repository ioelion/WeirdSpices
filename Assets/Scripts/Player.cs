using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace WeirdSpices{
    public class Player : Entity
    {       
        #region Parameters
        [Header("Parameters")]
        [SerializeField] private int movementSpeed;
        [SerializeField] private float timeToWaitTillGrab = 0.5f; 
        [SerializeField] private float xDropDistance;
        [SerializeField] private float yDropDistance;
        #endregion

        #region Keys
        [Header("Keys")]
        [SerializeField] private KeyCode dropKey;
        [SerializeField] private KeyCode attackKey;
        [SerializeField] private KeyCode interactKey;
        [Header("Objects")]
        #endregion
        
        #region Objects
        [SerializeField] private Animator animator;
        [SerializeField] private GameObject inventory;
        [SerializeField] private GameManager gameManager;
        #endregion
        private Rigidbody2D rb;
        private SpriteRenderer sr;
        private GameObject itemInInventory;
        private float lastItemTime;
        private float timeKeyToRemoveWasPressed;
        private bool isOnSoil = false;
        private Soil soil;
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
        private void FixedUpdate() { 
            if(isOnSoil && itemInInventory && itemInInventory.CompareTag("Seed")){
                soil.Highlight(transform.position);
            }
        }

        private void OnCollisionStay2D(Collision2D other)
        {
            if(other.gameObject.tag.Equals("SeedBox") && Input.GetKey(attackKey)){
                other.gameObject.GetComponent<SeedBox>().DropSeed();
            }
        }

        void OnTriggerStay2D(Collider2D other)
        {
            
            if(Input.GetKey(interactKey) && (Time.fixedTime - lastItemTime  > timeToWaitTillGrab))
            {
                if(other.tag.Equals("Seed")){
                    DropItem();
                    PickUpItem(other.gameObject);
                }
                else if(other.tag.Equals("Food")){
                    DropItem();
                    PickUpItem(other.gameObject);
                    gameManager.PickedUpFood(other.gameObject);
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.tag.Equals("Soil")){
                soil = other.GetComponent<Soil>();
                isOnSoil = true;
            }

            if(other.tag.Equals("RequestCard") && itemInInventory && itemInInventory.tag.Equals("Food")){
                other.gameObject.GetComponent<RequestCard>().ReceiveFood(itemInInventory);
                Destroy(itemInInventory, 0.01f);
                DropItem();
            }
        }

        private void OnTriggerExit2D(Collider2D other) {
            if(other.tag.Equals("Soil")){
                soil.ClearLastPositionHighlighted();
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
            
            if(Input.GetKeyDown(dropKey) && itemInInventory && (Time.fixedTime - lastItemTime  > timeToWaitTillGrab) ){
                DropItem();
            }

            if(Input.GetKeyDown(attackKey)){
                Attack();
            }
            
            if(Input.GetKeyDown(interactKey) && itemInInventory && itemInInventory.tag.Equals("Seed") && isOnSoil){
                soil.PlantSeed(itemInInventory, this.transform.position);
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

        public override void Heal(int pointsToHeal)
        {
            base.Heal(pointsToHeal);
            GameManager.Instance.SetPlayerHp(base.GetHealthPoints());
        }

        private void DropItem()
        {
            if(itemInInventory != null){
                if(itemInInventory.tag.Equals("Food")) gameManager.PlayerDroppedFood();
                Transform tfchildren = inventory.transform.GetChild(0);
                
                tfchildren.position = GetDropPosition();
                inventory.transform.DetachChildren();
                itemInInventory = null;
                lastItemTime = Time.fixedTime; 
            }
        }


        private void PickUpItem(GameObject newItem){
            itemInInventory = newItem;
            newItem.gameObject.transform.parent = inventory.transform;
            newItem.transform.position = new Vector2(inventory.transform.position.x, inventory.transform.position.y + 1);
            lastItemTime = Time.fixedTime; 
        }

        private void Attack(){
            animator.SetTrigger("attack");
            base.getWeapon().gameObject.SetActive(true);
            base.getWeapon().FlipPositionX(sr.flipX);
        }

        private Vector2 GetDropPosition(){
            float x = sr.flipX ? -xDropDistance : xDropDistance;
            return new Vector2(this.transform.position.x+x,this.transform.position.y+yDropDistance);
        }

    }
}



