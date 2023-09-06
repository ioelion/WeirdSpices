using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeirdSpices{
    public class Heart : Dropable
    {
        [SerializeField] private int hp = 1;
    

        public override void PickedUpBy(Player player)
        {
            player.RecoverHP(hp);
            base.PickedUpBy(player);
            Destroy(this.gameObject, 0.1f);
        }

    }
}

