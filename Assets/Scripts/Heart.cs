using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeirdSpices{
    public class Heart : MonoBehaviour
    {
        [SerializeField] private int addHP = 1;
        
        public int GetHP(){
            return addHP;
        }
        public void Destroy(){
            Destroy(gameObject, 0.1f);
        }
    }
}

