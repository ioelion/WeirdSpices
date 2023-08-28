using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeirdSpices{
    public class Crop : MonoBehaviour
    {
        [SerializeField] Animator animator;
        [SerializeField] AnimationClip growAnimationClip;
        [SerializeField] SpriteRenderer spriteRenderer;
        [SerializeField] Soil soil;
        private float timeToWaitFullGrowth;
        private GameObject foodPrefab;
        private float timeGrowStarted;
    
        void Start()
        {
            soil = Soil.Instance;
        }

        public void StartToGrow(GameObject foodPrefab){
            this.foodPrefab = foodPrefab;
            Food food = foodPrefab.GetComponent<Food>();
            spriteRenderer.sprite = food.GetSprite();
            timeToWaitFullGrowth = food.GetTimeNeededToGrow();
            animator.SetBool("isGrowing", true);
            animator.SetFloat("growSpeed", growAnimationClip.averageDuration/timeToWaitFullGrowth);
            timeGrowStarted = Time.fixedTime;

        }

        void Awake()
        {
            timeGrowStarted = Time.fixedTime;
        }

        void FixedUpdate()
        {
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