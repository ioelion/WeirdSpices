using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeirdSpices{
    public abstract class Dropable : MonoBehaviour
    {
        public float dropChance = 100f;
        [SerializeField] protected BoxCollider2D boxCollider2D;
    
        private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.CompareTag("Player"))
            {
                PickedUpBy(other.gameObject.GetComponent<Player>());
            }
        }

        public virtual void PickedUpBy(Player player){
            
        }
        

    }
}
