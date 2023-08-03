using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WeirdSpices;

public class Coin : MonoBehaviour
{
    public int value = 1;


    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Player"))
        {
            Destroy(this.gameObject);
            GameManager.Instance.GainGold(value);
        }
    }

}
