using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WeirdSpices;

public class Coin : Dropable
{
    [SerializeField] private int value = 1;
    [SerializeField] private AudioClip pickUpCoin;
    [SerializeField] private AudioSource audioSource;

    public override void PickedUpBy(Player player)
    {
        base.PickedUpBy(player);
        GameManager.Instance.GainGold(value);
        GameManager.Instance.RemoveToList(this.gameObject);
        audioSource.PlayOneShot(pickUpCoin);
        Destroy(this.gameObject, 0.1f);
    }

}
