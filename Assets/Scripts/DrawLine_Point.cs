using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine_Point : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform linesSet;
    public GameObject linePrefab;
    LineRenderer lr;

    public Vector3 startpos, endpos;

    
    void Start()
    {
        //startpos = new Vector3(5,2,0);
        //endpos = new Vector3(10,2,0);
        
        
        
        
        SetLineProperty();
    }

    // Update is called once per frame
    void Update()
    {
        SetPosition();
    }


    void SetLineProperty()
    {
        lr = linePrefab.GetComponent<LineRenderer>();
        lr.startColor = Color.red;
        lr.endColor = Color.red;

        lr.startWidth = 0.6f;
        lr.endWidth = 0.6f;
    }

    public void SetPosition()
    {
        lr.SetPosition(0, startpos);
        lr.SetPosition(1, endpos);
    }
}
