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

        public Segment()
        {
            p1 = new Vector3();
            p2 = new Vector3();
        }

        public void Draw(Color col)
        {
            l = new LineDrawer();
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

        obstacles = new Obstacle[3];
        obstacles[0] = new Obstacle();

        Vector3 p1 = new Vector3(-10,5,0);
        Vector3 p2 = new Vector3(10,5,0);
        Vector3 p3 = new Vector3(3,2,0);

        obstacles[0].CreateTriangle(p1, p2, p3);
        obstacles[0].DrawSegment();

        obstacles[1] = new Obstacle();

        p1 = new Vector3(-10, -5, 0);
        p2 = new Vector3(10, -5, 0);
        p3 = new Vector3(-3, -3, 0);


        obstacles[1].CreateTriangle(p1, p2, p3);
        obstacles[1].DrawSegment();

        obstacles[2] = new Obstacle();

        p1 = new Vector3(-2, 0.5f, 0);
        p2 = new Vector3(3, 1, 0);
        p3 = new Vector3(0, 0, 0);
        //Vector3 p4 = new Vector3(-2, 0, 0);


        obstacles[2].CreateTriangle(p1, p2, p3);
        obstacles[2].DrawSegment();

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
                                }
                            }
                        }
                    }

                    if (exist)
                    {
                        Segment seg = new Segment();
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
                                }
                            }
                        }
                    }
                    if (exist)
                    {
                        Segment seg = new Segment();
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
                    segments.RemoveAt(j);
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
