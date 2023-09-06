using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WeirdSpices;

public class Coin : Dropable
{
    [SerializeField] private int value = 1;
    [SerializeField] private AudioClip sound;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Player"))
        {
            AudioManager.Instance.PlaySound(sound);                     //new
            Destroy(this.gameObject);
            GameManager.Instance.GainGold(value);
            GameManager.Instance.RemoveToList(this.gameObject);
        }
    }

}
