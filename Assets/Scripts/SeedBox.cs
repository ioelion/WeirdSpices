using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace WeirdSpices{
    public class SeedBox : MonoBehaviour
    {
        [Header("Parameters")]
        [SerializeField] private TMP_Text quantityLeft; 
        [SerializeField] private float timeToWaitTillSeedDrop = 1.5f;
        [SerializeField] private int initialSeeds = 50;
        [SerializeField] private int currentSeeds;



        [Header("Objects")]
        [SerializeField] private Transform dispenser;
        [SerializeField] private GameObject seedPrefab;
        [SerializeField] private Animator animator;
        [SerializeField] private Tooltiper tooltiper;
        
        private float lastSeedDropTime = 0f;
        
        void Start()
        {
            currentSeeds = initialSeeds;
            //quantityLeft.text = "" + currentSeeds;
        }


        public void DropSeed(Vector2 positionToDrop)
        {
            
            if((Time.fixedTime - lastSeedDropTime  > timeToWaitTillSeedDrop)){
                lastSeedDropTime = Time.fixedTime;                               
                if (currentSeeds > 0 ) 
                {
                    Vector2 posS = dispenser.transform.position;
                    if ((Physics2D.OverlapPoint(posS) != null) && (Physics2D.OverlapPoint(posS).CompareTag("Seed") ) )
                    {
                        Debug.Log("Ya hay una semilla");
                    }
                    else
                    {                 
                        animator.SetTrigger("wasHit");
                        GameObject clone = Instantiate(seedPrefab, positionToDrop, Quaternion.identity);
                        GameManager.Instance.AddToList(clone);
                        currentSeeds--;                    
                    }
                }
                else
                {
                    Debug.Log("Maximo de semillas agarradas");
                }
            }

            if(tooltiper.ShowingTooltip() && !tooltiper.AlreadyDoneTooltip("seedbox") ) {
                tooltiper.CompletedTooltip("seedbox");
            }

        }

        void OnTriggerStay2D(Collider2D other)
        {
            if(other.gameObject.CompareTag("PlayerHitbox")){
                if(!tooltiper.AlreadyDoneTooltip("seedbox")){
                    tooltiper.ShowTooltip("seedbox");
                }
            }
        }
    }
}