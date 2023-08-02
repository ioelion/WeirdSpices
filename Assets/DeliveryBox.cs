using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeirdSpices{
    public class DeliveryBox : MonoBehaviour
    {
        [SerializeField] RequestCard requestCard;
        [SerializeField] FoodManager foodManager;
        [SerializeField] GameManager gameManager;
        private float timeToWaitToSetCard;
        private float timeLastCardSetted;
        private GameObject rc;

        int minGold;
        int maxGold;
        int minFood;
        int maxFood;
        float minTime;
        float maxTime;

        void Start()
        {
            rc = requestCard.gameObject;
            this.minGold = gameManager.GetMinGoldRewarded();
            this.maxGold = gameManager.GetMaxGoldRewarded();
            this.minFood = gameManager.GetMinFoodRequired();
            this.maxFood = gameManager.GetMaxFoodRequired();
            this.minTime = gameManager.GetMinDeliverTime();
            this.maxTime = gameManager.GetMaxDeliverTime();
            this.timeToWaitToSetCard = gameManager.GetWaitTimeBetweenCards();
        }

        void Update()
        {
            if(!rc.activeSelf && Time.fixedTime - timeLastCardSetted  > timeToWaitToSetCard){
                rc.SetActive(true);
                requestCard.SetCard(foodManager.GetRandomFood(), Random.Range(minGold,maxGold+1), Random.Range(minFood,maxFood+1), Random.Range(minTime, maxTime+1));
                timeLastCardSetted = Time.fixedTime;
            }
            
        }
    }
}
