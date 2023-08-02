using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace WeirdSpices{
    public class RequestCard : MonoBehaviour
    {
        [SerializeField] TMP_Text quantityText;
        [SerializeField] Image foodImage; 
        [SerializeField] TMP_Text goldText;
        [SerializeField] Image goldImage;
        [SerializeField] TMP_Text timerText;
        [SerializeField] Image timerImage;
        [SerializeField] GameManager gameManager;
        [SerializeField] DeliveryBox deliveryBox;
        GameObject foodRequired;
        int foodQuantity;
        int rewardGold;
        float maxTimeToDeliver;
        float timeLastDelivery;

        void FixedUpdate()
        {
            timerText.text = "" + Mathf.RoundToInt(maxTimeToDeliver - (Time.fixedTime - timeLastDelivery));
            if(Time.fixedTime - timeLastDelivery  > maxTimeToDeliver){
                PauseCard();
            }
        }
        public void SetCard(GameObject foodRequired, int foodQuantity, int rewardGold, float maxTimeToDeliver){
            this.foodRequired = foodRequired;
            this.foodQuantity = foodQuantity;
            this.rewardGold = rewardGold;
            this.maxTimeToDeliver = maxTimeToDeliver;
            quantityText.text = "" + foodQuantity;
            goldText.text = "" + rewardGold;
            foodImage.sprite = foodRequired.GetComponent<SpriteRenderer>().sprite;
            this.maxTimeToDeliver = maxTimeToDeliver;
            timeLastDelivery = Time.fixedTime;
        }

        public void ReceiveFood(GameObject food){
            if(food.GetComponent<SpriteRenderer>().sprite == foodImage.sprite){
                ReduceFoodLeft(1);
            }
        }

        private void ReduceFoodLeft(int quantity){
            this.foodQuantity -= 1;
            quantityText.text = "" + foodQuantity;
            if(foodQuantity == 0){
                DeliverRequest();
            }
        }

        private void DeliverRequest(){

            gameManager.SuccessfulDelivery(1);
        }

        private void PauseCard(){
            this.gameObject.SetActive(false);
            timeLastDelivery = Time.fixedTime;
            deliveryBox.AddRequestCardToWaitList(this);
        }
    }
}
