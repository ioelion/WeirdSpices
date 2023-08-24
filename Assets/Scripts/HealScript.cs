using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WeirdSpices;

public class HealScript : MonoBehaviour
{

    private bool canHeal = true;
    [SerializeField] private int price = 1;
    [SerializeField] private float healCooldown = 1f;
    [SerializeField] private int pointsToHeal = 1;

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            if(canHeal && Input.GetKey(KeyCode.E) )
            {
                if ((GameManager.Instance.totalGold >= price) && (GameManager.Instance.GetHealth() < 10) )
                {
                    GameManager.Instance.Heal(pointsToHeal,price);
                    Debug.Log("Curado");
                    canHeal = false;
                    StartCoroutine(HealCooldown());
                }
                // else ruidito
                
                
            }

        }
    }

    private IEnumerator HealCooldown()
    {
        yield return new WaitForSeconds(healCooldown);
        canHeal = true;
    }

}
