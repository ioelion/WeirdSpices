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
        [SerializeField] private float objectivePointsToWin = 100f;
        [SerializeField] private float objectivePointsToLose = -10f;
        [SerializeField] private float initialObjectivePoints = 40f;
        [SerializeField] float fadingVelocityFactor = 0.1f;
        private int currentDeliveries, failedDeliveries,successfulDeliveries = 0;
        private float currentObjectivePoints;
        private float currentObjectiveVelocity = 0f;

        private float fadeVelocity = 0f;

        [Header("Player")]
        [SerializeField] private int initialPlayerHP;
        [SerializeField] private int initialMaxPlayerHP;
        private int maxPlayerHP;

        [Header("End")]
        public string winText = "GANASTE!";
        public string loseText = "Apreta R para reiniciar el nivel! ";

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
            currentObjectivePoints = initialObjectivePoints;
            player.SetHP(initialPlayerHP);
            player.SetMaxHP(initialMaxPlayerHP);
            maxPlayerHP = uiManager.GetHeartQuantity();
            uiManager.SetHelpKey(helpKey);
            uiManager.SetHPParameters(initialPlayerHP, initialMaxPlayerHP);
            uiManager.SetUIGold(currentPlayerGold);
            uiManager.SetObjectivePoints(currentObjectivePoints);
            uiManager.SetObjectivePointsToWin(objectivePointsToWin);
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

        void FixedUpdate()
        {
            if (currentObjectivePoints >= objectivePointsToWin )
            {
                WinGame();
            }

            currentObjectivePoints = currentObjectivePoints + currentObjectiveVelocity;
            uiManager.SetObjectivePoints(currentObjectivePoints);
            currentObjectiveVelocity += fadeVelocity;
            fadeVelocity = currentObjectiveVelocity != 0 ? (currentObjectiveVelocity*-1)*fadingVelocityFactor : 0;
        }


        public void WinGame() { EndGame(winText); }
        public void LoseGame() { EndGame(loseText); }

        public void EndGame(string text)
        {
            uiManager.ShowEndScreen(text);
            PauseGame();
        }

        public void SetPlayerHP(int hp)
        {
            uiManager.SetUIHP(hp);
        }

        public void GainGold(int goldWon)
        {
            currentPlayerGold += goldWon;
            SetPlayerGold(currentPlayerGold);
        }

        public void SetPlayerGold(int gold)
        {
            uiManager.SetUIGold(gold);
        }

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
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
        public void PauseGame()
        {
            uiManager.SetPauseScreen(true);
            Time.timeScale = 0;
        }
        public void ResumeGame()
        {
            uiManager.SetPauseScreen(false);
            Time.timeScale = 1;
        }

        public int GetMinGoldRewarded() { return minGoldRewarded; }
        public int GetMaxGoldRewarded() { return maxGoldRewarded; }
        public int GetMinFoodRequired() { return minFoodRequired; }
        public int GetMaxFoodRequired() { return maxFoodRequired; }
        public float GetMinDeliverTime() { return minDeliverTime; }
        public float GetMaxDeliverTime() { return maxDeliverTime; }
        public float GetWaitTimeBetweenCards() { return waitTimeBetweenCards; }

        public void SuccessfulDelivery(int coinQuantity)
        {
            if(currentDeliveries == 0){
                uiManager.ShowObjectiveProgress();
            }
            currentDeliveries++;
            successfulDeliveries++;
            currentObjectiveVelocity += velocitySuccesfulDelivery;
            GainGold(coinQuantity);
        }

        public void FailedDelivery()
        {
            currentDeliveries++;
            failedDeliveries++;
            currentObjectiveVelocity += velocityFailedDelivery;
            if(currentObjectivePoints < objectivePointsToLose)
            {
                EndGame(loseText);
            }

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
        
    }
}
