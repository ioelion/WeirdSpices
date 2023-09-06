using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeirdSpices
{
    public class Tooltiper : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRendererTooltip;
        [SerializeField] private Sprite correctSprite;
        [SerializeField] private Animator animator;

        private bool showingTooltip = false;


        public void ShowTooltip(string tooltipName){
            if(!UIManager.Instance.IsShowingTooltip(tooltipName)){
                spriteRendererTooltip.gameObject.SetActive(true);
                animator.SetBool(tooltipName, true);
                showingTooltip = true;
                UIManager.Instance.ShowingTooltip(tooltipName);
            }
        }

        public void HideToolTip(string tooltipName){
            if(UIManager.Instance.IsShowingTooltip(tooltipName)){
                spriteRendererTooltip.gameObject.SetActive(false);
                animator.SetBool(tooltipName, false);
                showingTooltip = false;
                UIManager.Instance.NotShowingTooltip(tooltipName);
            }
        }

        public void CompletedTooltip(string tooltipName){
            spriteRendererTooltip.sprite = correctSprite;
            UIManager.Instance.AddCompletedTooltip(tooltipName);
            animator.SetTrigger("completed");
            animator.SetBool(tooltipName, false);
            StartCoroutine(HideTooltip());
        }

        private IEnumerator HideTooltip(){
            yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0)[0].clip.length);
            spriteRendererTooltip.gameObject.SetActive(false);
            showingTooltip = false;
            UIManager.Instance.ShowingTooltip("");
        }

        public bool AlreadyDoneTooltip(string tooltipName){
            return UIManager.Instance.AlreadyDoneTooltip(tooltipName);
        }

        public bool ShowingTooltip(){
            return showingTooltip;
        }
    }
}
