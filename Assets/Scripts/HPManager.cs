using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeirdSpices
{
    public class HPManager : MonoBehaviour
    {
        [SerializeField] private List<HeartUIElement> playerHearts;  
        private int currentPlayerHP;
        private int currentMaxPlayerHP;
        private int maxPlayerHP;
        private int heartQuantity;


        public void SetParameters(int currentPlayerHP, int currentMaxPlayerHP){
            this.maxPlayerHP = playerHearts.Count;
            SetCurrentMaxHP(currentMaxPlayerHP);
            SetCurrentHP(currentPlayerHP);

        }

        public void SetCurrentHP(int newPlayerHP){
            this.currentPlayerHP = newPlayerHP;
            for(int i = 0; i<currentMaxPlayerHP;i++){
                if(currentPlayerHP > i){
                    playerHearts[i].Full() ;
                }else{
                    playerHearts[i].Empty();
                }
            }
        }

        public void SetCurrentMaxHP(int currentMaxPlayerHP){
            this.currentMaxPlayerHP = currentMaxPlayerHP;
            for(int i = 0; i<playerHearts.Count-1;i++){
                if(currentMaxPlayerHP >= i+1){
                    playerHearts[i].gameObject.SetActive(true);
                }else{
                    playerHearts[i].gameObject.SetActive(false);;
                }
            } 
        }

        public void AddOneMoreMaxHP(){
            if(currentMaxPlayerHP+1 <= maxPlayerHP){
                playerHearts[currentMaxPlayerHP].gameObject.SetActive(true);
                this.currentMaxPlayerHP+=1;
            }else{
                Debug.Log("Maxima vida alcanzada");
            }
        }

        public int GetHeartQuantity(){
            return playerHearts.Count;
        }
    }
}
