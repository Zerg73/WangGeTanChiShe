using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject wallPrefab;
    GameObject linePrefab;
    GameObject env;//环境物体的父节点
    GameObject lines;//线条的父节点

    GameObject player, follower;
    Transform pt, ft;

    SimpleMove pm;


    Dictionary<Vector3, GameObject> lineMap;

    Vector3 cornerPos;

    void Start()
    {
        linePrefab = Resources.Load<GameObject>("Prefabs/Line");
        wallPrefab = Resources.Load<GameObject>("Prefabs/Wall");

        env = GameObject.Find("Env");
        lines = GameObject.Find("Lines");

        player = GameObject.Find("Player");
        pt = player.transform;
        pm = player.GetComponent<SimpleMove>();

        lineMap = new Dictionary<Vector3, GameObject>();

        cornerPos = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, Mathf.Abs(-Camera.main.transform.position.z)));//获取屏幕边界右上角坐标（2D可以忽略z值并设置为0）
        SetWall();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)){
            //    Debug.Log(player.GetComponent<SimpleMove>().direction);
            Draw();

        }

    }

    void Draw()
    {

        var qq = SetLineNumber(F.CheckPoint(pt.position), pm.direction);

        GameObject l;

        if (lineMap.ContainsKey(qq))
        {
            l = lineMap[qq];
        }
        else
        {
            l = Instantiate(linePrefab,lines.transform);
            lineMap.Add(qq,l);
        }
        
        Debug.Log(qq);

    }

    Vector2 PositionToCoordinate(Vector3 input)//以左上角方格为00，
    {
        return new Vector2(cornerPos.y - input.y, cornerPos.x + input.x);
    }

    Vector3 SetLineNumber(Vector2 pos,Vector3 dir)//横线Z值为0，竖线为1
    {
        if (dir == Vector3.left || dir == Vector3.right)
        {
            return new Vector3(pos.x,pos.y,0);
        }
        if (dir == Vector3.up || dir == Vector3.down)
        {
            return new Vector3(pos.x, pos.y, 1);
        }

        Debug.Log("Set Line Number Function Error.");
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

}
