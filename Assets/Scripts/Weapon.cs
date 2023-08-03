using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeirdSpices{
    public abstract class Weapon : MonoBehaviour
    {
        [SerializeField] protected int damage;
        private BoxCollider2D bc;
        void Start(){bc = this.GetComponent<BoxCollider2D>();}

        public void FlipPositionX(){bc.offset = new Vector2(bc.offset.x*-1, bc.offset.y);}

        
    }
}
