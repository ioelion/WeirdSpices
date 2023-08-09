using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeirdSpices{
    public class Food : MonoBehaviour
    {
        [SerializeField] private float timeNeededToGrow;
        [SerializeField] private SpriteRenderer spriteRenderer;
        //de ser necesario se puede crear el cropPrefab para cada morfi y asignarlo aca
        public float getTimeNeededToGrow(){
            return this.timeNeededToGrow;
        }  

        public SpriteRenderer GetSpriteRenderer(){
            return this.spriteRenderer;
        }
    }
}