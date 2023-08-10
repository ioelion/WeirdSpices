using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeirdSpices{
    public abstract class Weapon : MonoBehaviour
    {
        [SerializeField] protected int damage;
        private BoxCollider2D bc;
        private Vector2 original;
        private Vector2 negated;
        void Awake(){
            bc = this.GetComponent<BoxCollider2D>();
            original = new Vector2(this.transform.localPosition.x, transform.localPosition.y);
            negated = new Vector2(original.x *-1, original.y);
            }

        public void FlipPositionX(bool flipPosition){
            if(flipPosition){
                transform.localPosition = negated;
            }else{
                transform.localPosition = original;
            }
        }

        
    }
}
