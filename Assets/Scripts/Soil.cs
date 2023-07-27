using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
namespace WeirdSpices{
    public class Soil : MonoBehaviour
    {
        [SerializeField]
        private Tile wetTile;

        [SerializeField]
        private Tile dryTile;
        private Tilemap tilemap;

        // Start is called before the first frame update
        void Start()
        {
            tilemap = GetComponent<Tilemap>();
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public void PlantSeed(Vector3 position){
            Vector3Int positionInt = Vector3Int.FloorToInt(position); 
            TileBase tile = this.tilemap.GetTile(positionInt);
            tilemap.SetTile(positionInt,wetTile);
        }

        public void RemoveSeed(Vector3 position){
            Vector3Int positionInt = Vector3Int.FloorToInt(position); 
            TileBase tile = this.tilemap.GetTile(positionInt);
            tilemap.SetTile(positionInt,wetTile);
        } 
    }
}
