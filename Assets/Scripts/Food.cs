using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeirdSpices{
    public class Food : Dropable
    {
        [SerializeField] private float timeNeededToGrow;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private List<Seed> seedsNeeded;


        private void Start()
        {
            seedsNeeded.Sort();
        }

        //de ser necesario se puede crear el cropPrefab para cada morfi y asignarlo aca
        public float GetTimeNeededToGrow(){
            return this.timeNeededToGrow;
        }  

        public Sprite GetSprite(){
            return spriteRenderer.sprite;
        }

        public List<Seed> GetSeedsNeeded(){
            return this.seedsNeeded;
        }

    }
}