using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeirdSpices{
    public class Crop : MonoBehaviour
    {
        [SerializeField] Animator animator;
        [SerializeField] AnimationClip growAnimationClip;
        [SerializeField] SpriteRenderer spriteRenderer;
        [SerializeField] Sprite wrongCombinationSprite;
        private Soil soil;
        private float timeToWaitFullGrowth;
        private GameObject foodPrefab;
        private float timeGrowStarted;
        private bool correctCombination = true;    
        void Start()
        {
            soil = Soil.Instance;
            timeGrowStarted = Time.fixedTime;
        }

        public void StartToGrow(Food food){
            animator.enabled =true;
            this.foodPrefab = food.gameObject;
            spriteRenderer.sprite = food.GetSprite();
            timeToWaitFullGrowth = food.GetTimeNeededToGrow();
            animator.SetBool("isGrowing", true);
            animator.SetFloat("growSpeed", growAnimationClip.averageDuration/timeToWaitFullGrowth);
            timeGrowStarted = Time.fixedTime;
        }

        public void IncorrectSeeds(){
            correctCombination = false;
            animator.SetTrigger("incorrectSeeds");
            spriteRenderer.sprite = wrongCombinationSprite;
            StartCoroutine(WaitFor(animator.GetCurrentAnimatorClipInfo(0)[0].clip.length+1f));
        }

        private IEnumerator WaitFor(float seconds){
            yield return new WaitForSeconds(seconds);
            Destroy(this.gameObject);
        }

        void Awake()
        {
            timeGrowStarted = Time.fixedTime;
        }

        void FixedUpdate()
        {
            
            if(correctCombination){
                if(Time.fixedTime - timeGrowStarted  > timeToWaitFullGrowth){
                    Instantiate(foodPrefab, this.transform.position, Quaternion.identity);
                    foodPrefab = null;
                    animator.SetBool("isGrowing", false);
                    soil.RemoveSeeds(this.transform.position);
                    Destroy(this.transform.gameObject);
                }
            }
        }
        
    }
}