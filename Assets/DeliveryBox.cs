using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeirdSpices{
    public class DeliveryBox : MonoBehaviour
    {
        [SerializeField] List<RequestCard> requestCards;
        [SerializeField] FoodManager foodManager;
        [SerializeField] GameManager gameManager;
        List<RequestCard> requestCardsWaitList;
        private float timeToWaitToSetCard;
        private float timeLastCardSetted;
        private GameObject rc;
        int minGold;
        int maxGold;
        int minFood;
        int maxFood;
        float minTime;
        float maxTime;
        RequestCard currentRequestCard;
        int activeRequestCards =0;

        void Start()
        {
            requestCardsWaitList = new List<RequestCard>();
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
            if(requestCardsWaitList.Count != 0 && Time.fixedTime - timeLastCardSetted  > timeToWaitToSetCard){
                currentRequestCard = requestCardsWaitList[0];
                currentRequestCard.gameObject.SetActive(true);
                currentRequestCard.SetCard(foodManager.GetRandomFood(), Random.Range(minFood,maxFood+1), Random.Range(minGold,maxGold+1), Random.Range(minTime, maxTime+1));
                timeLastCardSetted = Time.fixedTime;
                requestCardsWaitList.Remove(currentRequestCard);
                activeRequestCards += 1;
            }
            
        }

        public void AddRequestCardToWaitList(RequestCard requestCard){
            requestCardsWaitList.Add(requestCard);
            activeRequestCards -= 1;
        }
    }
}
