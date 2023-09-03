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
                StartCoroutine(ShowText());
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            seamless.transform.position =new Vector3(Mathf.PingPong(Time.time/4,maxX*5)+minX, Mathf.PingPong(Time.time,maxY*5)+minY, transform.position.z);       
            if(Input.anyKeyDown && !keyWasPressed){
                keyWasPressed = true;
                StartCoroutine(StartGame());
            }
        }

        private IEnumerator ShowText(){
            text.CrossFadeAlpha(1.0f, textWaitTime, false);            
            yield return new WaitForSeconds(textWaitTime);
            StartCoroutine(HideText());
        }

        private IEnumerator HideText(){
            text.CrossFadeAlpha(0.0f,textWaitTime, false);
            yield return new WaitForSeconds(textWaitTime);
            StartCoroutine(ShowText());
        }

        private IEnumerator StartGame(){
            yield return new WaitForSeconds(startGameWaitTime); 
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1, LoadSceneMode.Single);
        }
    }
}
