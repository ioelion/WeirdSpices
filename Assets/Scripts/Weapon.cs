using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeirdSpices{
    public class Weapon : MonoBehaviour
    {
        [SerializeField]
        private int damage;

        [SerializeField]
        private List<Vector2> positions;
        private bool isFlipped = false;

        private BoxCollider2D bc;
        // Start is called before the first frame update
        void Start()
        {
            bc = this.GetComponent<BoxCollider2D>();
        }

        void Awake()
        {
            bc = this.GetComponent<BoxCollider2D>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnCollisionEnter2D(Collision2D other)
        {
            if(other.gameObject.CompareTag("Enemy") ){
                Enemy enemy = other.gameObject.GetComponent<Enemy>();
                enemy.ReduceHealth(damage);
            }else if(other.gameObject.CompareTag("Player")){
                Player player = other.gameObject.GetComponent<Player>();
                player.ReduceHealth(damage);
            }
        }

        public void FlipPositionX(){
            bc.offset = new Vector2(bc.offset.x*-1, bc.offset.y);
            if(isFlipped){
                isFlipped = false;
            }else{
                isFlipped = true;
            }
        }

        public bool IsFlipped(){
            return isFlipped;
        }
    }
}
