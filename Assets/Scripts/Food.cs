using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeirdSpices{
    public class Food : Dropable
    {
        [SerializeField] private float timeNeededToGrow;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private List<Seed> seedsNeeded;
        //de ser necesario se puede crear el cropPrefab para cada morfi y asignarlo aca
        public float getTimeNeededToGrow(){
            return this.timeNeededToGrow;
        }  

        public SpriteRenderer GetSpriteRenderer(){
            return this.spriteRenderer;
        }

        public List<Seed> GetSeedsNeeded(){
            return this.seedsNeeded;
        }

    }
}