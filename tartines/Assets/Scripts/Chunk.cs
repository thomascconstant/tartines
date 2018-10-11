using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk
{

    public Obstacle[] obstacles;
    public Path p = new Path();

    public List<Segment> segments = new List<Segment>();

    public float hauteurMin = 1f;
    public float hauteurMax = 5f;

    public float largeurMin = 0.1f;
    public float largeurMax = 0.7f;

    public float hauteurPerso = 1.5f;

    public void BuildSegments()
    {
        segments.Clear();

        foreach (Obstacle o1 in obstacles)
        {
                 
            foreach (Vector3 s in o1.sommets)
            {

                //Check if need to check up or down
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
                        Segment.HauteurSegment h = Segment.HauteurSegment.SEG_MILIEU;
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
                        seg.milieu = Segment.GetMilieu(seg);
                        seg.taille = Mathf.Abs(seg.p1.y - seg.p2.y);
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
                        Segment.HauteurSegment h = Segment.HauteurSegment.SEG_MILIEU;
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
                        seg.milieu = Segment.GetMilieu(seg);
                        seg.taille = Mathf.Abs(seg.p1.y - seg.p2.y);
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

        //p.CreerChemins(this);
        //segments.RemoveAll(s2 => s2.IsSame(segments[i]));

    }

    public void ShowPath()
    {
        foreach (Segment s in segments)
        {
            s.Draw(Color.green);
        }
        p.DrawPath(this);
    }

    public void ShowObstacles()
    {
        foreach (Obstacle o in obstacles)
        {
            o.DrawSegment();
        }
    }

    // p1 = point du haut du chunk precedant
    // p2 = point du bas du chunk precedant
    public void GenerateChunk(Vector3 p1, Vector3 p2, float largeurChunk = 20)
    {

        largeurChunk = Random.Range(5, largeurChunk);

        obstacles = new Obstacle[3];

        float hauteurChunk = Mathf.Abs(p2.y - p1.y);

        float largeur =  Random.Range(largeurMin* largeurChunk, largeurMax* largeurChunk - (2*hauteurPerso));
        float x1 = Random.Range(hauteurPerso, largeurChunk - largeur - hauteurPerso);
        float x2 = x1 + largeur;

        float hauteur = Mathf.Min(hauteurChunk - (2* hauteurPerso), Random.Range(hauteurMin, hauteurMax));
        float y1 = Random.Range(hauteurPerso + hauteur, hauteurChunk - hauteurPerso);
        float y2 = y1 - hauteur;

        obstacles[0] = new Obstacle(Obstacle.HauteurObstacle.OBS_MILIEU, Obstacle.TypeObstacle.OBS_RECTANGLE);
        Vector3 point1 = new Vector3(x1 + p1.x, p2.y+y1, 0);
        Vector3 point2 = new Vector3(x2 + p1.x, point1.y, 0);
        Vector3 point3 = new Vector3(point2.x, p2.y + y2, 0);
        Vector3 point4 = new Vector3(point1.x, point3.y, 0);
        obstacles[0].CreateRectangle(point1, point2, point3, point4);

        obstacles[1] = new Obstacle(Obstacle.HauteurObstacle.OBS_HAUT, Obstacle.TypeObstacle.OBS_TRIANGLE);
        point1 = new Vector3(p1.x, p1.y, 0);
        point2 = new Vector3(p1.x + largeurChunk, p1.y, 0);
        point3 = new Vector3(Random.Range(p1.x + 1, p1.x + largeurChunk - 1), p2.y + Random.Range(y1+ hauteurPerso, hauteurChunk), 0);
        obstacles[1].CreateTriangle(point1, point2, point3);                                             

        obstacles[2] = new Obstacle(Obstacle.HauteurObstacle.OBS_BAS, Obstacle.TypeObstacle.OBS_TRIANGLE);
        point1 = new Vector3(p2.x, p2.y, 0);
        point2 = new Vector3(p2.x + largeurChunk, p2.y, 0);
        point3 = new Vector3(Random.Range(p2.x + 1, p2.x + largeurChunk - 1), p2.y + Random.Range(0, y2 - hauteurPerso), 0);
        obstacles[2].CreateTriangle(point1, point2, point3);

    }

    public void GenerateChunkReset (Vector3 p1, Vector3 p2, float largeurChunk = 20)
    {
        largeurChunk = Random.Range(5, largeurChunk);

        obstacles = new Obstacle[2];

        float hauteurChunk = Mathf.Abs(p2.y - p1.y);

        float largeur = Random.Range(largeurMin * largeurChunk, largeurMax * largeurChunk - (2 * hauteurPerso));
        float x1 = Random.Range(hauteurPerso, largeurChunk - largeur - hauteurPerso);


        float hauteur = hauteurPerso/2;
        float y1 = hauteurChunk + hauteur;
        float y2 = hauteurChunk - hauteur;

        obstacles[0] = new Obstacle(Obstacle.HauteurObstacle.OBS_HAUT, Obstacle.TypeObstacle.OBS_TRIANGLE);
        Vector3 point1 = new Vector3(p1.x, p1.y, 0);
        Vector3 point2 = new Vector3(p1.x + largeurChunk, p1.y, 0);
        Vector3 point3 = new Vector3((p1.x + largeurChunk/2 + 0.01f), y1, 0);
        obstacles[0].CreateTriangle(point1, point2, point3);

        obstacles[1] = new Obstacle(Obstacle.HauteurObstacle.OBS_BAS, Obstacle.TypeObstacle.OBS_TRIANGLE);
        point1 = new Vector3(p1.x, p2.y, 0);
        point2 = new Vector3(p1.x + largeurChunk, p2.y, 0);
        point3 = new Vector3((p1.x + largeurChunk / 2 - 0.01f), y2, 0);
        obstacles[1].CreateTriangle(point1, point2, point3);
    }



}


