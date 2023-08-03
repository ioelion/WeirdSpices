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
        [SerializeField] private Transform Dispenser;
        [SerializeField] private GameObject Seed;
        
        private float lastSeedDropTime = 0f;
        
        void Start()
        {
            currentSeeds = initialSeeds;
            quantityLeft.text = "" + currentSeeds;
        }


        public void DropSeed()
        {
            if((Time.fixedTime - lastSeedDropTime  > timeToWaitTillSeedDrop)){
                lastSeedDropTime = Time.fixedTime;                               
                if (currentSeeds > 0 ) 
                {
                    Vector2 posS = Dispenser.transform.position;
                    if ((Physics2D.OverlapPoint(posS) != null) && (Physics2D.OverlapPoint(posS).CompareTag("Seed") ) )
                    {
                        Debug.Log("Ya hay una semila");
                    }
                    else
                    {                       
                        Instantiate(Seed, Dispenser.position, Dispenser.rotation);
                        Debug.Log("Semilla dropeada");
                        currentSeeds--;
                        Debug.Log(currentSeeds);
                        quantityLeft.text = "" + (currentSeeds);
                    }
                }
                else
                {
                    Debug.Log("Maximo de semillas agarradas");
                }
            }

        }

    }
}