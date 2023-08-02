using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
namespace WeirdSpices{
    public class GameManager : MonoBehaviour
    {
        
        [SerializeField] TMP_Text playerLives;
        [SerializeField] TMP_Text endText;
        [SerializeField] GameObject endScreen;

        [SerializeField] int minGoldRewarded;
        [SerializeField] int maxGoldRewarded;
        [SerializeField] int minFoodRequired;
        [SerializeField] int maxFoodRequired;
        [SerializeField] float minDeliverTime;
        [SerializeField] float maxDeliverTime;
        [SerializeField] float waitTimeBetweenCards;
        [SerializeField] int deliveriesRequiredToWin;

        int currentDeliveries;

        
        void Update()
        {
            if(Time.timeScale == 0 && Input.GetKeyDown(KeyCode.R)){
                ResumeGame();
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex,LoadSceneMode.Single);
            }

            if(currentDeliveries >= deliveriesRequiredToWin){
                EndGame("GANASTE!");
            }
        }

        public void EndGame(string newEndText){
            endText.text = newEndText;
            endText.gameObject.SetActive(true);
            endScreen.SetActive(true);
            PauseGame();
        }

        public void SetPlayerHp(int hp){
            this.playerLives.SetText(""+hp); 
        }

        public void PauseGame ()
        {
            Time.timeScale = 0;
        }
        public void ResumeGame ()
        {
            Time.timeScale = 1;
        }

        public int GetMinGoldRewarded(){
            return minGoldRewarded;
        }
        public int GetMaxGoldRewarded(){
            return maxGoldRewarded;
        }
        public int GetMinFoodRequired(){
            return minFoodRequired;
        }
        public int GetMaxFoodRequired(){
            return maxFoodRequired;
        }
        public float GetMinDeliverTime(){
            return minDeliverTime;
        }
        public float GetMaxDeliverTime(){
            return maxDeliverTime;
        }

        public float GetWaitTimeBetweenCards(){
            return waitTimeBetweenCards;
        }


        public void SuccessfulDelivery(int number){
            this.currentDeliveries += number;
        }
    }
}
