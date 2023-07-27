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

        [SerializeField]
        private Tilemap soil;

        [SerializeField]
        private Tilemap foresoil;

        public void IrrigateSoil(Vector3 position){
            Vector3Int positionInt = Vector3Int.FloorToInt(position); 
            if(this.soil.GetSprite(positionInt) != null){
                soil.SetTile(positionInt,wetTile);
            }

        }

        public void ManageSeed(Vector3 position, GameObject seed){
            Vector3Int positionInt = Vector3Int.FloorToInt(position); 
            if(soil.GetSprite(positionInt) != null && foresoil.GetSprite(positionInt) == null){
                Tile seedTile = seed.gameObject.GetComponent<Seed>().getTile();
                foresoil.SetTile(positionInt,seedTile);
            };
        }
        /*
        public void RemoveSeed(Vector3 position){
            Vector3Int positionInt = Vector3Int.FloorToInt(position); 
            TileBase tile = this.tilemap.GetTile(positionInt);
            tilemap.SetTile(positionInt,wetTile);
        }*/ 
    }
}
