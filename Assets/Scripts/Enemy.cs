using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeirdSpices{
    public class Enemy : MonoBehaviour
    {
        [SerializeField]
        private Transform target;

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, Time.deltaTime * 10f);
        }
    }

}
