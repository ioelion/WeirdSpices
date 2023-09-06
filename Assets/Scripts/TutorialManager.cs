using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeirdSpices
{

    public class TutorialManager : MonoBehaviour
    {
        public List<SeedBox> seedBoxes;
        public List<RequestCard> requestCards;
        public DeliveryBox deliveryBox;
        public EnemySpawner enemySpawner;
        public ScoreManager scoreManager;
        public UIManager uiManager;
        public Shop shop;
        public static TutorialManager Instance { get; private set; }

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

        public void SetFirstCardState(bool enabled){
            requestCards[0].gameObject.SetActive(enabled);
        }

        public void SetSeedBoxState(bool enabled, int seedBoxNumber){
            seedBoxes[seedBoxNumber].gameObject.SetActive(enabled);
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
        public void StartTutorial(){
            SetAllSeedBoxesState(false);
            SetDeliveryBoxState(false);
            SetEnemySpawnerState(false);
            SetScoreState(false);
            SetShopState(false);
        }
    }
}
