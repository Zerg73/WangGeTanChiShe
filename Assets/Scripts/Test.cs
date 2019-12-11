using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update


    public enum Dire { up = 0, down, left, right, empty }

    public GameObject pre;


    public Vector3[] vertex = { new Vector3(1, 0.5f, 0), new Vector3(0.5f, 1, 0), new Vector3(1, 0.5f, 0), new Vector3(0.5f, 0, 0) };


    public Material mat;
    void OnPostRender()
    {
        //if (!mat)
        //{
        //    Debug.LogError("Please Assign a material on the inspector");
        //    return;
        //}
        //GL.PushMatrix();
        //mat.SetPass(0);
        //GL.LoadOrtho();
        //GL.Begin(GL.QUADS);
        //GL.Color(Color.red);
        //GL.Vertex(vertex[0]);
        //GL.Vertex(vertex[1]);
        //GL.Vertex(vertex[2]);
        //GL.Vertex(vertex[3]);

        //GL.Color(Color.cyan);
        //GL.Vertex(vertex[0]);
        //GL.Vertex(vertex[1]);
        //GL.Vertex(vertex[2]);
        //GL.Vertex(vertex[3]);
        //GL.End();
        //GL.PopMatrix();
    }
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



}

