using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;


public class EnemyController : MonoBehaviour
{
    public float enemySpeed;
    public Vector3 startPos;
    public Vector3 endPos;
    public Transform birdPos;

    public Rigidbody2D rb;
    public SpriteRenderer birdSpr;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        transform.position = Vector2.Lerp(
        startPos, 
        endPos, 
        Mathf.PingPong((Time.time * enemySpeed), 1.0f));
        //rb.velocity = new Vector2 (enemySpeed, 0);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.gameObject.tag == "Positional")
        {
            Debug.Log("collided with object");
            Debug.Log(birdPos.rotation.y);
            if(birdPos.rotation.y >=0)
            {birdPos.rotation = Quaternion.Euler(0,180,0);}
            else if (birdPos.rotation.y <0)
            {birdPos.rotation = Quaternion.Euler(0,0,0);}
            //birdSpr.flipX = !birdSpr.flipX;
            //transform.rotation = Quaternion.Euler(0,transform.rotation.y + 180.0f,0);
        }
    }
}

