using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations;


public class PlayerController : MonoBehaviour
{

    private Rigidbody2D rb2d;

    public float speed;
    public float jumpForce;
    private float scoreCount;
    private float livesCount;

    public Text scoreText;
    public Text winText;
    public Text livesText;

    public Transform playerLocation;

    private bool lvl2;
    private bool facingRight = true;

    public Animator playerAnim;

    public AudioSource victorySound;
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        playerAnim = GetComponent<Animator>();
        scoreCount = 0;
        livesCount = 3;
        scoreText.text = "Score: " + scoreCount.ToString();
        livesText.text = "Lives: " + livesCount.ToString();
        winText.text = "";
        lvl2 = false;
        
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");

        Vector2 movement = new Vector2(moveHorizontal,0);

        rb2d.AddForce(movement * speed); 

        if (facingRight == false && moveHorizontal > 0)
        {
            Flip();
        }
        else if (facingRight == true && moveHorizontal < 0)
        {
            Flip();
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))
        {
            playerAnim.SetInteger("Walking",1);

            if(Input.GetKey(KeyCode.UpArrow))
            {
                playerAnim.SetInteger("Jumping",1);
            }
        }        
        else if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow))
        {
            playerAnim.SetInteger("Idle",1);
        }


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
            if(Input.GetKey(KeyCode.UpArrow))
            {
                rb2d.AddForce(new Vector2(0,jumpForce),ForceMode2D.Impulse);
                playerAnim.SetInteger("Jumping",1);
            }
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Coin")
        {
            Destroy(other.gameObject);
            SetScore();
        }    

        if(other.gameObject.tag == "Enemy")
        {
            SetLives();
            Destroy(other.gameObject);

        }
    }

    void SetScore()
    {
        scoreCount = scoreCount + 1;
        scoreText.text = "Score: " + scoreCount.ToString();
        if (scoreCount ==4)
        {
            StartCoroutine(moveDelay(3f));
            winText.text = "Level 1 Complete.";
            Invoke("disableText", 3f);
            rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
            Invoke("restorePlayer", 3.5f);

        }
        else if (scoreCount == 8 && lvl2 == true)
        {
            winDisplay();
        }

    }
    
    void SetLives()
    {
        livesCount = livesCount - 1;
        livesText.text = "Lives: " + livesCount.ToString();
        if (livesCount < 1)
        {
            lossDisplay();
        }
    }

    void movePlayer()
    {
        playerLocation.position = new Vector2(39.0f,-2f);
        lvl2 = true;
        livesCount = 3;
    }

    void winDisplay()
    {
        winText.text = "You Win!";
        scoreText.text = "";
        livesText.text = "";
        victorySound.Play();
    }
    void restorePlayer()
    {
        rb2d.constraints = RigidbodyConstraints2D.None;
    }
    void lossDisplay()
    {
        scoreText.text = "";
        livesText.text = "";
        winText.text = "You Lost!";
        rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
    }
     IEnumerator moveDelay(float time)
    {
         yield return new WaitForSeconds(time);
         movePlayer();    
    }
    void disableText ()
    {
        winText.text = "";
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
}

}



