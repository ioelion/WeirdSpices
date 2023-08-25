using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
namespace WeirdSpices{
    public class Seed : Dropable
    {
        [SerializeField] private int seedNumber;
        [SerializeField] private Sprite soilSprite;
        [SerializeField] private SpriteRenderer spriteRenderer;
        
        public int GetSeedNumber(){
            return seedNumber;
        }

        public Sprite GetSoilSprite(){
            return soilSprite;
        }

        public Sprite GetSprite(){
            return spriteRenderer.sprite;
        }
    }
}
