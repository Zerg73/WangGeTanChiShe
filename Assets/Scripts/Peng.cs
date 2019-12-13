using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Peng : MonoBehaviour
{
    [HideInInspector]
    public bool gameover;
    [HideInInspector]
    public bool stop;


    Rigidbody2D rb;
    void Awake()
    {
        gameover = false;
        stop = false;
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag=="Wall")
        {
            Debug.Log(gameObject.name+" Over.");
            gameover = true;
            rb.velocity = Vector2.zero;
        }

        if (collision.gameObject.tag == "Player")
        {
            Debug.Log(gameObject.name+" Stop.");
            stop = true;
            rb.velocity = Vector2.zero;
        }
    }

}
