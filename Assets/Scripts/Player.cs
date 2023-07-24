using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private int movementSpeed;

    private Rigidbody2D rb;

    private SpriteRenderer sr;

    [SerializeField]
    private GameObject ingredientContainer;

    private bool hasIngredient = false;

    [SerializeField]
    private float timeToWaitTillGrab = 0.5f;

    private float lastItemDropTime;

    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        sr = this.GetComponent<SpriteRenderer>();
        lastItemDropTime = Time.fixedTime + timeToWaitTillGrab;
    }

    // Update is called once per frame
    void Update()
    {
        float _x = Input.GetAxis("Horizontal") * movementSpeed;
        float _y = Input.GetAxis("Vertical") * movementSpeed;
        Vector2 _force = new Vector2(_x, _y);

        if (_force != Vector2.zero)
        {
            animator.SetBool("playerWalk", true);
            rb.velocity = _force;
            sr.flipX = Mathf.Sign(_force.x) < 0;
        }
        else
        {
            animator.SetBool("playerWalk", false);
            rb.velocity = Vector2.zero;
        }

        if(Input.GetKeyDown(KeyCode.Space) && (Time.fixedTime - lastItemDropTime  > timeToWaitTillGrab)){
            if(hasIngredient){
                Transform tfchildren = ingredientContainer.transform.GetChild(0);
                tfchildren.position = new Vector2(this.transform.position.x, this.transform.position.y);
                ingredientContainer.transform.DetachChildren();
                hasIngredient = false;
                lastItemDropTime = Time.fixedTime; 
            }

        }
        
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(other.tag.Equals("Ingredient") && Input.GetKey(KeyCode.Space) &&!hasIngredient && (Time.fixedTime - lastItemDropTime  > timeToWaitTillGrab))
        {
            other.gameObject.transform.parent = ingredientContainer.transform;
            other.transform.position = new Vector2(ingredientContainer.transform.position.x, ingredientContainer.transform.position.y + 1);
            hasIngredient = true;
        }
    }


}
