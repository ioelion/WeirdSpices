using System;
using UnityEngine;
namespace WeirdSpices{
    public class Seed : Dropable, IComparable<Seed>
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

        int IComparable<Seed>.CompareTo(Seed other)
        {
            return other.GetSprite() == this.GetSprite() ? 0 :1;
        }
    }
}
