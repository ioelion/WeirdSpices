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

        [ReadOnly] private Seed ingredient1;
        [ReadOnly] private Seed ingredient2;
        [ReadOnly] private Food foodRequired;
        private Sprite foodRequiredSprite;
        private int foodQuantity;
        private int rewardGold;
        private float maxTimeToDeliver;
        private float timeLastDelivery;



        void FixedUpdate()
        {
            timerText.text = "" + Mathf.RoundToInt(maxTimeToDeliver - (Time.fixedTime - timeLastDelivery));
            if(Time.fixedTime - timeLastDelivery  > maxTimeToDeliver){
                PauseCard();
            }
        }
        public void SetCard(Food foodRequired, int foodQuantity, int rewardGold, float maxTimeToDeliver){
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
            //foodText.text = "" + foodQuantity;
            if(foodQuantity == 0){
                DeliverRequest();
            }


        }

        private void DeliverRequest(){
            this.gameObject.SetActive(false);
            timeLastDelivery = Time.fixedTime;
            GameManager.Instance.SuccessfulDelivery(rewardGold);
        }

        private void PauseCard(){
            this.gameObject.SetActive(false);
            timeLastDelivery = Time.fixedTime;
            DeliveryBox.Instance.AddRequestCardToWaitList(this);
        }
    }
}
