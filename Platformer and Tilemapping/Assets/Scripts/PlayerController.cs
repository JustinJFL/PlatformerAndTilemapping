using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour
{

    private Rigidbody2D rb2d;

    public GameObject player;
    public GameObject playerCamera;
    public float speed;
    public float jumpForce;
    private float scoreCount;
    private float livesCount;
    private float powerUpTimer;

    public Text scoreText;
    public Text winText;
    public Text livesText;
    public Text instructionsText;
    public Transform playerLocation;

    private bool lvl2;
    private bool facingRight = true;
    private bool loss = false;
    private bool gameOver = false;
    private bool powerUp = false;

    public Animator playerAnim;

    public AudioSource victorySound;
    public AudioSource coinSound;
    public AudioSource birdSound;
    public AudioSource lossSound;
    public GameObject startScreen;
    public LavaController lcScript;
    public GameObject canvas;
    public GameObject lvl2Lava;
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        playerAnim = GetComponent<Animator>();
        scoreCount = 0;
        livesCount = 3;
        scoreText.text = "Score: " + scoreCount.ToString();
        livesText.text = "Lives: " + livesCount.ToString();
        winText.text = "";
        instructionsText.text = "";
        lvl2 = false;
        playerLocation.transform.position = new Vector3(-2.22f,.43f,0f);
        //SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName("Level 2"));
        //instructionsText.text = "Collect all the coins before the lava gets you!";
        //StartCoroutine(instructions(3f));
        
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
        else if (rb2d.velocity.y != 0)
        {
            playerAnim.SetBool("Jumping",true);
            playerAnim.SetBool("Walking",false);
        }  

        else if (rb2d.velocity.y == 0)
        {
            playerAnim.SetBool("Jumping",false);
            playerAnim.SetBool("Walking",false);

        }


    }
    void Update()
    {
        //Debug.Log(rb2d.velocity.y);
        if (Input.GetKey("escape"))
        {
         Application.Quit();
        }
        else if (Input.GetKeyDown(KeyCode.R) && gameOver == true)
        {
            /*if(lvl2==true)
            {
                 SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName("Level 2"));
            }
            */
            restartGame();
        }
        else if (rb2d.velocity == Vector2.zero)
        {
            playerAnim.SetBool("Walking",false);
        }
        else if (rb2d.velocity.x > 0 || rb2d.velocity.x < 0)
        {
            playerAnim.SetBool("Walking",true);

        }
        else if (rb2d.velocity.x > -.2 || rb2d.velocity.x < .2)
        {
            playerAnim.SetBool("Walking",false);

        }
        /*if(loss == true)
        {
            float camSize = playerCamera.GetComponent<Camera>().orthographicSize;
            Debug.Log(camSize);
            camSize = Mathf.MoveTowards(camSize, 1.0f,Time.deltaTime * 5);
            Debug.Log(camSize); 
        }*/

    
    }

    public void restartGame()
    {
        scoreCount = 0;
        livesCount = 3;
        scoreText.text = "Score: " + scoreCount.ToString();
        livesText.text = "Lives: " + livesCount.ToString();
        winText.text = "";
        instructionsText.text = "";
        lvl2 = false;
        gameOver = false;
        restorePlayer();
        Debug.Log("Restart successful");
        SceneManager.LoadScene("Level 1", LoadSceneMode.Single);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Level 1"));
    }

    void OnCollisionStay2D(Collision2D Collision)
    {
        if(Collision.collider.tag == "Ground")
        {
            if(Input.GetKey(KeyCode.UpArrow) && loss == false)
            {
                rb2d.AddForce(new Vector2(0,jumpForce),ForceMode2D.Impulse);
            }
            
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Coin")
        {
            Destroy(other.gameObject);
            SetScore();
            coinSound.Play();
        }    

        else if (other.gameObject.tag == "PowerUp")
        {
            powerUp = true;
            powerUpTimer =5.0f;
            instructionsText.text = "You take no damage from enemies for 5 seconds."; 
            StartCoroutine(timerCount());
            StartCoroutine(powerUpDisable(5.5f));
            Destroy(other.gameObject);
        }

        else if(other.gameObject.tag == "Enemy" && powerUp == false)
        {
            SetLives();
            Destroy(other.gameObject);
            birdSound.Play();
        }
        else if(other.gameObject.tag == "Enemy" && powerUp == true)
        {
            Destroy(other.gameObject);
            birdSound.Play();
        }
        else if (other.gameObject.tag == "Lava" || other.gameObject.tag == "Lava2")
        {
            livesCount = -1;
            SetLives();
        }
    }

    void SetScore()
    {
        scoreCount = scoreCount + 1;
        scoreText.text = "Score: " + scoreCount.ToString();
        if (scoreCount ==6)
        {
            livesCount = 3;
            playerAnim.SetBool("Dead",false);
            lcScript.lavaSpeed = 0;
            livesText.text = "Lives: " + livesCount.ToString();
            /*SceneManager.LoadSceneAsync("Level 2",LoadSceneMode.Additive);
            SceneManager.UnloadScene(SceneManager.GetSceneByName("Level 1"));
            movePlayer();*/
            //StartCoroutine(moveDelay(.5f));
            StartCoroutine(moveDelay(2.9f));
            winText.text = "Level 1 Complete.";
            Invoke("disableText", 3f);
            rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
            Invoke("restorePlayer", 3.5f); //restore when level 2 is running
            lvl2 = true;
            lvl2Lava.SetActive(true);

        }
        else if (scoreCount == 14 && lvl2 == true)
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
            playerAnim.SetBool("Dead",true);
            lossDisplay();

        }
    }

    void movePlayer()
    {
        //levelCompleteUI.SetActive(true);
        //playerLocation.position = new Vector2(-2.22f,.43f);
        //SceneManager.LoadSceneAsync("Level 2",LoadSceneMode.Additive);
        /*SceneManager.UnloadScene(SceneManager.GetSceneByName("Level 1"));
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Level 2"));
        SceneManager.MoveGameObjectToScene(playerCamera, SceneManager.GetSceneByName("Level 2"));
        SceneManager.MoveGameObjectToScene(player, SceneManager.GetSceneByName("Level 2"));
        SceneManager.MoveGameObjectToScene(canvas, SceneManager.GetSceneByName("Level 2")); */
        /*DontDestroyOnLoad(canvas);
        DontDestroyOnLoad(player);
        DontDestroyOnLoad(playerCamera); */
        //SceneManager.LoadSceneAsync("Level 2",LoadSceneMode.Additive);
        playerLocation.position = new Vector2(48f,.43f);
        lvl2 = true;
        livesCount = 3;
        
    }

    void winDisplay()
    {
        winText.text = "You Win!";
        scoreText.text = "";
        livesText.text = "";
        instructionsText.text = "";
        powerUpTimer = 0;
        victorySound.Play();
        playerCamera.GetComponent<AudioSource>().volume = 0.0f;
        rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
        Time.timeScale = 0;
        gameOver = true;
    }
    void restorePlayer()
    {
        rb2d.constraints = RigidbodyConstraints2D.None;
        rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
        livesCount = 3;
    }
    public void lossDisplay()
    {
        scoreText.text = "";
        livesText.text = "";
        winText.text = "You Lost!";
        rb2d.constraints = RigidbodyConstraints2D.FreezePositionX;
        //Time.timeScale = 0;
        livesText.text = "Press R to restart";
        loss = true;
        gameOver = true;
        playerCamera.GetComponent<AudioSource>().volume = 0.0f;
        lossSound.Play();
    }
     IEnumerator moveDelay(float time)
    {
         yield return new WaitForSeconds(time);
         //Time.timeScale = 0; //remove after level 2 is restored
         movePlayer();    
    }

    IEnumerator instructions(float time)
    {
         yield return new WaitForSeconds(time);
         instructionsText.text = "";
    }

        IEnumerator powerUpDisable(float time)
    {
         yield return new WaitForSeconds(time);
         powerUp = false;
         instructionsText.text = "";
    }
      IEnumerator timerCount()
  {
    while (powerUpTimer>0) 
    {
        yield return new WaitForSeconds (1);
        powerUpTimer--; 
        instructionsText.text = "You take no damage from enemies for " 
        + powerUpTimer.ToString() 
        + " seconds.";
    }
  }
    void disableText ()
    {
        winText.text = "";
    }
    

    void Flip()
    {
        if(loss == false)
        {
            facingRight = !facingRight;
            Vector2 Scaler = transform.localScale;
            Scaler.x = Scaler.x * -1;
            transform.localScale = Scaler;
        }
    }

}



