using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeirdSpices{
    public class DeliveryBox : MonoBehaviour
    {
        [SerializeField] private List<RequestCard> requestCards;
        private List<RequestCard> requestCardsWaitList;
        private float timeToWaitToSetCard;
        private float timeLastCardSetted;
        private GameObject rc;
        private int minGold, maxGold, minFood, maxFood;
        private float minTime, maxTime;
        private RequestCard currentRequestCard;
        private int activeRequestCards =0;
        public static DeliveryBox Instance { get; private set; }    

        void Start()
        {
            requestCardsWaitList = new List<RequestCard>();
            GetGameManagerVariables();
        }

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            } else {
                Debug.Log("Más de un DeliveryBox en escena.");
            }
        }


        void Update()
        {
            if(requestCardsWaitList.Count != 0 && Time.fixedTime - timeLastCardSetted  > timeToWaitToSetCard){
                UpdateRandomRequestCard();
            }
        }

        public void AddRequestCardToWaitList(RequestCard requestCard){
            requestCardsWaitList.Add(requestCard);
            activeRequestCards -= 1;
            timeLastCardSetted = Time.fixedTime;
        }

        private void RemoveRequestCardFromWaitList(RequestCard requestCard){
            requestCardsWaitList.Remove(requestCard);
            activeRequestCards += 1;
            timeLastCardSetted = Time.fixedTime;
        }

        private void UpdateRandomRequestCard(){
            currentRequestCard = requestCardsWaitList[0];
            currentRequestCard.gameObject.SetActive(true);
            currentRequestCard.SetCard(FoodManager.Instance.GetRandomFood(), Random.Range(minFood,maxFood+1), Random.Range(minGold,maxGold+1), Random.Range(minTime, maxTime+1));
            RemoveRequestCardFromWaitList(currentRequestCard);
        }

        private void GetGameManagerVariables(){
            this.minGold = GameManager.Instance.GetMinGoldRewarded();
            this.maxGold = GameManager.Instance.GetMaxGoldRewarded();
            this.minFood = GameManager.Instance.GetMinFoodRequired();
            this.maxFood = GameManager.Instance.GetMaxFoodRequired();
            this.minTime = GameManager.Instance.GetMinDeliverTime();
            this.maxTime = GameManager.Instance.GetMaxDeliverTime();
            this.timeToWaitToSetCard = GameManager.Instance.GetWaitTimeBetweenCards();
        }
    }
}
