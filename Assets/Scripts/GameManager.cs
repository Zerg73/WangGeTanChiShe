using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject wallPrefab;

    Vector3 cornerPos;
    void Start()
    {
        wallPrefab = Resources.Load<GameObject>("Prefabs/Wall");
        
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

        var temp=Instantiate(wallPrefab,wallPos[0],r);
        temp.transform.localScale = new Vector3(cornerPos.x * 2, 1, 1);
        temp=Instantiate(wallPrefab,wallPos[1],r);
        temp.transform.localScale = new Vector3(cornerPos.x * 2, 1, 1);
        temp =Instantiate(wallPrefab,wallPos[2],r);
        temp.transform.localScale = new Vector3(1, cornerPos.y * 2, 1);
        temp =Instantiate(wallPrefab,wallPos[3],r);
        temp.transform.localScale = new Vector3(1, cornerPos.y * 2, 1);
    }
}
