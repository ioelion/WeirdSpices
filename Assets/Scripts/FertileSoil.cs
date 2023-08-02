using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace WeirdSpices{
    public class FertileSoil : MonoBehaviour
    {
        [SerializeField] int maxSeeds;
        [SerializeField] Sprite notplantedSoil;
        [SerializeField] Sprite plantedSoil;
        [SerializeField] Sprite growingSoil;
        [SerializeField] FoodManager foodManager;
        [SerializeField] Tilemap soil;
        [SerializeField] Tilemap foresoil;
        List<GameObject> seeds;
        GameObject food;
        float timeGrowStarted;
        [SerializeField] float timeToWaitFullGrowth = 5;
        
        void Start()
        {
            seeds = new List<GameObject>();
        }

        void Update(){
            if(food != null && (Time.fixedTime - timeGrowStarted  > timeToWaitFullGrowth)){
                Instantiate(food, this.transform.position, Quaternion.identity);
                Vector3Int position = Vector3Int.FloorToInt(this.transform.position);
                soil.SetTile(position,CreateTile(notplantedSoil));
                food = null;
                seeds.Clear();
            }
        }

        public void PlantSeed(GameObject seed){
            Vector3Int position = Vector3Int.FloorToInt(this.transform.position);
            switch(seeds.Count){
                case 0:
                    seed.SetActive(false);
                    seeds.Add(seed);
                    soil.SetTile(position,CreateTile(plantedSoil));
                    foresoil.SetTile(position, CreateTile(seed.GetComponent<SpriteRenderer>().sprite));
                    break;
                case 1:
                    seed.SetActive(false);
                    seeds.Add(seed);
                    soil.SetTile(position,CreateTile(growingSoil));
                    foresoil.SetTile(position, null);
                    food = foodManager.GetFoodFromSeeds(seeds[0],seeds[1]);
                    timeGrowStarted = Time.fixedTime;
                    break;
                default:
                    break;
            }       
        }

        public void RemoveSeed(){
            if(seeds.Count == 1){
                seeds[0].SetActive(true);
                seeds.Clear();
                Vector3Int position = Vector3Int.FloorToInt(this.transform.position);
                soil.SetTile(position,CreateTile(notplantedSoil));
            }
        }

        private Tile CreateTile(Sprite sprite){
            Tile tile = ScriptableObject.CreateInstance<Tile>();
            tile.sprite = sprite;
            return tile;
        }
    }
}