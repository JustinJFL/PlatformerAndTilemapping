using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Rigidbody2D rb2d;

    public float speed;
    public float jumpForce;
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        Vector2 movement = new Vector2(moveHorizontal,0);

        rb2d.AddForce(movement * speed); 
    }
    void Update()
    {
        if (Input.GetKey("escape"))
        {
         Application.Quit();
        }
    }

    void OnCollisionStay2D(Collision2D Collision)
    {
        if(Collision.collider.tag == "Ground")
        {
            if(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            {
                rb2d.AddForce(new Vector2(0,jumpForce),ForceMode2D.Impulse);
            }
        }
    }
}
