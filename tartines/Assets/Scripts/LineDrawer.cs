﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineDrawer
{
    private LineRenderer lineRenderer;
    private float lineSize;
    private GameObject lineObj;


    public LineDrawer(float lineSize = 0.2f)
    {
        lineObj = new GameObject("LineObj");
        lineRenderer = lineObj.AddComponent<LineRenderer>();
        //Particles/Additive
        lineRenderer.material = new Material(Shader.Find("Hidden/Internal-Colored"));

        this.lineSize = lineSize;
    }

    public void SetParent(Transform parent)
    {
        lineObj.transform.SetParent(parent);
    }

    private void init(float lineSize = 0.2f)
    {
        if (lineRenderer == null)
        {
            GameObject lineObj = new GameObject("LineObj");
            lineRenderer = lineObj.AddComponent<LineRenderer>();
            //Particles/Additive
            lineRenderer.material = new Material(Shader.Find("Hidden/Internal-Colored"));

            this.lineSize = lineSize;
        }
    }

    //Draws lines through the provided vertices
    public void DrawLineInGameView(Vector3 start, Vector3 end, Color color, Segment s1 = null, Segment s2 = null)
    {
        if (lineRenderer == null)
        {
            init(0.2f);
        }

        //float diff = ParamDiff.CalculDiff(seg);

        if (s1 != null)
        {
            Text Diff = lineObj.AddComponent<Text>();
            Diff.text = "Pour le segment 1: \nLe milieu est:" + s1.milieu + "\nLa distance à parcourir est:" + s1.distanceAParcourir +
                "\nLa distance à l'obstacle est:" + s1.distanceObs + "\nLa taille du segment est:" + s1.taille +
                "\n\nPour le segment 2: \nLe milieu est:" + s2.milieu + "\nLa distance à parcourir est:" + s2.distanceAParcourir +
                "\nLa distance à l'obstacle est:" + s2.distanceObs + "\nLa taille du segment est:" + s2.taille; 
        }

        //Set color
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;

        //Set width
        lineRenderer.startWidth = lineSize;
        lineRenderer.endWidth = lineSize;

        //Set line count which is 2
        lineRenderer.positionCount = 2;

        //Set the postion of both two lines
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }

    public void Destroy()
    {
        if (lineRenderer != null)
        {
            UnityEngine.Object.Destroy(lineRenderer.gameObject);
        }
    }
}