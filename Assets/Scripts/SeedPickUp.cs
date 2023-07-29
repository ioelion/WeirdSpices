using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WeirdSpices;

public class SeedPickUp : MonoBehaviour
{

    [SerializeField] private Transform Dispenser;

    [SerializeField] private GameObject Seed;

    bool itemPicked = false;

    public int itemAmount;
    private int itemCount;
                
    [SerializeField] private float timeToWaitTillSeedDrop = 1.5f;
    private float lastSeedDropTime = 0f;
    
    public void DropSeed()
    {
        if((Time.fixedTime - lastSeedDropTime  > timeToWaitTillSeedDrop)){
            lastSeedDropTime = Time.fixedTime;
            if (itemCount < itemAmount && !itemPicked)
            { 
                Instantiate(Seed, Dispenser.position, Dispenser.rotation);
                Debug.Log("Semilla dropeada");

                itemCount++;
                Debug.Log(itemCount);
            }
            else
            {
                itemPicked = true;
                Debug.Log("Maximo de semillas agarradas");
            }
        }

    }

}
