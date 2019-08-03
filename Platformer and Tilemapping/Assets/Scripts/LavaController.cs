using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;

public class LavaController : MonoBehaviour
{
    
    public float lavaSpeed;
    public Text instructionsText;
    public PlayerController pcScript;
    void Start()
    {
        GameObject playerControllerObject = GameObject.FindWithTag("Player");
        instructionsText.text ="Collect all the coins before the lava gets you! \n Avoid the birds!";
        StartCoroutine(instructions(3f));
        instructionsText.text = "Collect all the coins before the lava gets you! \n Avoid the birds!";
        //StartCoroutine(instructions(3f));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(gameObject.tag == "Lava")
        {
            transform.position = new Vector3(0.0f,transform.position.y + lavaSpeed,0.0f);
        }
        else
        {
            transform.position = new Vector3(50.0f,transform.position.y + lavaSpeed,0.0f);
        }
        

    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            lavaSpeed = 0;
            pcScript.lossDisplay();
            if(Input.GetKeyDown(KeyCode.R))
            {
                pcScript.restartGame();
            }
        }
    }
    IEnumerator instructions(float time)
    {
         yield return new WaitForSeconds(time);
         instructionsText.text = "";
    }
}
