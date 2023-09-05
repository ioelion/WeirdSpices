using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

namespace WeirdSpices
{
    public class StartMenuManager : MonoBehaviour
    {
        [SerializeField] private GameObject seamless;
        [SerializeField] private TMP_Text text;
        public float min=128;
        public float max=128;

        public float textWaitTime = 0.25f;

        public float startGameWaitTime = 1f;

        private float minX,maxX,minY,maxY;
        private bool keyWasPressed = false;

        void Start()
        {
                minX=transform.position.x-min;
                maxX=transform.position.x+max;
                minY=transform.position.y-min;
                maxY=transform.position.y+max;
                text.CrossFadeAlpha(0.0f, 0f, false);
                StartCoroutine(ShowText(textWaitTime));
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            seamless.transform.position =new Vector3(Mathf.PingPong(Time.time/4,maxX*5)+minX, Mathf.PingPong(Time.time,maxY*5)+minY, transform.position.z);       
            if(Input.anyKeyDown && !keyWasPressed){
                keyWasPressed = true;
                StopAllCoroutines();
                StartCoroutine(StartGame());
            }
        }

        private void ShowText(){
            ShowText(textWaitTime);
        }


        private IEnumerator ShowText(float timeToWait){
            text.CrossFadeAlpha(1.0f, timeToWait, false);            
            yield return new WaitForSeconds(timeToWait);
            StartCoroutine(HideText(timeToWait));
        }

        private void HideText(){
            HideText(textWaitTime);
        }

        private IEnumerator HideText(float timeToWait){
            text.CrossFadeAlpha(0.0f,timeToWait, false);
            yield return new WaitForSeconds(timeToWait);
            StartCoroutine(ShowText(timeToWait));
        }

        private IEnumerator StartGame(){
            StartCoroutine(ShowText(0.1f));
            yield return new WaitForSeconds(startGameWaitTime); 
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1, LoadSceneMode.Single);
        }
    }
}
