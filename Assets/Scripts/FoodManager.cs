using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeirdSpices{
    public class FoodManager : MonoBehaviour
    {
        [SerializeField] GameObject[] foodList;

        [SerializeField] GameObject[] seedList;

        Dictionary<string, GameObject> recipes;
 
        void Start()
        {
           /* for(int i = 0; i < seedList.Length; i++){
                seedList[i].GetComponent<Seed>().SetSeedNumber(i+1);
            }*/
            recipes = new Dictionary<string, GameObject>();
            recipes.Add("I0-0", foodList[0]);
            recipes.Add("I0-1", foodList[1]);
            recipes.Add("I0-2", foodList[2]);
            recipes.Add("I1-0", foodList[3]);
            recipes.Add("I1-1", foodList[4]);
            recipes.Add("I1-2", foodList[5]);
            recipes.Add("I2-0", foodList[6]);
            recipes.Add("I2-1", foodList[7]);
            recipes.Add("I2-2", foodList[8]);
        }

        public GameObject GetFoodFromSeeds(GameObject seed1, GameObject seed2){
            int seedNumber1 = seed1.GetComponent<Seed>().GetSeedNumber();
            int seedNumber2 = seed2.GetComponent<Seed>().GetSeedNumber();
            return recipes["I"+seedNumber1+"-"+seedNumber2];

        }
    }
}