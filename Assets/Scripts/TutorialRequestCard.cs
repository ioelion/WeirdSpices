using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace WeirdSpices{
    public class TutorialRequestCard : MonoBehaviour
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
        private Sprite foodRequiredSprite;
        private int foodQuantity;
        private int rewardGold;
        private float maxTimeToDeliver;
        private float timeLastDelivery;


        void Start()
        {
            timeLastDelivery = Time.fixedTime;
            gameObject.SetActive(false);
        }

        void FixedUpdate()
        {
            timerText.text = "" + Mathf.RoundToInt(maxTimeToDeliver - (Time.fixedTime - timeLastDelivery));
            if(Time.fixedTime - timeLastDelivery  > maxTimeToDeliver){
                timeLastDelivery = Time.fixedTime;
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
        }

        public void SetCardUI(){
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
                Deliver();
            }


        }

        private void Deliver(){
            timeLastDelivery = Time.fixedTime;
            TutorialManager.Instance.CardCompleted(foodRequired);
            animator.SetTrigger("success");
            StartCoroutine(Deactivation());
        }

        private void Deactivate(){

            Destroy(this.gameObject);
        }

        private void Fail(){
            animator.SetBool("failed",true);
            StartCoroutine(Deactivation());
        }

        private IEnumerator Deactivation(){
            SetActiveTextsAndImages(false);
            yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0)[0].clip.length); 
            Deactivate();
        }

        public void SetActiveTextsAndImages(bool active){
            foodImage.gameObject.gameObject.SetActive(active);
            goldImage.gameObject.gameObject.SetActive(active);
            timerImage.gameObject.gameObject.SetActive(active);
            ingredient1Img.gameObject.gameObject.SetActive(active);
        }
    }
}
