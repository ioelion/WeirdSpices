using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeirdSpices
{
    public class ScoreManager : MonoBehaviour
    {
        [SerializeField] private float velocitySuccesfulDelivery = 0.05f;
        [SerializeField] private float velocityFailedDelivery = -0.02f;
        [SerializeField] private float pointsToWin = 100f;
        [SerializeField] private float pointsToLose = -10f;
        [SerializeField] float fadingPointsFactor = 0.1f;
        private int currentDeliveries, failedDeliveries,successfulDeliveries = 0;
        private float currentPoints, maxPointsReached = 0;
        private float currentVelocity = 0f;
        private float fadeVelocity = 0f;
        GameManager gameManager;
        UIManager uiManager;

        public void Set(float velocitySuccesfulDelivery, float velocityFailedDelivery, float pointsToWin, float pointsToLose, float fadingPointsFactor){
            this.velocitySuccesfulDelivery = velocitySuccesfulDelivery;
            this.velocityFailedDelivery = velocityFailedDelivery;
            this.pointsToWin = pointsToWin;
            this.pointsToLose = pointsToLose;
            this.fadingPointsFactor = fadingPointsFactor;
        }

        public void SetCurrentPoints(float currentPoints){
            this.currentPoints = currentPoints;
        }

        void Start()
        {
            gameManager = GameManager.Instance;    
            uiManager = UIManager.Instance;
        }



        void FixedUpdate()
        {
            if (currentPoints >= pointsToWin )
            {
                gameManager.WinGame();
            }else if(currentPoints < pointsToLose)
            {
                gameManager.LoseGame();
            }
            SetObjectivePoints();
            CheckForWaveTriggers();
        }

        public void SuccesfulDelivery(){
            currentDeliveries++;
            successfulDeliveries++;
            currentVelocity += velocitySuccesfulDelivery;
    //        uiManager.TurnGreenProgressBar();
        }

        public void FailedDelivery(){
            currentDeliveries++;
            failedDeliveries++;
            currentVelocity += velocityFailedDelivery;
//            uiManager.TurnRedProgressBar();
        }

        public void SetObjectivePoints(){
            currentPoints = currentPoints + currentVelocity;
            uiManager.SetObjectivePoints(currentPoints);
            currentVelocity += fadeVelocity;
            fadeVelocity = currentVelocity != 0 ? (currentVelocity*-1)*fadingPointsFactor : 0;
            if(currentPoints > maxPointsReached) maxPointsReached = currentPoints;
        }

        public void CheckForWaveTriggers(){
            gameManager.CheckForWaveTriggers(maxPointsReached*100/pointsToWin);
        }


    }
}
