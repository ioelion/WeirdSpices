using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WeirdSpices;

public class Coin : Dropable
{
    [SerializeField] private int value = 1;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Player"))
        {
            Destroy(this.gameObject);
            GameManager.Instance.GainGold(value);
            GameManager.Instance.RemoveToList(this.gameObject);
        }
    }

}
