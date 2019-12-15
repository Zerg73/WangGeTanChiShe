using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Peng : MonoBehaviour
{
    List<Quaternion> ro;



    [HideInInspector]
    public bool gameover;
    [HideInInspector]
    public bool stop;
    public float smooth;
    public float speed;
    GameManager gm;

    public Vector2 direction;


    Rigidbody2D rb;
    void Awake()
    {
        gameover = false;
        stop = false;
        direction = Vector2.zero;
        ro = new List<Quaternion>();
        ro.Add(Quaternion.Euler(0, 0, 0));
        ro.Add(Quaternion.Euler(0, 0, 90));
        ro.Add(Quaternion.Euler(0, 0, 180));
        ro.Add(Quaternion.Euler(0, 0, 270));
        rb = GetComponent<Rigidbody2D>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();

        speed = gm.speed;
        smooth = gm.smooth;

    }


    void Update()
    {
        Rotate();
        Move();
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            Debug.Log(gameObject.name + " Over.");
            gameover = true;
            direction = Vector2.zero;
        }

        if (collision.gameObject.tag == "Player")
        {
            Debug.Log(gameObject.name + " Stop.");
            stop = true;
            direction = Vector2.zero;
        }
    }
    void Rotate()
    {
        if (direction == Vector2.up)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, ro[0], smooth * Time.deltaTime);
        }
        if (direction == Vector2.left)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, ro[1], smooth * Time.deltaTime);
        }
        if (direction == Vector2.down)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, ro[2], smooth * Time.deltaTime);
        }
        if (direction == Vector2.right)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, ro[3], smooth * Time.deltaTime);
        }
    }
    void Move()
    {
        rb.velocity = direction * speed;
    }

    public bool CanRotate()
    {
        if (direction == Vector2.zero)
        {
            return true;
        }
        if (direction == Vector2.up)
        {
            if (transform.position.y >= CheckPoint(transform.position).y + 0.5f)
            {
                return true;
            }
        }
        if (direction == Vector2.left)
        {
            if (transform.position.x <= CheckPoint(transform.position).x + 0.5f)
            {
                return true;
            }
        }
        if (direction == Vector2.down)
        {
            if (transform.position.y <= CheckPoint(transform.position).y + 0.5f)
            {
                return true;
            }
        }
        if (direction == Vector2.right)
        {
            if (transform.position.x >= CheckPoint(transform.position).x + 0.5f)
            {
                return true;
            }
        }

        return false;
    }



    Vector2 CheckPoint(Vector3 input)//返回目标所在整数节点，例如（0，0）至（1，1）这个矩形的坐标是（0，0）
    {
        return Vector2Int.FloorToInt(input);
    }

}
