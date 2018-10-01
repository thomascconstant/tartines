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

    public List<List<Segment>> PathSegments = new List<List<Segment>>();

    int nbEntree = 2;

    public void CreerChemin(Chunk c)
    {
        List<Segment> segmentsList = c.segments;
        segmentsList.Sort((x, y) => x.p1.x.CompareTo(y.p1.x));
        foreach (Segment s in segmentsList)
        {
            if (s.hauteur == Segment.HauteurSegment.SEG_MILIEU)
            {
                if (lastUp != null && lastUp.cote == Segment.CoteSegment.SEG_SORTIE)
                {
                    lastUp.nextMilieu = s;
                }
                if (lastDown != null && lastDown.cote == Segment.CoteSegment.SEG_SORTIE)
                {
                    lastDown.nextMilieu = s;
                }
                if (lastMilieu != null)
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
                    nbEntree--;
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

        List<Segment> cheminTemp = new List<Segment>();
        // Creation des chemins
        //for (int i = 0; i < segmentsList.Count; i++)
       // {
            /*if (segmentsList[i].nextUp != null)
                Debug.Log("\nUP Segment depart:" + segmentsList[i].milieu + "\nSegment arrivée:" + segmentsList[i].nextUp.milieu);
            if (segmentsList[i].nextDown != null)
                Debug.Log("\nDOWN Segment depart:" + segmentsList[i].milieu + "\nSegment arrivée:" + segmentsList[i].nextDown.milieu);
            if (segmentsList[i].nextMilieu != null)
                Debug.Log("\nMILIEU Segment depart:" + segmentsList[i].milieu + "\nSegment arrivée:" + segmentsList[i].nextMilieu.milieu);
            if (segmentsList[i].nextMilieu == null && segmentsList[i].nextDown == null && segmentsList[i].nextUp == null)
                Debug.Log("\nJe n'ai pas de next! " + segmentsList[i].milieu);*/
           // if (segmentsList[i].nextUp != null || segmentsList[i].nextDown != null)
           // {
                AddSegment(cheminTemp, segmentsList[0]);
           // }


       // }
        Debug.Log(PathSegments.Count);
    }

    void AddLine(Segment s1, Segment s2)
    {
        LineDrawer l = new LineDrawer();

        l.DrawLineInGameView((s1.p1 + s1.p2) / 2, (s2.p1 + s2.p2) / 2, Color.red, s1, s2);
        PathView.Add(l);
    }
    public void DrawPath(Chunk c)
    {
        PathView.Clear();
        foreach(Segment s in c.segments)
        {
            
            if (s.nextUp != null)
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

    public void AddSegment(List<Segment> chemin, Segment seg)
    {
        if (seg.nextUp != null && seg.nextDown != null)
        {
            
            chemin.Add(seg);
            List<Segment> cheminBis = new List<Segment>(chemin);
            AddSegment(cheminBis, seg.nextUp);
            AddSegment(chemin, seg.nextDown);
            /*float rand = Random.Range(0, 2);
            if (rand < 0.5f)
                AddSegment(chemin, seg.nextDown);
            else
                AddSegment(chemin, seg.nextUp);*/
        }
        else if (seg.nextUp != null)
        {
            chemin.Add(seg);
            AddSegment(chemin, seg.nextUp);
        }
        else if (seg.nextDown != null)
        {
            chemin.Add(seg);
            AddSegment(chemin, seg.nextDown);
        }
        else if (seg.nextMilieu != null)
        {
            chemin.Add(seg);
            AddSegment(chemin, seg.nextMilieu);
        }
        else
        {
            chemin.Add(seg);
            List<Segment> bonChemin = new List<Segment>(chemin);
            chemin.Clear();
            PathSegments.Add(bonChemin);
        }
    }


}
