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
        
        private Dictionary<Vector3, GameObject> desactivatedSeeds;

        private Dictionary<Vector3, List<GameObject>> growingSeeds;

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
            if(soil.GetSprite(positionInt) != null)
            {
                if(foresoil.GetSprite(positionInt) == null){
                    Tile seedTile = ScriptableObject.CreateInstance<Tile>();
                    seedTile.sprite = seed.GetComponent<SpriteRenderer>().sprite;
                    foresoil.SetTile(positionInt,seedTile);
                    Debug.Log("Se guardo en: " + positionInt);
                    desactivatedSeeds.Add(positionInt, seed);
                    seed.SetActive(false);
                }else{
                    foresoil.SetTile(positionInt,null);
                    seed.SetActive(false);
                    GameObject removedSeed = null;
                    bool valueExists = desactivatedSeeds.TryGetValue(positionInt,out removedSeed);
                    if(valueExists){
                        desactivatedSeeds.Remove(positionInt);
                        List<GameObject> seedsToGrow = new List<GameObject>();
                        seedsToGrow.Add(removedSeed);
                        seedsToGrow.Add(seed);
                        growingSeeds.Add(positionInt, seedsToGrow);
                    }
                    // obtener del FoodManager el obj que se debe generar de plantar estas dos semillas
                    // reemplazar tile del foresoil por semillas plantadas foresoil.SetTile(positionInt, plantedTile);
                    // 
                }
            }
            
        }

        public void RemoveSeed(Vector3 position){
            Vector3Int positionInt = soil.WorldToCell(Vector3Int.FloorToInt(position));
            if(soil.GetSprite(positionInt) != null && foresoil.GetSprite(positionInt) != null){
                foresoil.SetTile(positionInt,null);
                GameObject removedSeed = null;
                Debug.Log("Removiendo en: " + positionInt);
                bool valueExists = desactivatedSeeds.TryGetValue(positionInt,out removedSeed);
                if(valueExists){
                    removedSeed.SetActive(true);
                    desactivatedSeeds.Remove(positionInt);
                }
            }
        }

        public bool IsOnSoil(Vector3 position){
            return soil.GetSprite(soil.WorldToCell(position)) != null ? true : false;
        }
    }
}
