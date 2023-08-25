using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeirdSpices{
    public class FoodManager : MonoBehaviour
    {
        [SerializeField] Food[] foods;
        Dictionary<string,Food> recipes;
        [SerializeField] Food badFood;
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
            recipes = new Dictionary<string, Food>();
            foreach(Food food in foods){
                List<Seed>  seeds = food.GetSeedsNeeded();
                List<int> numbers = new List<int>();
                foreach(Seed seed in seeds){
                    numbers.Add(seed.GetSeedNumber());
                }
                numbers.Sort();
                string code = string.Join(" ", numbers);
                Debug.Log(code);
                recipes.Add(code, food);
            }

        }

        public Food GetFoodFromSeeds(List<Seed> seeds){
            List<int> numbers = new List<int>();
            foreach(Seed seed in seeds){
                numbers.Add(seed.GetSeedNumber());
            }
            numbers.Sort();
            string code = string.Join(" ", numbers);
            Food food = null;
            recipes.TryGetValue(code, out food);
            if(food == null){
                return badFood;
            }
            return food;
        }

        public Food GetRandomFood(){
            return foods[Random.Range(0, foods.Length)];
        }
    }
}