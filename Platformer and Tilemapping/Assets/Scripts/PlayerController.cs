using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    private Rigidbody2D rb2d;

    public float speed;
    public float jumpForce;
    private float scoreCount;
    public Text scoreText;
    public Text winText;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        scoreCount = 0;
        scoreText.text = "Score: " + scoreCount.ToString();
        winText.text = "";
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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Coin")
        {
            SetScore();
            Destroy(other.gameObject);
            
        }    
    }

    void SetScore()
    {
        scoreCount = scoreCount + 1;
        scoreText.text = "Score: " + scoreCount.ToString();
        if (scoreCount >= 4)
        {
            winText.text = "You Win!";
            scoreText.text = "";
        }
    }
}
