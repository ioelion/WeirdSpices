using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeirdSpices{
    public class FoodManager : MonoBehaviour
    {
        [SerializeField] GameObject[] foodList;

        [SerializeField] GameObject[] seedList;

        Dictionary<string, GameObject> recipes;

        public static FoodManager Instance { get; private set; }    

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            } else {
                Debug.Log("Más de un FoodManager en escena.");
            }
        }
 
        void Start()
        {
            //TODO rehacer sistema de recetas
            recipes = new Dictionary<string, GameObject>();
            recipes.Add("0-0", foodList[0]);
            recipes.Add("0-1", foodList[1]);
            recipes.Add("0-2", foodList[2]);
            recipes.Add("1-0", foodList[3]);
            recipes.Add("1-1", foodList[4]);
            recipes.Add("1-2", foodList[5]);
            recipes.Add("2-0", foodList[6]);
            recipes.Add("2-1", foodList[7]);
            recipes.Add("2-2", foodList[8]);
        }

        public GameObject GetFoodFromSeeds(GameObject seed1, GameObject seed2){
            int seedNumber1 = seed1.GetComponent<Seed>().GetSeedNumber();
            int seedNumber2 = seed2.GetComponent<Seed>().GetSeedNumber();
            return recipes[seedNumber1+"-"+seedNumber2];
        }

        public GameObject GetRandomFood(){
            return recipes[Random.Range(0,3)+"-"+Random.Range(0,3)];
        }
    }
}