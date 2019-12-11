using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class SimpleMove : MonoBehaviour
{

    public float speed;//移动速度

    public Text bugtext;//测试用
    
    Vector3 direction;//当前移动方向
    Vector2 coordinate;//当前坐标

    Quaternion rota;//当前朝向（未实装）

    Queue<Vector3> directionQ;//输入接收


    bool hasChangeDirection;//判断是否可以改变方向

    void Start()
    {
        speed = 10;
        direction = Vector3.zero;
        rota = Quaternion.Euler(0, 0, 0);
        hasChangeDirection = false;

        directionQ = new Queue<Vector3>();
    }

    // Update is called once per frame
    void Update()
    {
        
        GetInput();//获取输入
        Move();//移动
        ToNextPoint();//判断是否到达下一格


        BugText();
    }




    void BugText()
    {
        var str = new StringBuilder();
        str.Clear();
        str.Append("当前方向： "+DtoD(direction)+'\n');
        if(directionQ.Count==0)
        str.Append("下一个方向： 无");
        else
        str.Append("下一个方向： "+DtoD(directionQ.Peek()));

        bugtext.text = str.ToString();

    }
    void Move()
    {
        //判断是否到达可位移点
        if (IsOverHalf(transform.position, direction))
        {
            if (SetDirection())//设置方向
            {
                transform.position = PointFix(transform.position);
            }
        }
        
        //移动玩家
        //transform.rotation=rota;
        transform.position += direction * speed * Time.deltaTime;
    }
    Vector2 CheckPoint(Vector3 input)
    {
        Vector2 output = new Vector2(Mathf.FloorToInt(input[0]), Mathf.FloorToInt(input[1]));
        return output;
    }

    void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            directionQ.Enqueue(Vector3.up);
        }

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            directionQ.Enqueue(Vector3.down);
        }

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            directionQ.Enqueue(Vector3.left);
        }

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
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
                hasChangeDirection = true;
                return true;
            }
        }
        return false;
    }
    bool ToNextPoint()
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
        if (dire == Vector3.up)
            return pos.y >= CheckPoint(pos).y+0.5;
        if (dire == Vector3.down)
            return pos.y <= CheckPoint(pos).y + 0.5;
        if (dire == Vector3.left)
            return pos.x <= CheckPoint(pos).x + 0.5;
        if (dire == Vector3.right)
            return pos.x >= CheckPoint(pos).x + 0.5;

        if (dire == Vector3.zero)
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


}
