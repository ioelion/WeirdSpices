using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WeirdSpices;
using TMPro;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private int price = 1; 
    [SerializeField] private int quantity = 1; 
    [SerializeField] private int timeToSpawn = 1;
    [SerializeField] private float sellCooldown = 1f;
    [SerializeField] private GameObject objectToSell;
    [SerializeField] private string notEnoughMessage = "Not enough coins";

    [Header("UI Objects")]
    [SerializeField] private TMP_Text priceText;
    [SerializeField] private Image objectToSellImg;
    [SerializeField] private Image priceImg;
    [SerializeField] private TMP_Text notEnoughText;

    [Header("General Objects")]
    [SerializeField] private BoxCollider2D boxCollider2D;
    [SerializeField] private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool canSell = true;
    private int currentQuantity;
    private bool isAnimatingText = false;

    [SerializeField] private AudioClip sound;                //new

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(Waiting());
        objectToSellImg.sprite = objectToSell.GetComponent<SpriteRenderer>().sprite;
        priceText.text = "" + price;
        currentQuantity = quantity;
        notEnoughText.text = "" + notEnoughMessage;
    }

    public void Buy(Vector2 positionToLeaveItem)
    {
        if(canSell){
            if ((GameManager.Instance.currentPlayerGold >= price))
                {
                    GameManager.Instance.LoseGold(price);
                    canSell = false;
                    Instantiate(objectToSell, positionToLeaveItem, Quaternion.identity);
                    StartCoroutine(SellCooldown());
                    currentQuantity -=1;
                    if(currentQuantity == 0){
                        StartCoroutine(Waiting());
                    }

            }else{
                
                if(!isAnimatingText){
                    StartCoroutine(ShowNotEnoughText());
                    AudioManager.Instance.PlaySound(sound);             //new
                }
            }
        }

    }

    private IEnumerator ShowNotEnoughText(){
        isAnimatingText = true;
        notEnoughText.gameObject.SetActive(true);
        notEnoughText.CrossFadeAlpha(0.0f, 0f, false);
        notEnoughText.CrossFadeAlpha(1.0f, 0.25f, false);
        yield return new WaitForSeconds(1);
        StartCoroutine(HideNotEnoughText());
    }
    private IEnumerator HideNotEnoughText(){
        notEnoughText.CrossFadeAlpha(0.0f, 0.25f, false);
        yield return new WaitForSeconds(0.25f);
        notEnoughText.gameObject.SetActive(false);
        isAnimatingText = false;
    }


    private IEnumerator Waiting()
    {
        boxCollider2D.enabled = false;
        animator.SetBool("closed",true);
        yield return new WaitForSeconds(timeToSpawn);
        Selling();
    }
    private void Selling()
    {
        boxCollider2D.enabled = true;
        animator.SetBool("closed",false);
        currentQuantity = quantity;
    }

    private IEnumerator SellCooldown()
    {
        yield return new WaitForSeconds(sellCooldown);
        canSell = true;
    }

}
