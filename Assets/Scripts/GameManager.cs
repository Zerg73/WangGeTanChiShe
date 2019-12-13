using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    GameObject wallPrefab;//墙体预制体
    GameObject linePrefab;//线条预制体
    
    GameObject env;//环境物体的父节点
    GameObject lines;//线条的父节点
    Text infoText;
    GameObject player, follower;//P1和P2
    Transform pt, ft;//玩家的 Transform 组件
    Peng pp, fp;//玩家的 Peng 组件
    Rigidbody2D prb, frb;//玩家的刚体组件
    public Vector2 pd, fd;//玩家移动方向

    float speed,smooth;//速度以及转向速度

    bool isReversed;//控制反向

    Vector3 cornerPos;//保存屏幕右上角坐标，用于计算坐标和像素点之间的关系
    Dictionary<Vector3, GameObject> lineMap;//用于存储线条以及其对应位置的字典
    Queue<Vector2> directionQ;//输入接收
    Vector2 nextDirection;


    void Awake()
    {
        linePrefab = Resources.Load<GameObject>("Prefabs/Line");
        wallPrefab = Resources.Load<GameObject>("Prefabs/Wall");

        linePrefab.GetComponent<LineRenderer>().material = Resources.Load<Material>("Materials/Red");
        linePrefab.GetComponent<LineRenderer>().startWidth = 0.3f;
        linePrefab.GetComponent<LineRenderer>().endWidth = 0.3f;
        linePrefab.GetComponent<LineRenderer>().startColor=Color.red;
        linePrefab.GetComponent<LineRenderer>().endColor=Color.red;

        infoText = GameObject.Find("InfoText").GetComponent<Text>();
        env = GameObject.Find("Env");
        lines = GameObject.Find("Lines");
        player = GameObject.Find("Player");
        follower = GameObject.Find("Follower");


        pt = player.transform;
        pp = player.GetComponent<Peng>();
        prb = player.GetComponent<Rigidbody2D>();
        pd = Vector2.zero;

        ft = follower.transform;
        fp = follower.GetComponent<Peng>();
        frb = follower.GetComponent<Rigidbody2D>();
        fd = Vector2.zero;

        speed = 10;
        smooth = 20;
        nextDirection = Vector2.zero;


        cornerPos = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, Mathf.Abs(-Camera.main.transform.position.z)));//获取屏幕边界右上角坐标（2D可以忽略z值并设置为0）

        isReversed = false;


        lineMap = new Dictionary<Vector3, GameObject>();
        directionQ = new Queue<Vector2>();

        SetWall();
    }

    

    void Update()
    {
        GetInput();

        SetDirection();

        Rotate();
        Move();
        
        //Draw();
    }

    void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isReversed = !isReversed;
        }
        
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)))
        {
            directionQ.Enqueue(Vector2.up);
        }

        if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)))
        {
            directionQ.Enqueue(Vector2.down);
        }

        if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)))
        {
            directionQ.Enqueue(Vector2.left);
        }

        if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)))
        {
            directionQ.Enqueue(Vector2.right);
        }
    }

    void SetDirection()//计算各类变量后设置玩家的方向
    {
        if (pp.gameover || fp.gameover)
        {
            GameOver();
            return;
            //后续就没有了
        }

        if (directionQ.Count != 0)
        {
            Debug.Log("当前方向： ");
            if (pd != Vector2.zero)
            {
                do
                {
                    if (directionQ.Peek() == pd)
                    {
                        directionQ.Dequeue();
                    }
                    else
                    {
                        pd = directionQ.Dequeue();
                        break;
                    }
                } while (directionQ.Count != 0);
            }
            else
            {
                pd = directionQ.Peek();
                pp.stop = false;
            }





        }
        else
        {
            if (pp.stop)
            {
                pd = Vector2.zero;
            }
            if (fp.stop)
            {
                fd = Vector2.zero;
            }
        }


           


        
        



    }
    void Rotate()//调整朝向
    {
        if (pd == Vector2.up)
        {
            prb.rotation = Mathf.Lerp(prb.rotation, 0, smooth*Time.deltaTime);

        }
        if (pd == Vector2.left)
        {
            prb.rotation = Mathf.Lerp(prb.rotation, 90, smooth * Time.deltaTime);

        }
        if (pd == Vector2.down)
        {
            prb.rotation = Mathf.Lerp(prb.rotation, 180, smooth * Time.deltaTime);

        }
        if (pd == Vector2.right)
        {
            prb.rotation = Mathf.Lerp(prb.rotation, 270, smooth * Time.deltaTime);

        }

        if (fd == Vector2.up)
        {
            frb.rotation = Mathf.Lerp(frb.rotation, 0, smooth * Time.deltaTime);

        }
        if (fd == Vector2.left)
        {
            frb.rotation = Mathf.Lerp(frb.rotation, 90, smooth * Time.deltaTime);

        }
        if (fd == Vector2.down)
        {
            frb.rotation = Mathf.Lerp(frb.rotation, 180, smooth * Time.deltaTime);

        }
        if (fd == Vector2.right)
        {
            frb.rotation = Mathf.Lerp(frb.rotation, 270, smooth * Time.deltaTime);

        }
    }

    void Move()//运动的最终函数，所有计算应在前面计算完
    {
        prb.velocity = pd * speed;
        frb.velocity = fd * speed;
    }




    Vector2 CheckPoint(Vector3 input)//返回目标所在整数节点，例如（0，0）至（1，1）这个矩形的坐标是（0，0）
    {
        return new Vector2(Mathf.FloorToInt(input[0]), Mathf.FloorToInt(input[1]));
    }
    //void Draw()
    //{
    //    if (pm.direction != Vector3.zero)
    //    {

    //        var qq = SetLineNumber(CheckPoint(pt.position), pm.direction);//线条坐标及方向。
    //        GameObject l;
    //        LineRenderer lr;
    //        if (lineMap.ContainsKey(qq))
    //        {
    //            l = lineMap[qq];
    //            l.transform.parent = env.transform;
    //            l.transform.parent = lines.transform;

    //        }
    //        else
    //        {
    //            l = Instantiate(linePrefab, transform.position, transform.rotation);
    //            lr = l.GetComponent<LineRenderer>();

    //            if (pm.direction == Vector3.up)
    //            {
    //                lr.SetPosition(0, new Vector2(qq.x + 0.5f, qq.y));
    //            }
    //            if (pm.direction == Vector3.down)
    //            {
    //                lr.SetPosition(0, new Vector2(qq.x + 0.5f, qq.y + 1f));
    //            }
    //            if (pm.direction == Vector3.left)
    //            {
    //                lr.SetPosition(0, new Vector2(qq.x + 1, qq.y + 0.5f));
    //            }
    //            if (pm.direction == Vector3.right)
    //            {
    //                lr.SetPosition(0, new Vector2(qq.x, qq.y + 0.5f));
    //            }


    //            l.transform.parent = lines.transform;
    //            lineMap.Add(qq, l);
    //        }
    //        lr = l.GetComponent<LineRenderer>();
    //        lr.SetPosition(1, pt.position);

    //        //var lr = l.GetComponent<LineRenderer>();
    //    }


    //}

    void DrawLine()
    {

    }

    Vector2 PositionToCoordinate(Vector3 input)//以左上角方格为00，暂时没用
    {
        return new Vector2(cornerPos.y - input.y, cornerPos.x + input.x);
    }

    Vector3 SetLineNumber(Vector2 pos,Vector3 dir)//上下左右，0123
    {
        
        if (dir == Vector3.up)
        {
            return new Vector3(pos.x, pos.y, 0);
        }
        if (dir == Vector3.down)
        {
            return new Vector3(pos.x, pos.y, 1);
        }
        if (dir == Vector3.left)
        {
            return new Vector3(pos.x, pos.y, 2);
        }
        if (dir == Vector3.right)
        {
            return new Vector3(pos.x, pos.y, 3);
        }
        if (dir == Vector3.zero)
        {

        }


        return Vector3.zero;
    }

    void SetWall()
    {
        Quaternion r = Quaternion.Euler(0, 0, 0);
        Vector3[] wallPos = { new Vector3(0, cornerPos.y + 0.5f, 0), new Vector3(0, -cornerPos.y - 0.5f, 0), new Vector3(-cornerPos.x - 0.5f, 0, 0), new Vector3(cornerPos.x + 0.5f, 0, 0)};
        List<GameObject> temp = new List<GameObject>();
        for(int i = 0; i < 4; i++)
        {
            temp.Add( Instantiate(wallPrefab, wallPos[i], r));
        }


        
        temp[0].transform.localScale = new Vector3(cornerPos.x * 2, 1, 1);
        temp[0].name = "NorthWall";
        
        temp[1].transform.localScale = new Vector3(cornerPos.x * 2, 1, 1);
        temp[1].name = "SouthWall";
        
        temp[2].transform.localScale = new Vector3(1, cornerPos.y * 2, 1);
        temp[2].name = "WestWall";
        
        temp[3].transform.localScale = new Vector3(1, cornerPos.y * 2, 1);
        temp[3].name = "EastWall";

        temp[0].transform.parent = env.transform;
        temp[1].transform.parent = env.transform;
        temp[2].transform.parent = env.transform;
        temp[3].transform.parent = env.transform;

        

    }

    void Test()
    {

    }

    void GameOver()
    {
        Debug.Log("Game Over.");
        //do something...
    }

}
