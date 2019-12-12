using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//用于存放公共函数
public class F : MonoBehaviour
{
    static public Vector2 CheckPoint(Vector3 input)//返回目标所在整数节点，例如（0，0）至（1，1）这个矩形的坐标是（0，0）
    {
        return new Vector2(Mathf.FloorToInt(input[0]), Mathf.FloorToInt(input[1]));
    }
}
