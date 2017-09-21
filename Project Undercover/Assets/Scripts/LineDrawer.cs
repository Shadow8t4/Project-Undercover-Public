using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LineDrawer
{

    public static GameObject MakeLine()
    {
        GameObject myLine = new GameObject();
        myLine.name = "line";
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        lr.startColor = Color.red;
        lr.endColor = Color.red;
        lr.startWidth = 0.005f;
        lr.endWidth = 0.005f;
        return myLine;
    }

    public static void DrawLine(GameObject line, Vector3 start, Vector3 end)
    {
        GameObject myLine = line;
        myLine.transform.position = start;
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.enabled = true;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
    }
}