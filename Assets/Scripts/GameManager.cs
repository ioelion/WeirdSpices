using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace WeirdSpices
{
    public class GameManager : MonoBehaviour
    {
        #region GameParameters
        [Header("Requests")]
        [SerializeField] private int minGoldRewarded;
        [SerializeField] private int maxGoldRewarded;
        private int minFoodRequired = 1;
        private int maxFoodRequired = 1;
        [SerializeField] private float minDeliverTime;
        [SerializeField] private float maxDeliverTime;
        [SerializeField] private float waitTimeBetweenCards;
        [SerializeField] private float velocitySuccesfulDelivery = 0.05f;
        [SerializeField] private float velocityFailedDelivery = -0.02f;
        [SerializeField] private float pointsToWin = 100f;
        [SerializeField] private float pointsToLose = -10f;
        [SerializeField] private float initialPoints = 40f;
        [SerializeField] float fadingPointsFactor = 0.1f;
        [Header("Player")]
        [SerializeField] private int initialPlayerHP;
        [SerializeField] private int initialMaxPlayerHP;
        private int maxPlayerHP;

        [Header("Texts")]
        public string winText = "YOU SURVIVED!";
        public string loseText = "PRESS R TO TRY AGAIN";
        public string waveText = "MONSTERS ARE COMING";

        [Header("Keys")]
        [SerializeField] private KeyCode helpKey;
        [SerializeField] private KeyCode resetKey;
        [SerializeField] private KeyCode pauseKey;
        #endregion

        #region Objects
        [Header("Objects")]
        [SerializeField] private GameObject coin;
        [SerializeField] private Player player;
        [SerializeField] private DeliveryBox deliveryBox;
        [SerializeField] private UIManager uiManager;
        [SerializeField] private List<Dropable> dropables;
        [SerializeField] private WaveManager waveManager;
        [SerializeField] private ScoreManager scoreManager;
        
        #endregion
        public int currentPlayerGold { get; private set; }
        public static GameManager Instance { get; private set; }


        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Debug.Log("Más de un GameManager en escena.");
            }
        }

        void Start()
        {   
            scoreManager.SetCurrentPoints(initialPoints);
            scoreManager.Set(velocitySuccesfulDelivery, velocityFailedDelivery, pointsToWin, pointsToLose, fadingPointsFactor);
            player.SetHP(initialPlayerHP);
            player.SetMaxHealthPoints(initialMaxPlayerHP);
            maxPlayerHP = uiManager.GetHeartQuantity();
            uiManager.SetHelpKey(helpKey);
            uiManager.SetHPParameters(initialPlayerHP, initialMaxPlayerHP);
            uiManager.SetUIGold(currentPlayerGold);
            uiManager.SetObjectivePoints(initialPoints);
            uiManager.SetObjectivePointsToWin(pointsToWin);
            TutorialManager.Instance.StartTutorial();
        }

        void Update()
        {
            if (Time.timeScale == 0 && Input.GetKeyDown(resetKey))
            {
                ResumeGame();
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
            }

            if (Input.GetKeyDown(pauseKey))
            {
                ToggleGameState();
            }

        }

        public void WinGame() { EndGame(winText); }
        public void LoseGame() { EndGame(loseText); }

        public void EndGame(string text)
        {
            uiManager.ShowEndScreen(text);
            PauseGame();
        }

        public void SetPlayerHealthPoints(int healthPoints){ uiManager.SetUIHP(healthPoints);}

        public void GainGold(int goldWon)
        {
            currentPlayerGold += goldWon;
            SetPlayerGold(currentPlayerGold);
        }

        public void LoseGold(int goldLost)
        {
            currentPlayerGold -= goldLost;
            SetPlayerGold(currentPlayerGold);
        }

        public void SetPlayerGold(int gold) { uiManager.SetUIGold(gold); }

        public void CreateCoin(Vector3 p)
        {
            if (Physics2D.OverlapPoint(p) != null)
            {
                if (!Physics2D.OverlapPoint(p).CompareTag("Coin"))
                {
                    GameObject clone;
                    clone = Instantiate(coin, p, Quaternion.identity);
                    AddToList(clone);
                }
                else
                {
                    float r = Random.Range(-0.5f, 0.5f);
                    GameObject clone;
                    clone = Instantiate(coin, new Vector3(p.x + r, p.y + r, p.z), Quaternion.identity);
                    AddToList(clone);
                }
            }
        }

        public void ToggleGameState()
        {
            if (Time.timeScale == 1)
            {
                PauseGameWithKey();
            }
            else
            {
                ResumeGameWithKey();
            }
        }
        public void PauseGame()
        {
            uiManager.SetPauseScreen(true);
            Time.timeScale = 0;
        }

        public void PauseGameWithKey()
        {
            PauseGame();
            uiManager.SetPauseText(true);
        }

        public void ResumeGameWithKey(){
            ResumeGame();
            uiManager.SetPauseText(false);
        }


        public void ResumeGame()
        {
            uiManager.SetPauseScreen(false);
            Time.timeScale = 1;
        }

        public void SuccessfulDelivery(int coinQuantity)
        {
            scoreManager.SuccesfulDelivery();
            GainGold(coinQuantity);
        }

        public void FailedDelivery()
        {
            scoreManager.FailedDelivery();
        }

        public void PickedUpFood(GameObject food)
        {
            deliveryBox.AnimateCardsWithFood(food);
        }
        public void PlayerDroppedFood()
        {
            deliveryBox.StopAnimations();
        }

        public void AddToList(GameObject item)
        {   
            if (item.GetComponent<Dropable>() != null)
            {
                Dropable itemD = item.GetComponent<Dropable>();
                dropables.Add(itemD);
            }
        }

        public void RemoveToList(GameObject item)
        {
            
            //dropables.Remove(itemD);
            
            if (item.GetComponent<Dropable>() != null)
            {
                Dropable itemD = item.GetComponent<Dropable>();
                itemD.gameObject.SetActive(false);
                //Debug.Log("Remover item= " + itemD.name);
            }
            
        }

        public Dropable RandomParentlessActiveDropable()
        {
            int i = -1;
            foreach (Dropable dropable in dropables)
            {
                i++;
                if (dropable != null && dropable.gameObject.activeSelf && (dropable.GetComponentInParent<Dropable>() != null))
                {
                    return dropable;
                }
                
            }
            return null;
        }

        public void IncorrectCombinationDone(Vector2 position){
            EnemySpawner.Instance.SpawnGrowingEnemy("Zombie",position);
        }

        public void LoadFlags(List<float> wavesTriggerPercentages){
            uiManager.LoadFlags(wavesTriggerPercentages);
        }
        
        public void WaveEnd(){
            uiManager.ClearFirstWaveFlag();
        }

        public void CheckForWaveTriggers(float percentage){
            if(waveManager.CheckForWaveTrigger(percentage)){
                uiManager.ShowWaveAnnouncement(waveText);
            };
        }

        public int GetMinGoldRewarded() { return minGoldRewarded; }
        public int GetMaxGoldRewarded() { return maxGoldRewarded; }
        public int GetMinFoodRequired() { return minFoodRequired; }
        public int GetMaxFoodRequired() { return maxFoodRequired; }
        public float GetMinDeliverTime() { return minDeliverTime; }
        public float GetMaxDeliverTime() { return maxDeliverTime; }
        public float GetWaitTimeBetweenCards() { return waitTimeBetweenCards; }
    }
}
