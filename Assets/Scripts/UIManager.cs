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
        [SerializeField] private TMP_Text pauseText;
        [SerializeField] private TMP_Text helpText;
        [SerializeField] private TMP_Text goldText;
        [SerializeField] private HPManager hpManager;
        [SerializeField] private Slider objectiveSlider;
        [SerializeField] private GameObject waveFlagPrefab;
        [SerializeField] private GameObject waveFlagsGroup;
        [SerializeField] private TMP_Text waveAnnouncement;
        [SerializeField] private float timeToHideWaveAnn;
        private KeyCode helpKey;

        private List<GameObject> flags;

        private List<GameObject> clearedFlags;
        public static UIManager Instance { get; private set; }

        private List<string> alreadyDoneTooltips = new List<string>();

        private HashSet<string> tooltipsShowing = new HashSet<string>();

        private string tooltipShowing = "";


        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Debug.Log("Más de un UIManager en escena.");
            }
        }



        void Start()
        {
            flags = new List<GameObject>();
            clearedFlags= new List<GameObject>();
        }

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

        public void SetPauseText(bool active){
            pauseText.gameObject.SetActive(active);
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

        public void LoadFlags(List<float> wavesTriggerPercentages){
            Vector2 localPosition = waveFlagsGroup.transform.localPosition;
            RectTransform rectTransform = objectiveSlider.gameObject.GetComponent<RectTransform>();
            float width = rectTransform.rect.width;
            float height = rectTransform.rect.height;
            RectTransform rfWaveFlag = waveFlagPrefab.GetComponent<RectTransform>();
            float offsetToCenterFlag = rfWaveFlag.localScale.x * rfWaveFlag.rect.width /5;
            foreach(float percentage in wavesTriggerPercentages){
                Vector3 position = new Vector3(localPosition.x - offsetToCenterFlag - width/2 + width * percentage/100 , localPosition.y+height/2, 0f);
                GameObject gameObject = Instantiate(waveFlagPrefab,Vector3.zero ,Quaternion.identity,waveFlagsGroup.transform);
                gameObject.transform.localPosition = position;
                gameObject.GetComponent<RectTransform>().localScale = new Vector3(0.4f, 0.4f, 1f);
                flags.Add(gameObject);
            }
        }

        public void ShowWaveAnnouncement(string text){
            waveAnnouncement.gameObject.SetActive(true);
            waveAnnouncement.text = text;
            waveAnnouncement.CrossFadeAlpha(0.0f, 0f, false);
            waveAnnouncement.CrossFadeAlpha(1.0f, 0.25f, false);
            StartCoroutine(HideWaveAnnouncement());
        }


        private IEnumerator HideWaveAnnouncement(){
            yield return new WaitForSeconds(timeToHideWaveAnn);
            waveAnnouncement.CrossFadeAlpha(0.0f, 0.25f, false);
            yield return new WaitForSeconds(1f);
            waveAnnouncement.gameObject.SetActive(false);
        }

        public void ClearFirstWaveFlag(){
            flags[0].SetActive(false);
            clearedFlags.Add(flags[0]);
            flags.Remove(flags[0]);
        }


        public bool AlreadyDoneTooltip(string tooltipName){
            
            foreach(string name in alreadyDoneTooltips){
                if(name.Equals(tooltipName)) return true;
            }
            return false;
        }

        public void AddCompletedTooltip(string tooltipName){
            alreadyDoneTooltips.Add(tooltipName);
            tooltipsShowing.Remove(tooltipName);
        }

        public bool isShowingTooltip(string tooltipName){
            return tooltipsShowing.Contains(tooltipName);
        }

        public void showingTooltip(string tooltipName){
            tooltipsShowing.Add(tooltipName);
        }
        
    }

}
