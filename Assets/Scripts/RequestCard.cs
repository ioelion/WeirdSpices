using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace WeirdSpices{
    public class RequestCard : MonoBehaviour
    {
        [Header("Food")]
        [SerializeField] private TMP_Text foodQuantityText;
        [SerializeField] private Image foodImage; 

        [Header("Gold")]
        [SerializeField] private TMP_Text goldText;
        [SerializeField] private Image goldImage;

        [Header("Timer")]
        [SerializeField] private TMP_Text timerText;
        [SerializeField] private Image timerImage;

        private GameObject foodRequired;
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
        public void SetCard(GameObject foodRequired, int foodQuantity, int rewardGold, float maxTimeToDeliver){
            this.foodRequired = foodRequired;
            this.foodQuantity = foodQuantity;
            this.rewardGold = rewardGold;
            this.maxTimeToDeliver = maxTimeToDeliver;
            SetCardUI();
            timeLastDelivery = Time.fixedTime;
        }

        public void SetCardUI(){
            foodQuantityText.text = "" + foodQuantity;
            goldText.text = "" + rewardGold;
            foodImage.sprite = foodRequired.GetComponent<SpriteRenderer>().sprite;
        }

        public void ReceiveFood(GameObject food){
            if(food.GetComponent<SpriteRenderer>().sprite == foodImage.sprite){
                ReduceFoodLeft(1);
            }
        }

        private void ReduceFoodLeft(int quantity){
            this.foodQuantity -= 1;
            foodQuantityText.text = "" + foodQuantity;
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
