using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WeirdSpices;

public class HealScript : MonoBehaviour
{

    private bool canHeal = true;
    [SerializeField] private int price = 1; 

    private bool canUse;
    [SerializeField] private int timeToSpawn = 1;
    [SerializeField] private int timeSpawned = 2;

    [SerializeField] private float healCooldown = 1f;
    [SerializeField] private int pointsToHeal = 1;

    [SerializeField] private Sprite spriteHealth;
    [SerializeField] private Sprite spriteNothing;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = spriteHealth;
        StartCoroutine(Waiting());
    }

    private void Update()
    {
        if (canUse)
        {
            spriteRenderer.sprite = spriteHealth;
        }
        else
        {
            spriteRenderer.sprite = spriteNothing;
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (canUse)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                if (canHeal && Input.GetKey(KeyCode.E))
                {
                    if ((GameManager.Instance.totalGold >= price) && (GameManager.Instance.GetHealth() < 10))
                    {
                        GameManager.Instance.Heal(pointsToHeal, price);
                        Debug.Log("Curado");
                        canHeal = false;
                        StartCoroutine(HealCooldown());
                    }
                    // else ruidito


                }

            }
        }
        // else ruidito
        
    }

    private IEnumerator Waiting()
    {
        Debug.Log("Esperando");
        canUse = false;
        yield return new WaitForSeconds(timeToSpawn);
        StartCoroutine(Selling());
    }
        private IEnumerator Selling()
    {
        Debug.Log("Mostrar");
        canUse = true;  
        yield return new WaitForSeconds(timeSpawned);
        StartCoroutine(Waiting());
    }

    private IEnumerator HealCooldown()
    {
        yield return new WaitForSeconds(healCooldown);
        canHeal = true;
    }

}
