using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace WeirdSpices{
    public class RequestCard : MonoBehaviour
    {
        [Header("Food")]
        [SerializeField] TMP_Text foodQuantityText;
        [SerializeField] Image foodImage; 

        [Header("Gold")]
        [SerializeField] TMP_Text goldText;
        [SerializeField] Image goldImage;

        [Header("Timer")]
        [SerializeField] TMP_Text timerText;
        [SerializeField] Image timerImage;

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
            foodQuantityText.text = "" + foodQuantity;
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
            if(foodQuantity == 0){
                DeliverRequest();
            }
            this.foodQuantity -= 1;
            foodQuantityText.text = "" + foodQuantity;

        }

        private void DeliverRequest(){
            this.gameObject.SetActive(false);
            timeLastDelivery = Time.fixedTime;
            GameManager.Instance.SuccessfulDelivery(1);
        }

        private void PauseCard(){
            this.gameObject.SetActive(false);
            timeLastDelivery = Time.fixedTime;
            DeliveryBox.Instance.AddRequestCardToWaitList(this);
        }
    }
}
