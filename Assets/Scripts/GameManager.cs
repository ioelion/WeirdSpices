using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using static UnityEditor.Progress;

namespace WeirdSpices
{
    public class GameManager : MonoBehaviour
    {
        #region UI
        [Header("UI")]
        [SerializeField] private TMP_Text playerLives;
        [SerializeField] private TMP_Text endText;
        [SerializeField] private GameObject greyScreen;
        [SerializeField] private Image recipeGuide;
        [SerializeField] private TMP_Text helpText;
        [SerializeField] private TMP_Text goldText;
        #endregion

        #region GameParameters
        [Header("Game Parameters")]
        [SerializeField] private int minGoldRewarded;
        [SerializeField] private int maxGoldRewarded;
        [SerializeField] private int minFoodRequired;
        [SerializeField] private int maxFoodRequired;
        [SerializeField] private float minDeliverTime;
        [SerializeField] private float maxDeliverTime;
        [SerializeField] private float waitTimeBetweenCards;
        [SerializeField] private int deliveriesRequiredToWin;
        public string winText = "GANASTE!";
        public string loseText = "Apreta R para reiniciar el nivel! ";
        [SerializeField] private KeyCode recipeKey;
        [SerializeField] private KeyCode helpKey;
        [SerializeField] private KeyCode resetKey;
        [SerializeField] private KeyCode pauseKey;
        #endregion

        #region Objects
        [Header("Objects")]
        [SerializeField] private GameObject coin;
        [SerializeField] private Player player;
        [SerializeField] private DeliveryBox deliveryBox;
        [SerializeField] private Soil soil;
        [SerializeField] private List<Dropable> dropables;
        #endregion
        public int totalGold { get; private set; }
        public static GameManager Instance { get; private set; }
        private int currentDeliveries;

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

        void Update()
        {
            if (Time.timeScale == 0 && Input.GetKeyDown(resetKey))
            {
                ResumeGame();
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
            }

            if (Input.GetKeyDown(recipeKey))
            {
                if (!recipeGuide.gameObject.activeInHierarchy)
                {
                    recipeGuide.gameObject.SetActive(true);
                }
                else
                {
                    recipeGuide.gameObject.SetActive(false);
                }
            }

            if (Input.GetKeyDown(helpKey))
            {
                if (!helpText.gameObject.activeInHierarchy)
                {
                    helpText.gameObject.SetActive(true);
                }
                else
                {
                    helpText.gameObject.SetActive(false);
                }
            }

            if (Input.GetKeyDown(pauseKey))
            {
                ToggleGameState();
            }
        }

        void FixedUpdate()
        {
            if (currentDeliveries >= deliveriesRequiredToWin)
            {
                WinGame();
            }
        }

        public void WinGame() { EndGame(winText); }
        public void LoseGame() { EndGame(loseText); }

        public void EndGame(string text)
        {
            endText.text = text;
            endText.gameObject.SetActive(true);
            PauseGame();
        }

        public void SetPlayerHp(int hp)
        {
            this.playerLives.SetText("" + hp);
        }

        public void GainGold(int goldWon)
        {
            totalGold += goldWon;
            SetPlayerGold(totalGold);
        }

        public void SetPlayerGold(int gold)
        {
            this.goldText.SetText("" + gold);
        }

        public void CreateCoin(Vector3 p)
        {
            if (Physics2D.OverlapPoint(p) != null)
            {
                if (!Physics2D.OverlapPoint(p).CompareTag("Coin"))
                {
                    GameObject clone;
                    clone = Instantiate(coin, p, Quaternion.identity);
                    EnemySpawner.Instance.AddToList(clone);
                }
                else
                {
                    float r = Random.Range(-0.5f, 0.5f);
                    GameObject clone;
                    clone = Instantiate(coin, new Vector3(p.x + r, p.y + r, p.z), Quaternion.identity);
                    EnemySpawner.Instance.AddToList(clone);
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
            greyScreen.SetActive(true);
            Time.timeScale = 0;
        }
        public void ResumeGame()
        {
            greyScreen.SetActive(false);
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
            this.currentDeliveries++;
            for (int i = 0; i < coinQuantity; i++)
            {
                Instantiate(coin, player.transform.position, Quaternion.identity);
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
            dropables.Add(item);
            Debug.Log("item Agregado =" + item.name);
        }

        public void RemoveToList(GameObject item)
        {
            Debug.Log("Remover item= " + item.name);
            dropables.Remove(item);
        }

        /*
        public Dropable RandomParentlessActiveDropable()
        {
            foreach (Dropable dropable in dropables)
            {
                //if (dropable.gameObject.IsActive && dropable.obtener padres == null o 0 )
                    {
                    return dropable;
                    break;
                }
            }
        }
        */
    }
}
