using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float enemySpeed;
    public Vector2 startPos;
    public Vector2 endPos;
    void Start()
    {
    }


    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.Lerp(startPos, endPos, Mathf.PingPong(Time.time * enemySpeed, 1.0f));

    }

}
