using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject wallPrefab;
    GameObject Env;//环境物体的父节点

    Vector3 cornerPos;
    void Start()
    {
        wallPrefab = Resources.Load<GameObject>("Prefabs/Wall");

        Env = GameObject.Find("Env");
        
        cornerPos = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, Mathf.Abs(-Camera.main.transform.position.z)));//获取屏幕边界右上角坐标（2D可以忽略z值并设置为0）
        SetWall();
    }

    // Update is called once per frame
    void Update()
    {
        
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

        temp[0].transform.parent = Env.transform;
        temp[1].transform.parent = Env.transform;
        temp[2].transform.parent = Env.transform;
        temp[3].transform.parent = Env.transform;

        

    }

    void Test()
    {

    }

}
