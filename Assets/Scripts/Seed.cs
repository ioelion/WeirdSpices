using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
namespace WeirdSpices{
    public class Seed : MonoBehaviour
    {
        [SerializeField] public int seedNumber {get; private set;}
        private SpriteRenderer sr;
        
        void Start()
        {
            sr = GetComponent<SpriteRenderer>();
        }
    }
}
