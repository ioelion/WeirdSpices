using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
namespace WeirdSpices{
    public class Seed : MonoBehaviour
    {
        private SpriteRenderer sr;
        // Start is called before the first frame update
        void Start()
        {
            sr = GetComponent<SpriteRenderer>();
        }

        public string getSpriteName(){
            return sr.sprite.name;
        }
    }
}