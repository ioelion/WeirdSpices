using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.Mathematics;

namespace WeirdSpices{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] 
        private TMP_Text playerLives;
        public TMP_Text endText;
        public GameObject endScreen;
        public TMP_Text gold;
        public GameObject coin;

        public static GameManager Instance { get; private set; }    
        public int TotalGold { get { return totalGold; } }
        private int totalGold;


        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            } else {
                Debug.Log("Mas de un GameManager en escena");
            }
        }
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.R)){
                ResumeGame();
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex,LoadSceneMode.Single);
            }
        }

        public void EndGame(){
            endText.gameObject.SetActive(true);
            endScreen.SetActive(true);
            PauseGame();
        }

        public void SetPlayerHp(int hp){
            this.playerLives.SetText(""+hp); 
        }

        public void GainGold(int goldWon)
        {
            totalGold += goldWon;
            SetPlayerGold(totalGold);
        }

        public void SetPlayerGold(int gold)
        {
            this.gold.SetText("Oro " + gold);
        }

        public void CreateCoin(Vector3 p)
        {
            Instantiate(coin, p, Quaternion.identity);
        }
        public void PauseGame ()
        {
            Time.timeScale = 0;
        }
        public void ResumeGame ()
        {
            Time.timeScale = 1;
        }
    }
}
