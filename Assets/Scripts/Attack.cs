using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeirdSpices{
    public class Attack : StateMachineBehaviour
    {
        private GameObject weapon;
        private Entity entity;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            entity = animator.GetComponentInParent<Entity>();
            weapon = entity.getWeapon().gameObject;
        }
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            weapon.SetActive(false);
        }
    }
}
