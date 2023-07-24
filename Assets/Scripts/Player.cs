using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private int movementSpeed;

    private Rigidbody2D rb;

    private SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        sr = this.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float _x = Input.GetAxis("Horizontal") * movementSpeed;
        float _y = Input.GetAxis("Vertical") * movementSpeed;
        Vector2 _force = new Vector2(_x, _y);

        if (_force != Vector2.zero)
        {
            //movementSpeed.animator.SetBool("walking", true);
            rb.velocity = _force;
            //TODO yoelpedemonte cambiar sprite personaje por uno de costado
            sr.flipX = Mathf.Sign(_force.x) < 0;
        }
        else
        {
            //manager.animator.SetBool("walking", false);
            rb.velocity = Vector2.zero;
        }

        if(Input.GetKey(KeyCode.Space)){
            //TODO yoelpedemonte objeto a donde se adhieran para evitar problemas con el detach children 
            Transform tfchildren = this.transform.GetChild(0);
            tfchildren.position = new Vector2(this.transform.position.x + 2, this.transform.position.y + 2);
            this.transform.DetachChildren();
        }
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag.Equals("Ingredient"))
        {
            //TODO yoelpedemonte agregar limite de hijos (semillas)
            other.gameObject.transform.parent = this.transform;
            other.transform.position = new Vector2(this.transform.position.x, this.transform.position.y + 1);

        }
    }


}
