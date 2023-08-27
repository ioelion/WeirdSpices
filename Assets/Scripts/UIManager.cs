using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace WeirdSpices
{
    public class UIManager : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private TMP_Text endText;
        [SerializeField] private GameObject greyScreen;
        [SerializeField] private TMP_Text helpText;
        [SerializeField] private TMP_Text goldText;
        [SerializeField] private HPManager hpManager;
        [SerializeField] private Slider objectiveSlider;

        private KeyCode helpKey;


        void Update() {
            //TODO mover este comportamiento al player llamando al gamemanager

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
        }

        public void SetHelpKey(KeyCode helpKey){
            this.helpKey = helpKey; 
        }

        public void ShowEndScreen(string text){
            endText.text = text;
            endText.gameObject.SetActive(true);
        }

        public void SetUIHP(int hp){
            hpManager.SetCurrentHP(hp);
        }

        public void SetUIGold(int gold){
            this.goldText.SetText("" + gold);
        }

        public void SetPauseScreen(bool active){
            greyScreen.SetActive(active);
        }

        public void SetHPParameters(int currentPlayerHP, int currentMaxPlayerHP){
            hpManager.SetParameters(currentPlayerHP, currentMaxPlayerHP);
        }

        public int GetHeartQuantity(){
            return hpManager.GetHeartQuantity();
        }

        public void SetObjectivePoints(float objectivePoints){
            objectiveSlider.SetValueWithoutNotify(objectivePoints);
        }
        public void SetObjectivePointsToWin(float maxValue){
            objectiveSlider.maxValue = maxValue;
        }

        public void ShowObjectiveProgress(){
            objectiveSlider.gameObject.SetActive(true);
        }
    }

}