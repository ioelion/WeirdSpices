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

        [SerializeField]
        private GameManager gameManager;
        
        private Dictionary<Vector3, GameObject> desactivatedSeeds;

        void Start()
        {
            desactivatedSeeds = new Dictionary<Vector3, GameObject>();
        }

        public void IrrigateSoil(Vector3 position){
            Vector3Int positionInt = Vector3Int.FloorToInt(position); 
            if(this.soil.GetSprite(positionInt) != null){
                soil.SetTile(positionInt,wetTile);
            }

        }

        public void PlantSeed(Vector3 position, GameObject seed){
            Vector3Int positionInt = soil.WorldToCell(Vector3Int.FloorToInt(position)); 
            if(soil.GetSprite(positionInt) != null && foresoil.GetSprite(positionInt) == null){
                Tile seedTile = ScriptableObject.CreateInstance<Tile>();
                seedTile.sprite = seed.GetComponent<SpriteRenderer>().sprite;
                foresoil.SetTile(positionInt,seedTile);
                Debug.Log("Se guardo en: " + positionInt);
                desactivatedSeeds.Add(positionInt, seed);
                seed.SetActive(false);
            }
        }

        public void RemoveSeed(Vector3 position){
            Vector3Int positionInt = soil.WorldToCell(Vector3Int.FloorToInt(position));
            if(soil.GetSprite(positionInt) != null && foresoil.GetSprite(positionInt) != null){
                foresoil.SetTile(positionInt,null);
                GameObject removedSeed = null;
                Debug.Log("Removiendo en: " + positionInt);
                bool hasItBeenRemoved = desactivatedSeeds.TryGetValue(positionInt,out removedSeed);
                if(hasItBeenRemoved){
                    removedSeed.SetActive(true);
                    desactivatedSeeds.Remove(positionInt);
                }
            }
        }
    }
}
