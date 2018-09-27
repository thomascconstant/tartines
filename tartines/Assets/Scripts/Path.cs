using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path {
    
    Chunk.Segment lastUp = null;
    Chunk.Segment lastDown = null;
    Chunk.Segment lastMilieu = null;
    Chunk.Segment lastSortieDown = null;
    Chunk.Segment lastSortieUp = null;

    List<LineDrawer> PathView = new List<LineDrawer>();

    int nbEntree = 2;

    public void CreerChemin(Chunk c)
    {
        List<Chunk.Segment> segmentsList = c.segments;
        segmentsList.Sort((x, y) => x.p1.x.CompareTo(y.p1.x));
        foreach(Chunk.Segment s in segmentsList)
        {
            if (s.hauteur == Chunk.Segment.HauteurSegment.SEG_MILIEU)
            {
                if(lastUp != null && lastUp.cote == Chunk.Segment.CoteSegment.SEG_SORTIE)
                {
                    lastUp.nextMilieu = s;
                }
                if (lastDown != null && lastDown.cote == Chunk.Segment.CoteSegment.SEG_SORTIE)
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

            if (s.hauteur == Chunk.Segment.HauteurSegment.SEG_HAUT)
            {
                if (lastUp != null)
                {
                    lastUp.nextUp = s;
                }

                if (s.cote == Chunk.Segment.CoteSegment.SEG_ENTREE)
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
                if (s.cote == Chunk.Segment.CoteSegment.SEG_SORTIE)
                    lastSortieUp = s;

                if (s.cote == Chunk.Segment.CoteSegment.SEG_ENTREE)
                    nbEntree --;
                if (nbEntree <= 0)
                    lastMilieu = null;
            }

            if (s.hauteur == Chunk.Segment.HauteurSegment.SEG_BAS)
            {
                if (lastDown != null)
                {
                    lastDown.nextDown = s;
                }

                if (s.cote == Chunk.Segment.CoteSegment.SEG_ENTREE)
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
                if (s.cote == Chunk.Segment.CoteSegment.SEG_SORTIE)
                    lastSortieDown = s;
                if (s.cote == Chunk.Segment.CoteSegment.SEG_ENTREE)
                    nbEntree--;
                if (nbEntree <= 0)
                    lastMilieu = null;
            }

        }

        

    }

    void AddLine(Chunk.Segment s1, Chunk.Segment s2)
    {
        LineDrawer l = new LineDrawer();
        l.DrawLineInGameView((s1.p1 + s1.p2) / 2, (s2.p1 + s2.p2) / 2, Color.red);
        PathView.Add(l);
    }
    public void DrawPath(Chunk c)
    {
        PathView.Clear();
        foreach(Chunk.Segment s in c.segments)
        {
            if(s.nextUp != null)
            {
                AddLine(s, s.nextUp);
            }
            if (s.nextDown != null)
            {
                AddLine(s, s.nextDown);
            }
            if (s.nextMilieu != null)
            {
                AddLine(s, s.nextMilieu);
            }
        }

    }


}
