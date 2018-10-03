﻿using System.Collections;
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
        AddSegment(cheminTemp, segmentsList[0]);
     }

    void AddLine(Segment s1, Segment s2, Color col, float epaisseur, float diff)
    {
        LineDrawer l = new LineDrawer(epaisseur);

        l.DrawLineInGameView((s1.p1 + s1.p2) / 2, (s2.p1 + s2.p2) / 2, col, s1, s2, diff);
        PathView.Add(l);

    }
    public void DrawPath(Chunk c)
    {
        PathView.Clear();
        Color col = new Color();
        float epaisseur = 0.1f;
        float diffChemin = 0;
        foreach (List<Segment> chemin in PathSegments)
        {
            CalculParam(chemin);
            for (int j = 0; j < chemin.Count - 1; j++)
            {
                chemin[j].difficulte = ParamDiff.CalculDiff(chemin[j]);
            }
                

            col = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            for (int j = 0; j < chemin.Count -1; j++)
            {
                diffChemin += chemin[j].difficulte;
                if (chemin[j + 1] != null)
                {
                    AddLine(chemin[j], chemin[j + 1], col, epaisseur, diffChemin);
                }
            }
            epaisseur += 0.1f;
            diffChemin = 0;

        }
    }

    public void AddSegmentChunks(List<Chunk> listChunk)
    {
        foreach (Chunk c in listChunk)
        {
            List<Segment> chemin = new List<Segment>();

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

            if (seg.nextMilieu.nextUp != null && seg.nextMilieu.nextDown != null && Mathf.Abs(seg.nextMilieu.p1.x - seg.nextMilieu.nextUp.p1.x) < 0.5 && Mathf.Abs(seg.nextMilieu.p1.x - seg.nextMilieu.nextDown.p1.x) < 0.5)
            {
                chemin.Add(seg);
                List<Segment> cheminSec = new List<Segment>(chemin);
                AddSegment(cheminSec, seg.nextMilieu.nextUp);
                AddSegment(chemin, seg.nextMilieu.nextDown);
            }

            else if (seg.nextMilieu.nextUp != null && Mathf.Abs(seg.nextMilieu.p1.x - seg.nextMilieu.nextUp.p1.x) < 0.5)
            {
                chemin.Add(seg);
                AddSegment(chemin, seg.nextMilieu.nextUp);
            }
            else if (seg.nextMilieu.nextDown != null && Mathf.Abs(seg.nextMilieu.p1.x - seg.nextMilieu.nextDown.p1.x) < 0.5)
            {
                chemin.Add(seg);
                AddSegment(chemin, seg.nextMilieu.nextDown);
            }
            else if (seg.nextMilieu.nextMilieu != null && Mathf.Abs(seg.nextMilieu.p1.x - seg.nextMilieu.nextMilieu.p1.x) < 0.5)
            {
                chemin.Add(seg);
                AddSegment(chemin, seg.nextMilieu);
            }
            else if (Mathf.Abs(seg.p1.x - seg.nextMilieu.p1.x) < 0.5)
            {
                AddSegment(chemin, seg.nextMilieu);
            }
            else
            {
                chemin.Add(seg);
                AddSegment(chemin, seg.nextMilieu);
            }
        }
        else
        {
            chemin.Add(seg);
            List<Segment> bonChemin;
            
            //CalculParam(bonChemin);
            PathSegments.Add(bonChemin = new List<Segment>(chemin));
            chemin.Clear();
            // bonChemin.Clear();
        }
    }

    private void CalculParam(List<Segment> chemin)
    {
        for (int i = 0; i < chemin.Count - 1; i++)
        {
            chemin[i].distanceObs = Mathf.Abs(chemin[i].p1.x - chemin[i + 1].p1.x);
            chemin[i].distanceAParcourir = Mathf.Abs(chemin[i].milieu.y - chemin[i + 1].milieu.y);
        }
    }

}
