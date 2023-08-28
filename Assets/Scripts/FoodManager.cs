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
            foreach(Food food in foods) recipes.Add(GetFoodCode(food.GetSeedsNeeded()), food);
        }

        public Food GetFoodFromSeeds(List<Seed> seeds){
            Food food = null;
            recipes.TryGetValue(GetFoodCode(seeds), out food);
            return food;
        }

        public Food GetRandomFood(){
            return foods[Random.Range(0, foods.Length)];
        }

        private string GetFoodCode(List<Seed> seeds){
            List<int> numbers = new List<int>();
            foreach(Seed seed in seeds) numbers.Add(seed.GetSeedNumber());
            numbers.Sort();
            return string.Join(" ", numbers);
        }

        public Food GetBadFood(){
            return badFood;
        }
    }
}