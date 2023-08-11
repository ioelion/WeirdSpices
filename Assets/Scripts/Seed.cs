using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
namespace WeirdSpices{
    public class Seed : Dropable
    {
        [SerializeField] private int seedNumber;
        private SpriteRenderer sr;
        
        void Start()
        {
            sr = GetComponent<SpriteRenderer>();
        }

        public int GetSeedNumber(){
            return seedNumber;
        }
    }
}
