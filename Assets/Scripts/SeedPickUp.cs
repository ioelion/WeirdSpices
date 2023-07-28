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
    int itemCount;
    
    private void OnCollisionStay2D(Collision2D other)
    {
        if ((other.collider.GetComponent<Player>() != null) && (itemPicked == false))
        {

            if (Input.GetKeyDown(KeyCode.E))  // SI se usa GetKey te dropea todas las semillas a la vez, si se usa GetKeyDown a veces no funciona (LUCA)//
            {
                Debug.Log("Semilla dropeada");
                soltarSemilla();
            }
        }
    }

    private void soltarSemilla()
    {
        if (itemCount < itemAmount)
        { 
            Instantiate(Seed, Dispenser.position, Dispenser.rotation);
            

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
