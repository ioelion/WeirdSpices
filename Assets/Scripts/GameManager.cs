using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
namespace WeirdSpices{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] 
        private TMP_Text playerLives;
        public TMP_Text endText;
        public GameObject endScreen;
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
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
