using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace WeirdSpices{
    public class SeedPickUp : MonoBehaviour
    {

        [SerializeField] private Transform Dispenser;

        [SerializeField] private GameObject Seed;

        [SerializeField] private TMP_Text quantityLeft;

        bool itemPicked = false;

        public int maxItemAmount;
        private int itemCount;
                    
        [SerializeField] private float timeToWaitTillSeedDrop = 1.5f;
        private float lastSeedDropTime = 0f;
        
        void Start()
        {
            quantityLeft.text = "" + maxItemAmount;
        }


        public void DropSeed()
        {
            if((Time.fixedTime - lastSeedDropTime  > timeToWaitTillSeedDrop)){
                lastSeedDropTime = Time.fixedTime;
                if (itemCount < maxItemAmount && !itemPicked)
                { 
                    Instantiate(Seed, Dispenser.position, Dispenser.rotation);
                    Debug.Log("Semilla dropeada");

                    itemCount++;
                    Debug.Log(itemCount);
                    quantityLeft.text = "" + (maxItemAmount - itemCount);
                }
                else
                {
                    itemPicked = true;
                    Debug.Log("Maximo de semillas agarradas");
                }
            }

        }

    }
}