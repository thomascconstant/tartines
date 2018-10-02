using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle {
    
    public Vector3[] sommets;

    private LineDrawer[] segments;
    private Color mycolor = Color.black;

    public enum HauteurObstacle
    {
        OBS_HAUT,
        OBS_MILIEU,
        OBS_BAS,
        OBS_NONE

    }

    public HauteurObstacle hauteur = HauteurObstacle.OBS_NONE;


    public Obstacle(HauteurObstacle h)
    {
        hauteur = h;
    }


    
    public void DrawSegment()
    {

       if (sommets == null)
        {
            return;
        }

        segments = new LineDrawer[sommets.Length];
        for (int i = 1; i < segments.Length; i++)
        {
            segments[i - 1] = new LineDrawer();
            segments[i - 1].DrawLineInGameView(sommets[i - 1], sommets[i], mycolor);
        }
        segments[segments.Length - 1] = new LineDrawer();
        segments[segments.Length - 1].DrawLineInGameView(sommets[sommets.Length - 1], sommets[0], mycolor);
    }

    public void CreateBord(Vector3 p1, Vector3 p2)
    {
        sommets = new Vector3[2];
        sommets[0] = p1;
        sommets[1] = p2;
    }

    public void CreateTriangle(Vector3 p1, Vector3 p2, Vector3 p3)
    {
        sommets = new Vector3[3];
        sommets[0] = p1;
        sommets[1] = p2;
        sommets[2] = p3;
    }


    public void CreateRectangle(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
    {
        sommets = new Vector3[4];
        sommets[0] = p1;
        sommets[1] = p2;
        sommets[2] = p3;
        sommets[3] = p4;
    }

    public bool GetIntersecSegment(Vector3 s1, Vector3 s2, Vector3 p, ref Vector3 intersec)
    {
        double t;
        if (Mathf.Abs(s1.x - s2.x) < 0.0001)
            return false;
  
        t = (p.x - s1.x) / (s2.x - s1.x);

        if (t < 0 || t > 1)
            return false;

        intersec.x = p.x;
        intersec.z = p.z;
        intersec.y = (float)(s1.y + (s2.y - s1.y) * t);
        return true;

    }

    public bool GetIntersec(Vector3 p, ref Vector3 intersec, bool checkUp = true, bool checkDown = true)
    {
        double dist = double.MaxValue;
        bool exist = false;
        Vector3 intersecCur = new Vector3();

        for (int i = 0; i < sommets.Length; i++)
        {
            if (GetIntersecSegment(sommets[(i + 1) % sommets.Length], sommets[i], p, ref intersecCur))
            {
                if((intersecCur.y > p.y && checkUp) || (intersecCur.y < p.y && checkDown))
                {
                    double distCur = Mathf.Abs(intersecCur.y - p.y);
                    if (distCur < dist)
                    {
                        dist = distCur;
                        intersec.Set(intersecCur.x, intersecCur.y, intersecCur.z);
                        exist = true;
                    }
                }
            }
        }

       


        return exist;
    }


}
