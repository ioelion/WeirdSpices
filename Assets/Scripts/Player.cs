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
            sr.flipX = Mathf.Sign(_force.x) < 0;
        }
        else
        {
            //manager.animator.SetBool("walking", false);
            rb.velocity = Vector2.zero;
        }
    }
}
