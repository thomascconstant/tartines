using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path {
    
    Segment lastUp = null;
    Segment lastDown = null;
    Segment lastMilieu = null;
    Segment lastSortieDown = null;
    Segment lastSortieUp = null;

    List<LineDrawer> PathView = new List<LineDrawer>();

    int nbEntree = 2;

    public void CreerChemin(Chunk c)
    {
        List<Segment> segmentsList = c.segments;
        segmentsList.Sort((x, y) => x.p1.x.CompareTo(y.p1.x));
        foreach(Segment s in segmentsList)
        {
            if (s.hauteur == Segment.HauteurSegment.SEG_MILIEU)
            {
                if(lastUp != null && lastUp.cote == Segment.CoteSegment.SEG_SORTIE)
                {
                    lastUp.nextMilieu = s;
                }
                if (lastDown != null && lastDown.cote == Segment.CoteSegment.SEG_SORTIE)
                {
                    lastDown.nextMilieu = s;
                }
                if (lastMilieu != null )
                {
                    lastMilieu.nextMilieu = s;
                }
                lastMilieu = s;
                lastUp = null;
                lastDown = null;
                lastSortieDown = null;
                lastSortieUp = null;
                nbEntree = 2;
            }

            if (s.hauteur == Segment.HauteurSegment.SEG_HAUT)
            {
                if (lastUp != null)
                {
                    lastUp.nextUp = s;
                }

                if (s.cote == Segment.CoteSegment.SEG_ENTREE)
                {
                    if (lastSortieDown != null)
                    {
                        lastSortieDown.nextUp = s;
                    }
                    if (lastMilieu != null)
                    {
                        lastMilieu.nextUp = s;
                    }
                }
                lastUp = s;
                if (s.cote == Segment.CoteSegment.SEG_SORTIE)
                    lastSortieUp = s;

                if (s.cote == Segment.CoteSegment.SEG_ENTREE)
                    nbEntree --;
                if (nbEntree <= 0)
                    lastMilieu = null;
            }

            if (s.hauteur == Segment.HauteurSegment.SEG_BAS)
            {
                if (lastDown != null)
                {
                    lastDown.nextDown = s;
                }

                if (s.cote == Segment.CoteSegment.SEG_ENTREE)
                {
                    if (lastSortieUp != null)
                    {
                        lastSortieUp.nextDown = s;
                    }
                    if (lastMilieu != null)
                    {
                        lastMilieu.nextDown = s;
                    }
                }
                lastDown = s;
                if (s.cote == Segment.CoteSegment.SEG_SORTIE)
                    lastSortieDown = s;
                if (s.cote == Segment.CoteSegment.SEG_ENTREE)
                    nbEntree--;
                if (nbEntree <= 0)
                    lastMilieu = null;
            }
        }
        foreach (Segment s in segmentsList)
        {
            s.distanceAParcourir = Mathf.Abs(s.p1.y) + Mathf.Abs(s.p2.y);
            if (s.nextMilieu != null)
            {
                s.distanceObs = Mathf.Abs(s.p1.x) + Mathf.Abs(s.nextMilieu.p1.x);
                if ((s.nextUp != null && s.nextUp.p1.x > s.nextMilieu.p1.x) || (s.nextDown != null && s.nextDown.p1.x > s.nextMilieu.p1.x))
                    s.distanceObs = Mathf.Abs(s.p1.x) + Mathf.Abs(s.nextMilieu.p1.x);
                else if (s.nextDown != null && s.nextDown.p1.x < s.nextMilieu.p1.x)
                    s.distanceObs = Mathf.Abs(s.p1.x) + Mathf.Abs(s.nextDown.p1.x);
                else if (s.nextUp != null && s.nextUp.p1.x < s.nextMilieu.p1.x)
                    s.distanceObs = Mathf.Abs(s.p1.x) + Mathf.Abs(s.nextUp.p1.x);
            }
            else
            {
                if (s.nextDown != null)
                    s.distanceObs = Mathf.Abs(s.p1.x) + Mathf.Abs(s.nextDown.p1.x);
                else if (s.nextUp != null)
                    s.distanceObs = Mathf.Abs(s.p1.x) + Mathf.Abs(s.nextUp.p1.x);
            }
        }

        

    }

    void AddLine(Segment s1, Segment s2, float x = 0)
    {
        LineDrawer l = new LineDrawer();

        l.DrawLineInGameView((s1.p1 + s1.p2) / 2, (s2.p1 + s2.p2) / 2, Color.red, x);
        PathView.Add(l);
    }
    public void DrawPath(Chunk c)
    {
        PathView.Clear();
        foreach(Segment s in c.segments)
        {

            float x = ParamDiff.CalculDiff(s);
            if (s.nextUp != null)
            {
                AddLine(s, s.nextUp, x);
            }
            if (s.nextDown != null)
            {
                AddLine(s, s.nextDown, x);
            }
            if (s.nextMilieu != null)
            {
                AddLine(s, s.nextMilieu, x);
            }
        }

    }


}
