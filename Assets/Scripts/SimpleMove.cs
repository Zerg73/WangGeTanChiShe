using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class SimpleMove : MonoBehaviour
{

    public float speed;//移动速度
    public float smooth;//转向速度

    public Text bugtext;//测试用

    public Vector3[] fang= {Vector3.up,Vector3.down,Vector3.left,Vector3.right,Vector3.zero };

    public Vector3 direction;//当前移动方向
    Vector2 coordinate;//当前坐标
    Vector3 prePosition;//上一帧的坐标。


    Quaternion rota;//目标朝向
    public Queue<Vector3> directionQ;//输入接收

    Rigidbody2D rb2d;


    bool hasChangeDirection;//判断是否可以改变方向
    HashSet<Vector3> stopSet;

    void Start()
    {
        speed = 10;
        smooth = 20;

        direction = Vector3.zero;
        prePosition = transform.position;
        rota = Quaternion.Euler(0, 0, 0);

        hasChangeDirection = false;

        rb2d = GetComponent<Rigidbody2D>();

        //顺序为上下左右

        stopSet = new HashSet<Vector3>();
        directionQ = new Queue<Vector3>();
        transform.position = PointFix(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
        GetInput();//获取输入
        //Move();//移动
        FMove();
        ToNextPoint();//判断是否到达下一格


        BugText();
    }




    void BugText()
    {
        var str = new StringBuilder();
        str.Clear();
        str.Append("当前方向： "+DtoD(direction)+'\n');
        if(directionQ.Count==0)
        str.Append("下一个方向： 无"+"\n");
        else
        str.Append("下一个方向： "+DtoD(directionQ.Peek())+"\n");
        
        foreach(var t in stopSet)
        {
            str.Append(t.ToString()+'\n');
        }

        bugtext.text = str.ToString();

    }

    void FMove()
    {
        if (IsOverHalf(transform.position, direction))
        {
            if (SetDirection())//设置方向
            {
                transform.position = PointFix(transform.position);
            }
        }//修改这个的逻辑！

        prePosition = transform.position;

        rb2d.velocity = direction * speed;
        transform.rotation = Quaternion.Lerp(transform.rotation, rota, smooth * Time.deltaTime);//转向

    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("碰撞发生");

        transform.position = PointFix(transform.position);
        
        

        foreach (var temp in collision.contacts)
        {
            stopSet.Add(Vector3Int.RoundToInt(Vector3.Normalize(temp.point - new Vector2(transform.position.x, transform.position.y))));
            if (stopSet.Contains(direction))
            {
                direction = fang[4];
            }
        }

    }
    void OnCollisionStay2D(Collision2D collision)
    {
        Debug.Log("持续碰撞");

        foreach (var temp in collision.contacts)
        {
            stopSet.Add(Vector3Int.RoundToInt(Vector3.Normalize(temp.point - new Vector2(transform.position.x, transform.position.y))));
        }
        
        
    }

    void Move()//bug应该在这儿
    {
        if(direction==fang[4])
            transform.position = PointFix(transform.position);

        //判断是否到达可位移点
        if (IsOverHalf(transform.position, direction))
        {
            if (SetDirection())//设置方向
            {
                transform.position = PointFix(transform.position);
            }
        }

        //移动玩家
        
            CheckDirection();
            transform.rotation = Quaternion.Lerp(transform.rotation, rota, smooth*Time.deltaTime);//转向
        
            transform.position += direction * speed * Time.deltaTime;//位移
        
    }
    Vector2 CheckPoint(Vector3 input)//返回目标所在整数节点，例如（0，0）至（1，1）这个矩形的坐标是（0，0）
    {
        return new Vector2(Mathf.FloorToInt(input[0]), Mathf.FloorToInt(input[1]));
    }

    void GetInput()
    {
        if (!stopSet.Contains(fang[0]) && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)))
        {
            directionQ.Enqueue(Vector3.up);
        }

        if (!stopSet.Contains(fang[1]) && (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)))
        {
            directionQ.Enqueue(Vector3.down);
        }

        if (!stopSet.Contains(fang[2]) && (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)))
        {
            directionQ.Enqueue(Vector3.left);
        }

        if (!stopSet.Contains(fang[3]) && (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)))
        {
            directionQ.Enqueue(Vector3.right);
        }
    }
    //如果方向有改变，返回true
    bool SetDirection()
    {
        //如果方向已经改变，那么在到达下一个可位移点之前不可改变方向

        if (directionQ.Count != 0)
        {
            while (directionQ.Count != 0&&directionQ.Peek() == direction)
            {
                directionQ.Dequeue();
            }//移除同向的输入。
           

            if (directionQ.Count != 0 && !hasChangeDirection)
            {
                direction = directionQ.Dequeue();
                SetRotation();
                hasChangeDirection = true;
                return true;
            }
        }
        return false;
    }

    void CheckDirection()
    {
        if (stopSet.Contains(direction))
        {
            direction = fang[4];
        }
    }
    bool ToNextPoint()//判断是否已经到达下一格
    {

        if (coordinate == CheckPoint(transform.position))
        {
            return false;
        }
        else
        {
            coordinate = CheckPoint(transform.position);
            hasChangeDirection = false;
            return true;
        }

    }

    bool IsOverHalf(Vector3 pos,Test.Dire dire)
    {
        switch (dire)
        {
            case Test.Dire.up:
                return pos.y >= CheckPoint(pos).y + 0.5;
            case Test.Dire.down:
                return pos.y <= CheckPoint(pos).y + 0.5;
            case Test.Dire.left:
                return pos.x <= CheckPoint(pos).x + 0.5;
            case Test.Dire.right:
                return pos.x >= CheckPoint(pos).x + 0.5;
            default:
                Debug.Log("IsOverHalf Direction Error.");
                return false;
        }
    }
    bool IsOverHalf(Vector3 pos, Vector3 dire)
    {
        if (dire == fang[0])
            return pos.y >= CheckPoint(pos).y+0.5;
        if (dire == fang[1])
            return pos.y <= CheckPoint(pos).y + 0.5;
        if (dire == fang[2])
            return pos.x <= CheckPoint(pos).x + 0.5;
        if (dire == fang[3])
            return pos.x >= CheckPoint(pos).x + 0.5;

        if (dire == fang[4])
            return true;

        Debug.Log("IsOverHalf Direction Error.");
        return false;

    }


    Test.Dire DtoD(Vector3 dir)
    {
        if (dir == Vector3.up)
            return Test.Dire.up;
        if (dir == Vector3.down)
            return Test.Dire.down;
        if (dir == Vector3.left)
            return Test.Dire.left;
        if (dir == Vector3.right)
            return Test.Dire.right;
        if (dir == Vector3.zero)
            return Test.Dire.empty;
        Debug.Log("Direction to Enum Error.");
        return Test.Dire.empty;
    }
    Vector3 PointFix(Vector3 input)//将坐标位置修正至网格中心点
    {
        var re = new Vector3(Mathf.FloorToInt(input[0])+0.5f, Mathf.FloorToInt(input[1])+0.5f, input[2]);
        return re;
    }
    void SetRotation()//旋转
    {
        if (direction == Vector3.up)
            rota = Quaternion.Euler(0, 0, 0);
        if (direction == Vector3.down)
            rota = Quaternion.Euler(0, 0, 180);
        if (direction == Vector3.left)
            rota = Quaternion.Euler(0, 0, 90f);
        if (direction == Vector3.right)
            rota = Quaternion.Euler(0, 0, 270f);
    }

    //void OnCollisionEnter2D(Collision2D collision)//碰撞的执行顺序在 Update()之前
    //{


    //    Debug.Log("碰撞发生");

    //    transform.position = PointFix(transform.position);

    //    ContactPoint2D con = collision.GetContact(0);
    //    var t = new Vector3(con.point.x, con.point.y, 0) - transform.position;
    //    t = Vector3Int.RoundToInt(t.normalized);
    //    stopSet.Add(t);







    //}
    //void OnCollisionStay2D(Collision2D collision)
    //{
    //    Debug.Log("触发Stay");

    //    ContactPoint2D con = collision.GetContact(0);
    //    var t = new Vector3(con.point.x, con.point.y, 0) - transform.position;
    //    t = Vector3Int.RoundToInt(t.normalized);
    //    stopSet.Add(t);


    //}
    void OnCollisionExit2D(Collision2D collision)
    {
        stopSet.Clear();

        Debug.Log("碰撞结束");

        //Debug.Log(con.point);
    }

}
