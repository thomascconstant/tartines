using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk
{

    public Obstacle[] obstacles;
    public Path p = new Path();

    public List<Segment> segments = new List<Segment>();

    public void CreateChunk()
    {

        obstacles = new Obstacle[4];
        obstacles[0] = new Obstacle(Obstacle.HauteurObstacle.OBS_HAUT);

        Vector3 p1 = new Vector3(-10, 5, 0);
        Vector3 p2 = new Vector3(10, 5, 0);
        Vector3 p3 = new Vector3(3, 2, 0);

        obstacles[0].CreateTriangle(p1, p2, p3);
        obstacles[0].DrawSegment();

        obstacles[1] = new Obstacle(Obstacle.HauteurObstacle.OBS_BAS);

        p1 = new Vector3(-10, -5, 0);
        p2 = new Vector3(10, -5, 0);
        p3 = new Vector3(-3, -3, 0);


        obstacles[1].CreateTriangle(p1, p2, p3);
        obstacles[1].DrawSegment();

        obstacles[2] = new Obstacle(Obstacle.HauteurObstacle.OBS_MILIEU);

        p1 = new Vector3(-1, 1.5f, 0);
        p2 = new Vector3(4, 1.5f, 0);
        p3 = new Vector3(4, -1, 0);
        Vector3 p4 = new Vector3(-1, -1, 0);


        obstacles[2].CreateRectangle(p1, p2, p3, p4);
        obstacles[2].DrawSegment();

        Vector3 t = new Vector3(-6, -1, 0);

        p1 += t;
        p2 += t;
        p3 += t;
        p4 += t;

        obstacles[3] = new Obstacle(Obstacle.HauteurObstacle.OBS_MILIEU);

        obstacles[3].CreateRectangle(p1, p2, p3, p4);
        obstacles[3].DrawSegment();

    }

    public void BuildSegments()
    {
        segments.Clear();

        foreach (Obstacle o1 in obstacles)
        {
            foreach (Vector3 s in o1.sommets)
            {

                //Cehck if need to check up or down
                bool checkUp = true;
                bool checkDown = true;
                foreach (Vector3 s2 in o1.sommets)
                {
                    if (Mathf.Abs(s.x - s2.x) < 0.0001)
                    {
                        if (s2.y > s.y)
                            checkUp = false;
                        if (s2.y < s.y)
                            checkDown = false;
                    }
                }

                //Cas up
                if (checkUp)
                {
                    double dist = double.MaxValue;
                    Vector3 intersec = new Vector3();
                    bool exist = false;
                    Obstacle oGood = null;
                    foreach (Obstacle o2 in obstacles)
                    {
                        if (o1 != o2)
                        {
                            Vector3 intersecCur = new Vector3();
                            if (o2.GetIntersec(s, ref intersecCur, true, false))
                            {
                                double distCur = Mathf.Abs(intersecCur.y - s.y);
                                if (distCur < dist)
                                {
                                    dist = distCur;
                                    intersec.Set(intersecCur.x, intersecCur.y, intersecCur.z);
                                    exist = true;
                                    oGood = o2;
                                }
                            }
                        }
                    }

                    if (exist)
                    {
                        Segment.HauteurSegment h = Segment.HauteurSegment.SEG_NONE;
                        Segment.CoteSegment c = Segment.CoteSegment.SEG_NONE;

                        if (o1.hauteur == Obstacle.HauteurObstacle.OBS_HAUT)
                        {
                            c = Segment.CoteSegment.SEG_NONE;

                            if (oGood.hauteur == Obstacle.HauteurObstacle.OBS_MILIEU)
                                h = Segment.HauteurSegment.SEG_HAUT;

                            if (oGood.hauteur == Obstacle.HauteurObstacle.OBS_BAS)
                                h = Segment.HauteurSegment.SEG_MILIEU;
                        }

                        if (o1.hauteur == Obstacle.HauteurObstacle.OBS_BAS)
                        {
                            c = Segment.CoteSegment.SEG_NONE;

                            if (oGood.hauteur == Obstacle.HauteurObstacle.OBS_MILIEU)
                                h = Segment.HauteurSegment.SEG_BAS;

                            if (oGood.hauteur == Obstacle.HauteurObstacle.OBS_HAUT)
                                h = Segment.HauteurSegment.SEG_MILIEU;
                        }

                        if (o1.hauteur == Obstacle.HauteurObstacle.OBS_MILIEU)
                        {
                            c = Segment.CoteSegment.SEG_SORTIE;

                            foreach (Vector3 sTest in o1.sommets)
                            {
                                if (sTest.x - s.x > 0.001)
                                    c = Segment.CoteSegment.SEG_ENTREE;
                            }

                            if (oGood.hauteur == Obstacle.HauteurObstacle.OBS_HAUT)
                                h = Segment.HauteurSegment.SEG_HAUT;

                            if (oGood.hauteur == Obstacle.HauteurObstacle.OBS_BAS)
                                h = Segment.HauteurSegment.SEG_BAS;
                        }

                        Segment seg = new Segment(h, c);
                        seg.p1.Set(s.x, s.y, s.z);
                        seg.p2.Set(intersec.x, intersec.y, intersec.z);
                        seg.milieu.Set(seg.p1.x, (seg.p1.y + seg.p2.y) / 2, seg.p1.z);
                        seg.taille = Mathf.Abs(seg.p1.y) + Mathf.Abs(seg.p2.y);
                        segments.Add(seg);
                    }

                }

                //Cas down
                if (checkDown)
                {
                    double dist = double.MaxValue;
                    Vector3 intersec = new Vector3();
                    bool exist = false;
                    Obstacle oGood = null;
                    foreach (Obstacle o2 in obstacles)
                    {
                        if (o1 != o2)
                        {
                            Vector3 intersecCur = new Vector3();
                            if (o2.GetIntersec(s, ref intersecCur, false, true))
                            {
                                double distCur = Mathf.Abs(intersecCur.y - s.y);
                                if (distCur < dist)
                                {
                                    dist = distCur;
                                    intersec.Set(intersecCur.x, intersecCur.y, intersecCur.z);
                                    exist = true;
                                    oGood = o2;
                                }
                            }
                        }
                    }
                    if (exist)
                    {
                        Segment.HauteurSegment h = Segment.HauteurSegment.SEG_NONE;
                        Segment.CoteSegment c = Segment.CoteSegment.SEG_NONE;

                        if (o1.hauteur == Obstacle.HauteurObstacle.OBS_HAUT)
                        {
                            c = Segment.CoteSegment.SEG_NONE;

                            if (oGood.hauteur == Obstacle.HauteurObstacle.OBS_MILIEU)
                                h = Segment.HauteurSegment.SEG_HAUT;

                            if (oGood.hauteur == Obstacle.HauteurObstacle.OBS_BAS)
                                h = Segment.HauteurSegment.SEG_MILIEU;
                        }

                        if (o1.hauteur == Obstacle.HauteurObstacle.OBS_BAS)
                        {
                            c = Segment.CoteSegment.SEG_NONE;

                            if (oGood.hauteur == Obstacle.HauteurObstacle.OBS_MILIEU)
                                h = Segment.HauteurSegment.SEG_BAS;

                            if (oGood.hauteur == Obstacle.HauteurObstacle.OBS_HAUT)
                                h = Segment.HauteurSegment.SEG_MILIEU;
                        }

                        if (o1.hauteur == Obstacle.HauteurObstacle.OBS_MILIEU)
                        {
                            c = Segment.CoteSegment.SEG_SORTIE;

                            foreach (Vector3 sTest in o1.sommets)
                            {
                                if (sTest.x - s.x > 0.001)
                                    c = Segment.CoteSegment.SEG_ENTREE;
                            }

                            if (oGood.hauteur == Obstacle.HauteurObstacle.OBS_HAUT)
                                h = Segment.HauteurSegment.SEG_HAUT;

                            if (oGood.hauteur == Obstacle.HauteurObstacle.OBS_BAS)
                                h = Segment.HauteurSegment.SEG_BAS;
                        }

                        Segment seg = new Segment(h, c);
                        seg.p1.Set(s.x, s.y, s.z);
                        seg.p2.Set(intersec.x, intersec.y, intersec.z);
                        seg.milieu.Set(seg.p1.x, (seg.p1.y + seg.p2.y) / 2, seg.p1.z);
                        seg.taille = Mathf.Abs(seg.p1.y) + Mathf.Abs(seg.p2.y);
                        segments.Add(seg);
                    }
                }


            }
        }


        for (int i = segments.Count - 1; i >= 0; i--)
        {
            for (int j = segments.Count - 1; j >= 0; j--)
            {
                if (segments[j].IsSame(segments[i]) && i != j)
                {
                    if (segments[j].cote == Segment.CoteSegment.SEG_NONE)
                        segments.RemoveAt(j);
                    else if (segments[i].cote == Segment.CoteSegment.SEG_NONE)
                        segments.RemoveAt(i);
                    else
                    {
                        segments[i].cote = Segment.CoteSegment.SEG_NONE;
                        segments.RemoveAt(j);
                    }
                }
            }

        }

        p.CreerChemin(this);
        //segments.RemoveAll(s2 => s2.IsSame(segments[i]));

    }

    public void ShowSegments()
    {
        foreach (Segment s in segments)
        {
            s.Draw(Color.green);
        }
        p.DrawPath(this);
    }






}


