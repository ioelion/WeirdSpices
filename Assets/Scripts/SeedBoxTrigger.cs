using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeirdSpices{
    public class SeedBoxTrigger : Dropable
    {
        [SerializeField] int seedboxNumber;
        private TutorialManager tutorialManager;
        private Vector3 currentVelocity = Vector3.zero;
        private Vector3 seedBoxPosition; 
        private bool pickedUp = false;
        void Start()
        {
            tutorialManager = TutorialManager.Instance;
            seedBoxPosition = tutorialManager.GetSeedBoxPosition(seedboxNumber);

        }
        public override void PickedUpBy(Player player)
        {
            pickedUp = true;
            tutorialManager.SetSeedBoxState(true, seedboxNumber);
        }

        private void FixedUpdate()
        {
            if(pickedUp){
                transform.position = Vector3.SmoothDamp(transform.position,seedBoxPosition,ref currentVelocity,0.4f);
                if(IsNearTo(transform.position, seedBoxPosition)){
                    Destroy(this.gameObject);
                }
            }
        }

        private bool IsNearTo(Vector2 v1, Vector2 v2){
            return ((v1.x - v2.x) < 0.05f && (v1.x - v2.x) > -0.05f) && ((v1.y - v2.y) < 0.05f) && (v1.y - v2.y) > -0.05f;
        }
    }
}

