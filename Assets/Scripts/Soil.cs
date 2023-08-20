using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace WeirdSpices{
    public class Soil : MonoBehaviour
    {
        #region Parameters
        [Header("Parameters")]
        [SerializeField] private int maxSeeds;
        #endregion Parameters
        
        #region Objects
        [Header("Objects")]
        [SerializeField] private FoodManager foodManager;
        [SerializeField] private GameObject cropPrefab;
        [SerializeField] private Tilemap soil;
        [SerializeField] private Tilemap foresoil;
        [SerializeField] private Tilemap highlights;
        [SerializeField] private Sprite highlight;
        #endregion
       
        private Dictionary<Vector3Int,List<GameObject>> seeds = new Dictionary<Vector3Int, List<GameObject>>();
        private GameObject foodToPlant;
        private GameObject currentCrop;

        private Vector3Int lastHighlightPosition;

        public void PlantSeed(GameObject seed, Vector3 positionToPlant){
            Vector3Int position = Vector3Int.FloorToInt(positionToPlant);
            List<GameObject> seedList = null; 
            if(soil.GetTile(position) != null){
                if(!seeds.TryGetValue(position, out seedList)){
                    seed.SetActive(false);
                    seeds.Add(position, new List<GameObject>(){seed});
                    foresoil.SetTile(position, CreateTile(seed.GetComponent<Seed>().GetSoilSprite()));
                } else if(seeds[position].Count == 1){
                    seed.SetActive(false);
                    seeds[position].Add(seed);
                    foresoil.SetTile(position, null);
                    foodToPlant = foodManager.GetFoodFromSeeds(seeds[position][0],seeds[position][1]);
                    currentCrop = Instantiate(cropPrefab, new Vector2(position.x + 0.5f, position.y +0.5f), Quaternion.identity);
                    currentCrop.GetComponent<Crop>().SetSoilAndFood(foodToPlant,this);
                }
            }
        }

        public void RemoveSeeds(Vector3 seedPosition){
            Vector3Int position = Vector3Int.FloorToInt(seedPosition);
            if(seeds[position] != null){
                foreach(GameObject seed in seeds[position]){
                    Destroy(seed);
                }
                Destroy(seeds[position][1]);
                seeds.Remove(position);
            }
        }

        public void RetrieveSeed(Vector3 seedPosition){
            Vector3Int position = Vector3Int.FloorToInt(seedPosition);
            if(seeds[position] != null){
                seeds[position][0].SetActive(true);
                seeds.Remove(position);
            }
        }

        private Tile CreateTile(Sprite sprite){
            Tile tile = ScriptableObject.CreateInstance<Tile>();
            tile.sprite = sprite;
            return tile;
        }

        public void Highlight(Vector3 newPosition){
            if(lastHighlightPosition != newPosition && soil.GetTile(soil.WorldToCell(newPosition)) != null){
                ClearLastPositionHighlighted();
                lastHighlightPosition = highlights.WorldToCell(newPosition);
                highlights.SetTile(lastHighlightPosition, CreateTile(highlight));
            }
        }

        public void ClearLastPositionHighlighted(){
            highlights.SetTile(lastHighlightPosition, null);
        }

    }
}
