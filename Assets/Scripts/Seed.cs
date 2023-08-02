using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
namespace WeirdSpices{
    public class Seed : MonoBehaviour
    {
        [SerializeField] int seedNumber;
        SpriteRenderer sr;
        
        void Start()
        {
            sr = GetComponent<SpriteRenderer>();
        }

        public int GetSeedNumber(){
            return seedNumber;
        }

        public void SetSeedNumber(int seedNumber){
            this.seedNumber = seedNumber;
        }
    }
}
