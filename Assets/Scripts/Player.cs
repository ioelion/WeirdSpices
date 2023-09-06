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
        [SerializeField] private float timeToEnableBeingHit = 1.2f;
        [SerializeField] private float knockbackDuration = 0.10f;
        [SerializeField] private float xDropDistance;
        [SerializeField] private float yDropDistance;

        #endregion

        #region Keys
        [Header("Keys")]
        [SerializeField] private KeyCode dropKey;
        [SerializeField] private KeyCode attackKey;
        [SerializeField] private KeyCode interactKey;
        
        #endregion
        
        #region Objects
        [Header("Objects")]
        [SerializeField] private AnimationClip walkClip;
        [SerializeField] private Animator animator;
        [SerializeField] private GameObject inventory;
        [SerializeField] private GameManager gameManager;
        [SerializeField] private BoxCollider2D hitbox;
        [SerializeField] private Tooltiper tooltiper;
        #endregion
       
               
        #region Sounds
        [Header("Sounds")]
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip attack;
        [SerializeField] private AudioClip plant;
        [SerializeField] private AudioClip getHit;
        [SerializeField] private AudioClip walk;
        [SerializeField] private AudioClip pickUpObject;
        #endregion Sounds
        private SpriteRenderer sr;
        private GameObject itemInInventory = null;
        private float lastItemTime;
        private float timeKeyToRemoveWasPressed;
        private bool isOnSoil = false;
        private Soil soil;
        private bool wasHit = false;
        private float timePlayerWasHitted;
        public static Player Instance { get; private set; }

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            } else {
                Debug.Log("Más de un Player en escena.");
            }
        }
        override public void Start()
        {
            sr = this.GetComponent<SpriteRenderer>();
            base.Start();
            soil = Soil.Instance;
            StartCoroutine(ShowTooltip("movement"));
        }

        void Update()
        {
            KeyDownActions();
            Move();
        }
        private void FixedUpdate() { 
            if(isOnSoil && itemInInventory != null && itemInInventory.CompareTag("Seed")){
                soil.Highlight(transform.position);
                StartCoroutine(ShowTooltip("plant"));
            }
            if(wasHit && Time.fixedTime - timePlayerWasHitted > timeToEnableBeingHit){
                wasHit = false;
                hitbox.gameObject.SetActive(true);
            }
        }
        
        void OnTriggerStay2D(Collider2D other)
        {
            
            if(Input.GetKey(interactKey) && (Time.fixedTime - lastItemTime  > timeToWaitTillGrab))
            {
                if(other.tag.Equals("Seed") && !other.gameObject.Equals(itemInInventory)){
                    DropItem();
                    PickUpItem(other.gameObject);
                    audioSource.PlayOneShot(pickUpObject);
                }
                else if(other.tag.Equals("Food")){
                    DropItem();
                    PickUpItem(other.gameObject);
                    gameManager.PickedUpFood(other.gameObject);
                    audioSource.PlayOneShot(pickUpObject);

                }
            }

            if(other.tag.Equals("Shop") && (Input.GetKey(interactKey) || Input.GetKey(attackKey))){
                other.gameObject.GetComponent<Shop>().Buy(transform.position);

            }

            if(other.gameObject.tag.Equals("SeedBox") && (Input.GetKey(attackKey) || Input.GetKey(interactKey)) && itemInInventory == null){
                PickUpItem(other.gameObject.GetComponent<SeedBox>().DropSeed(this.transform.position));
                
            }


        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.tag.Equals("Soil") ){
                isOnSoil = true;
            }

            if(other.tag.Equals("RequestCard") && itemInInventory != null && itemInInventory.tag.Equals("Food")){
                    other.gameObject.GetComponent<RequestCard>().ReceiveFood(itemInInventory);
                    Destroy(itemInInventory, 0.01f);
                    DropItem();
            }

            if(other.tag.Equals("TutorialRequestCard") && itemInInventory != null && itemInInventory.tag.Equals("Food")){
                    other.gameObject.GetComponent<TutorialRequestCard>().ReceiveFood(itemInInventory);
                    Destroy(itemInInventory, 0.01f);
                    DropItem();
            }
           
        }

        public void IsOnSoil(bool isOnSoil){
            this.isOnSoil = isOnSoil;
        }
        private void Move(){
            float _x = Input.GetAxis("Horizontal") * movementSpeed;
            float _y = Input.GetAxis("Vertical") * movementSpeed;
            Vector2 _force = new Vector2(_x, _y);

            if (_force != Vector2.zero)
            {
                animator.SetBool("walk", true);
                animator.SetFloat("walkSpeed", walkClip.averageDuration*movementSpeed*0.5f);
                rb.velocity = _force;
                sr.flipX = Mathf.Sign(_force.x) < 0;
                audioSource.PlayOneShot(walk);
            }
            else
            {
                animator.SetBool("walk", false);
                rb.velocity = Vector2.zero;
            }
        }

        private void KeyDownActions(){
            
            if(Input.GetKeyDown(dropKey) && itemInInventory != null && (Time.fixedTime - lastItemTime  > timeToWaitTillGrab) ){
                DropItem();
            }

            if(Input.GetKeyDown(attackKey)){
                Attack();
            }
            
            if(Input.GetKeyDown(interactKey) && itemInInventory != null && itemInInventory.tag.Equals("Seed") && isOnSoil){
                Seed seed = itemInInventory.GetComponent<Seed>();
                soil.PlantSeed(seed, this.transform.position);
                soil.ClearLastPositionHighlighted();
                DropItem();
            }
        }

        override protected void Die(){
            GameManager.Instance.LoseGame();
        }

        public override void ReduceHP(int pointsToReduce)
        {
            base.ReduceHP(pointsToReduce);
            GameManager.Instance.SetPlayerHealthPoints(base.GetHealthPoints());
            wasHit = true;
            hitbox.gameObject.SetActive(false);
            timePlayerWasHitted = Time.fixedTime;
        }

        public void RecoverHP(int pointsToAdd){
            base.AddHealthPoints(pointsToAdd);
            GameManager.Instance.SetPlayerHealthPoints(base.GetHealthPoints());
        }

        public override void Heal(int pointsToHeal)
        {
            base.Heal(pointsToHeal);
            GameManager.Instance.SetPlayerHealthPoints(base.GetHealthPoints());
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
            if(newItem != null){
                itemInInventory = newItem;
                newItem.gameObject.transform.parent = inventory.transform;
                newItem.transform.position = new Vector2(inventory.transform.position.x, inventory.transform.position.y + 1);
                lastItemTime = Time.fixedTime; 
            }
        }

        private void Attack(){
            animator.SetTrigger("attack");
            base.getWeapon().gameObject.SetActive(true);
            base.getWeapon().FlipPositionX(sr.flipX);
            audioSource.PlayOneShot(attack);

        }

        private Vector2 GetDropPosition(){
            float x = sr.flipX ? -xDropDistance : xDropDistance;
            return new Vector2(this.transform.position.x+x,this.transform.position.y+yDropDistance);
        }

        private bool IsMoving(){
            return this.rb.velocity != Vector2.zero;
        }

        private IEnumerator ShowTooltip(string tooltipName){
            //TODO rehacer
            if(!tooltiper.AlreadyDoneTooltip(tooltipName)){
                switch(tooltipName){
                    case "movement":
                        if(!IsMoving()){
                                tooltiper.ShowTooltip(tooltipName);
                                yield return new WaitForSeconds(1f);
                                StartCoroutine(ShowTooltip(tooltipName));
                            }else{
                                tooltiper.CompletedTooltip(tooltipName);
                            }
                            break;
                    case "plant":
                        if(!(Input.GetKey(interactKey)) && isOnSoil){
                                tooltiper.ShowTooltip(tooltipName);
                                yield return new WaitForSeconds(0.5f);
                                StartCoroutine(ShowTooltip(tooltipName));
                        }else if (!isOnSoil) {
                                tooltiper.HideToolTip(tooltipName);
                        }else{
                            tooltiper.CompletedTooltip(tooltipName);
                        }
                            break;
                    default:
                        break;
                }
            }
        }
        public override void Knockback(Vector3 hitterPosition)
        {
            Knockback(hitterPosition,knockbackDuration);
        }

        public void Knockback(Vector3 hitterPosition, float stunTime){
            base.Knockback(hitterPosition);
            StartCoroutine(EnablePlayer(stunTime));
            this.enabled = false;
        }

        private IEnumerator EnablePlayer(float timeToWait){
            yield return new WaitForSeconds(timeToWait);
            this.enabled =true;
        }

    }
}



