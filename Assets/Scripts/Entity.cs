using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeirdSpices{
    public abstract class Entity : MonoBehaviour
    {
        [SerializeField]
        private GameObject weapon;
        public virtual GameObject getWeapon(){
            return weapon;
        }
    }
}