using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeirdSpices
{

    public class TutorialManager : MonoBehaviour
    {
        [Header("Cards")]
        public Food firstCardFood, secondCardFood;
        public TutorialRequestCard firstRequestCard;
        public TutorialRequestCard secondRequestCard;

        [Header("Objects")]
        public Enemy tutorialZombie;
        public Transform tutorialZombieSpawnPoint;
        public List<SeedBox> seedBoxes;

        [Header("Managers")]
        public DeliveryBox deliveryBox;
        public EnemySpawner enemySpawner;
        public ScoreManager scoreManager;
        public UIManager uiManager;
        public Shop shop;


        public static TutorialManager Instance { get; private set; }

        private int activatedSeedBoxesQty = 0;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Debug.Log("Más de un TutorialManager en escena.");
            }
        }

        public void SetEnemySpawnerState(bool enabled){
            enemySpawner.isSpawning(enabled);
        }

        public void SetDeliveryBoxState(bool enabled){
            deliveryBox.gameObject.SetActive(enabled);
        }

        public void SetFirstCard(){
            Debug.Log("First card activated");
            firstRequestCard.gameObject.SetActive(true);
            firstRequestCard.SetCard(firstCardFood, 1, 0, 99);
        }

        public void SetSecondCard(){
            Debug.Log("Second card activated");
            secondRequestCard.gameObject.SetActive(true);
            secondRequestCard.SetCard(secondCardFood, 1, 0, 99);
        }

        public void SetSeedBoxState(bool enabled, int seedBoxNumber){
            foreach(SeedBox seedBox in seedBoxes){
                if(seedBox.GetSeedBoxNumber() == seedBoxNumber){
                    seedBox.gameObject.SetActive(enabled);
                    activatedSeedBoxesQty++;
                    Debug.Log(activatedSeedBoxesQty);
                    if(activatedSeedBoxesQty == 2){
                        SetFirstCard();
                    }else if(activatedSeedBoxesQty == 3){
                        SetSecondCard();
                    }
                }
            }
        }

        public Vector2 GetSeedBoxPosition(int seedBoxNumber){
            foreach(SeedBox seedBox in seedBoxes){
                if(seedBox.GetSeedBoxNumber() == seedBoxNumber){
                    return seedBox.gameObject.transform.position;
                }
            }
            Debug.Log("No se encontro la seedbox bajo el numero buscado");
            return Vector2.zero;
        }

        public void SetAllSeedBoxesState(bool enabled){
            foreach(SeedBox seedBox in seedBoxes){
                seedBox.gameObject.SetActive(enabled);
            }
        }

        public void SetScoreState(bool enabled){
            uiManager.ObjectiveProgressVisible(false);
            scoreManager.gameObject.SetActive(false);
        }

        public void SetShopState(bool enabled){
            shop.gameObject.SetActive(false);
        }


        public void CardCompleted(Food food){
            if(food.GetFoodNumber() == firstCardFood.GetFoodNumber()){
                FirstCardCompleted();
            }else if(food.GetFoodNumber() == secondCardFood.GetFoodNumber()){
                SecondCardCompleted();
            }
        }
        public void FirstCardCompleted(){
            Debug.Log("First card completed");
            Instantiate(tutorialZombie, tutorialZombieSpawnPoint.position, Quaternion.identity);
        }

        public void SecondCardCompleted(){
            Debug.Log("Second card completed");
            EndTutorial();
        }

        public void DeactivateAllTutorialCards(){
            firstRequestCard.gameObject.SetActive(true);
            secondRequestCard.gameObject.SetActive(true);
        }
        public void StartTutorial(){
            Debug.Log("Tutorial started");
            DeactivateAllTutorialCards();
            SetAllSeedBoxesState(false);
            SetDeliveryBoxState(false);
            SetEnemySpawnerState(false);
            SetScoreState(false);
            SetShopState(false);
            uiManager.ObjectiveProgressVisible(false);
        }

        public void EndTutorial(){
            Debug.Log("Ending Tutorial");
            SetAllSeedBoxesState(true);
            SetDeliveryBoxState(true);
            SetEnemySpawnerState(true);
            SetScoreState(true);
            SetShopState(true);
            uiManager.ObjectiveProgressVisible(true);
            this.gameObject.SetActive(false);
        }
    }
}
