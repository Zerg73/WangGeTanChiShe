using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update


    public enum Dire {up=0,down,left,right,empty }

    public GameObject pre;
    List<bool> map;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Debug.Log(CheckPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)));
        //}

        
        


    }
    //暂时OK的函数
    public Vector2 CheckPoint(Vector3 input)
    {
        Vector2 output = new Vector2(Mathf.FloorToInt(input[0]), Mathf.FloorToInt(input[1]));
        return output;
    }

    public void DrawPoint(Vector3 pos,Dire dir)
    {
        Vector3 startPos, endPos;

        switch (dir)
        {
            case Dire.up:
                startPos = new Vector3(pos.x, pos.y - 0.5f, pos.z);
                endPos = new Vector3(pos.x, pos.y + 0.5f, pos.z);
                break;
            case Dire.down:
                startPos = new Vector3(pos.x, pos.y + 0.5f, pos.z);
                endPos = new Vector3(pos.x, pos.y - 0.5f, pos.z);
                break;
            case Dire.left:
                startPos = new Vector3(pos.x + 0.5f, pos.y, pos.z);
                endPos = new Vector3(pos.x - 0.5f, pos.y, pos.z);
                break;
            case Dire.right:
                startPos = new Vector3(pos.x - 0.5f, pos.y, pos.z);
                endPos = new Vector3(pos.x + 0.5f, pos.y, pos.z);
                break;
            default:
                Debug.Log("DrawPoint Direction Error.");
                break;
        }
        


          

    }
}
