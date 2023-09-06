using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WeirdSpices;

public class Coin : Dropable
{
    [SerializeField] private int value = 1;

    public override void PickedUpBy(Player player)
    {
        base.PickedUpBy(player);
        GameManager.Instance.GainGold(value);
        GameManager.Instance.RemoveToList(this.gameObject);
        Destroy(this.gameObject, 0.1f);
    }

}
