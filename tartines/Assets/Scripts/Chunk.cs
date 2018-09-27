using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk {

    public Obstacle[] obstacles;

    public class Segment
    {
        public Vector3 p1;
        public Vector3 p2;
        public LineDrawer l;

        public enum HauteurSegment
        {
            SEG_HAUT,
            SEG_MILIEU,
            SEG_BAS,
            SEG_NONE,
        }

        public enum CoteSegment
        {
            SEG_ENTREE,
            SEG_SORTIE,
            SEG_NONE,

        }

        public HauteurSegment hauteur;
        public CoteSegment cote;

        public Segment(HauteurSegment h,CoteSegment c)
        {
            hauteur = h;
            cote = c;

            p1 = new Vector3();
            p2 = new Vector3();
        }

        public void Draw(Color col)
        {
            float epaisseur = 0.2f;
            if (hauteur == HauteurSegment.SEG_MILIEU)
                epaisseur = 0.4f;
            l = new LineDrawer(epaisseur);

            if (cote == CoteSegment.SEG_SORTIE)
                col = Color.cyan;
            if (cote == CoteSegment.SEG_ENTREE)
                col = Color.green;
            if (cote == CoteSegment.SEG_NONE)
                col = Color.gray;

            l.DrawLineInGameView(p1, p2, col);
        }
        
        public bool IsSame(Segment s)
        {
            if(Mathf.Abs(s.p1.x - p1.x) < 0.00001 && Mathf.Abs(s.p1.y - p1.y) < 0.00001 && 
               Mathf.Abs(s.p2.x - p2.x) < 0.00001 && Mathf.Abs(s.p2.y - p2.y) < 0.00001)
            {
                return true;
            }
            if (Mathf.Abs(s.p1.x - p2.x) < 0.00001 && Mathf.Abs(s.p1.y - p2.y) < 0.00001 &&
                Mathf.Abs(s.p2.x - p1.x) < 0.00001 && Mathf.Abs(s.p2.y - p1.y) < 0.00001)
            {
                return true;
            }
            return false;
        }

    }

    List<Segment> segments = new List<Segment>();   


	public void CreateChunk () {

        obstacles = new Obstacle[4];
        obstacles[0] = new Obstacle(Obstacle.HauteurObstacle.OBS_HAUT);

        Vector3 p1 = new Vector3(-10,5,0);
        Vector3 p2 = new Vector3(10,5,0);
        Vector3 p3 = new Vector3(3,2,0);

        obstacles[0].CreateTriangle(p1, p2, p3);
        obstacles[0].DrawSegment();

        obstacles[1] = new Obstacle(Obstacle.HauteurObstacle.OBS_BAS);

        p1 = new Vector3(-10, -5, 0);
        p2 = new Vector3(10, -5, 0);
        p3 = new Vector3(-3, -3, 0);


        obstacles[1].CreateTriangle(p1, p2, p3);
        obstacles[1].DrawSegment();

        obstacles[2] = new Obstacle(Obstacle.HauteurObstacle.OBS_MILIEU);

        p1 = new Vector3(-2, 0.5f, 0);
        p2 = new Vector3(3, 0.5f, 0);
        p3 = new Vector3(3, -1, 0);
        Vector3 p4 = new Vector3(-2, -1, 0);


        obstacles[2].CreateRectangle(p1, p2, p3, p4);
        obstacles[2].DrawSegment();

        Vector3 t = new Vector3(-5, -1, 0);

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

        foreach(Obstacle o1 in obstacles) {
            foreach (Vector3 s in o1.sommets)
            {

                //Cehck if need to check up or down
                bool checkUp = true;
                bool checkDown = true;
                foreach (Vector3 s2 in o1.sommets)
                {
                    if(Mathf.Abs(s.x - s2.x) < 0.0001)
                    {
                        if (s2.y > s.y)
                            checkUp = false;
                        if (s2.y < s.y)
                            checkDown = false;
                    }
                }

                //Cas up
                if(checkUp)
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
                            if (o2.GetIntersec(s, ref intersecCur,true, false))
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
                        Segment.CoteSegment c =  Segment.CoteSegment.SEG_NONE;

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
                        
                        Segment seg = new Segment(h,c);
                        seg.p1.Set(s.x, s.y, s.z);
                        seg.p2.Set(intersec.x, intersec.y, intersec.z);
                        segments.Add(seg);
                    }
                    
                }

                //Cas down
                if(checkDown)
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
                            if (o2.GetIntersec(s, ref intersecCur,false,true))
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

                        Segment seg = new Segment(h,c);
                        seg.p1.Set(s.x, s.y, s.z);
                        seg.p2.Set(intersec.x, intersec.y, intersec.z);
                        segments.Add(seg);
                    }
                }

                
            }
        }
               

        for(int i=segments.Count-1;i>=0;i--)
        {
            for (int j = segments.Count - 1; j >= 0; j--)
            {
                if(segments[j].IsSame(segments[i]) && i != j)
                {
                    if(segments[j].cote == Segment.CoteSegment.SEG_NONE)
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

        //segments.RemoveAll(s2 => s2.IsSame(segments[i]));

    }

    public void ShowSegments()
    {
        foreach(Segment s in segments)
        {
            s.Draw(Color.green);
        }
    }
}
