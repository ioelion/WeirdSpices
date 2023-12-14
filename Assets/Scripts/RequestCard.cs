using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace WeirdSpices{
    public class RequestCard : MonoBehaviour
    {
        [Header("Food")]
        [SerializeField] private TMP_Text foodText;
        [SerializeField] private Image foodImage; 

        [Header("Gold")]
        [SerializeField] private TMP_Text goldText;
        [SerializeField] private Image goldImage;

        [Header("Timer")]
        [SerializeField] private TMP_Text timerText;
        [SerializeField] private Image timerImage;

        [Header("Recipe")]
        [SerializeField] private Image ingredient1Img;
        [SerializeField] private Image ingredient2Img;

        [Header("Objects")]
        [SerializeField] private Animator animator;

        [SerializeField] private Seed ingredient1;
        [SerializeField] private Seed ingredient2;
        [SerializeField] private Food foodRequired;

        [SerializeField] private GameObject recipe;
        [SerializeField] private Sprite backgroundSprite;
        private Sprite foodRequiredSprite;
        private int foodQuantity;
        private int rewardGold;
        private float maxTimeToDeliver;
        private float timeLastDelivery;


        void Start()
        {
            timeLastDelivery = Time.fixedTime;
        }

        void FixedUpdate()
        {
            timerText.text = "" + Mathf.RoundToInt(maxTimeToDeliver - (Time.fixedTime - timeLastDelivery));
            if(Time.fixedTime - timeLastDelivery  > maxTimeToDeliver){
                Fail();
            }
        }
        public void SetCard(Food foodRequired, int foodQuantity, int rewardGold, float maxTimeToDeliver){
            SetActiveTextsAndImages(true);
            this.foodRequired = foodRequired;
            this.foodRequiredSprite = foodRequired.GetSprite();
            this.foodQuantity = foodQuantity;
            this.rewardGold = rewardGold;
            this.maxTimeToDeliver = maxTimeToDeliver;
            List<Seed> seedsNeeded = foodRequired.GetSeedsNeeded();
            this.ingredient1 = seedsNeeded[0];
            this.ingredient2 = seedsNeeded[1];
            SetCardUI();
            timeLastDelivery = Time.fixedTime;
            this.GetComponent<SpriteRenderer>().sprite = backgroundSprite;
        }

        public void SetCardUI(){
            //foodText.text = "" + foodQuantity;
            goldText.text = "" + rewardGold;
            foodImage.sprite = foodRequiredSprite;
            ingredient1Img.sprite = ingredient1.GetSprite();
            ingredient2Img.sprite = ingredient2.GetSprite();
        }

        public void ReceiveFood(GameObject food){
            if(food.GetComponent<SpriteRenderer>().sprite == foodImage.sprite){
                ReduceFoodLeft(1);
            }else if(food.GetComponent<SpriteRenderer>().sprite != foodImage.sprite){
                Fail();
            }
        }

        public Food GetFoodRequired(){
            return foodRequired;
        }

        public Sprite GetFoodRequiredSprite(){
            return foodRequiredSprite;
        }

        public void PlayAnimation(){
            animator.SetBool("playerHasFood", true);
        }

        public void StopAnimation(){
            animator.SetBool("playerHasFood", false);
        }

        private void ReduceFoodLeft(int quantity){
            this.foodQuantity -= 1;
            Deliver();
            //foodText.text = "" + foodQuantity;
        }

        private void Deliver(){
            timeLastDelivery = Time.fixedTime;
            animator.SetTrigger("success");
            GameManager.Instance.SuccessfulDelivery(rewardGold);
            StartCoroutine(Deactivation());
        }


        private void Fail(){
            GameManager.Instance.FailedDelivery();
            animator.SetTrigger("fail");
            StartCoroutine(Deactivation());
        }

        private IEnumerator Deactivation(){
            SetActiveTextsAndImages(false);
            yield return new WaitForSeconds(0.5f);//animator.GetCurrentAnimatorClipInfo(0)[0].clip.length); 
            Deactivate();
        }
        
        private void Deactivate(){
            timeLastDelivery = Time.fixedTime;
            DeliveryBox.Instance.AddRequestCardToWaitList(this);  
        }

        
        public void SetActiveTextsAndImages(bool active){
            foodText.gameObject.gameObject.SetActive(active);
            goldText.gameObject.gameObject.SetActive(active);
            timerText.gameObject.gameObject.SetActive(active);
            recipe.SetActive(active);
        }
    }
}
