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

    public void CreerChemins(Chunk c, bool clear = true)
    {
        if (clear)
            PathSegments.Clear();
        List<Segment> segmentsList = c.segments;
        //segmentsList.Sort((x, y) => x.p1.x.CompareTo(y.p1.x));
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
        l.pointEntree = s1.pointSousOpti;
        l.pointSortie = s2.pointSousOpti;
        l.DrawLineInGameView(l.pointEntree, l.pointSortie, col, false, s1, s2, diff);
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
            //LisserChemins(chemin);
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

    public void AddSegment(List<Segment> chemin, Segment seg)
    {

        if (seg.nextUp != null && seg.nextDown != null)
        {

            chemin.Add(new Segment(seg));
            List<Segment> cheminBis = new List<Segment>(chemin);
            AddSegment(cheminBis, seg.nextUp);
            AddSegment(chemin, seg.nextDown);

        }
        else if (seg.nextUp != null)
        {
            chemin.Add(new Segment(seg));
            AddSegment(chemin, seg.nextUp);

        }
        else if (seg.nextDown != null)
        {

            chemin.Add(new Segment(seg));
            AddSegment(chemin, seg.nextDown);

        }
        else if (seg.nextMilieu != null)
        {

            if (seg.nextMilieu.nextUp != null && seg.nextMilieu.nextDown != null && Mathf.Abs(seg.nextMilieu.p1.x - seg.nextMilieu.nextUp.p1.x) < 1 && Mathf.Abs(seg.nextMilieu.p1.x - seg.nextMilieu.nextDown.p1.x) < 1)
            {
                chemin.Add(new Segment(seg));
                List<Segment> cheminSec = new List<Segment>(chemin);
                AddSegment(cheminSec, seg.nextMilieu.nextUp);
                AddSegment(chemin, seg.nextMilieu.nextDown);
            }

            else if (seg.nextMilieu.nextUp != null && Mathf.Abs(seg.nextMilieu.nextUp.p1.x - seg.nextMilieu.p1.x) < 1)
            {
                chemin.Add(new Segment(seg));
                AddSegment(chemin, seg.nextMilieu.nextUp);
            }
            else if (seg.nextMilieu.nextDown != null && Mathf.Abs(seg.nextMilieu.nextDown.p1.x - seg.nextMilieu.p1.x) < 1)
            {
                chemin.Add(new Segment(seg));
                AddSegment(chemin, seg.nextMilieu.nextDown);
            }
            else if (seg.nextMilieu.nextMilieu != null && Mathf.Abs(seg.nextMilieu.nextMilieu.p1.x - seg.nextMilieu.p1.x) < 1)
            {
                chemin.Add(new Segment(seg));
                AddSegment(chemin, seg.nextMilieu);
            }
            else if (Mathf.Abs(seg.nextMilieu.p1.x - seg.p1.x) < 1 && (seg.cote == Segment.CoteSegment.SEG_ENTREE || seg.cote == Segment.CoteSegment.SEG_SORTIE))
            {
                chemin.Add(new Segment(seg));
                AddSegment(chemin, seg.nextMilieu.nextMilieu);
            }
            else
            {
                chemin.Add(new Segment(seg));
                AddSegment(chemin, seg.nextMilieu);
            }
        }
        else
        {

            chemin.Add(new Segment(seg));

            PathSegments.Add(new List<Segment>(chemin));
            chemin.Clear();
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

    public void LisserChemins(Chunk c)
    {
        

        foreach(List<Segment> chemin in c.p.PathSegments)
            for(int i = 0; i < chemin.Count - 1; i++)
            {
                if(chemin[i+1].pointSousOpti.y == 0)
                {
                    if(chemin[0].pointSousOpti.y == 0)
                        chemin[0].pointSousOpti = chemin[0].milieu;
                    chemin[i + 1].pointSousOpti = chemin[i + 1].milieu;
            
                    float yBas = chemin[i + 1].p1.y;
                    float yHaut = chemin[i + 1].p2.y; 

                    if (yBas > yHaut)
                    {
                        float yTemp = yBas;
                        yBas = yHaut + c.hauteurPerso / 2;
                        yHaut = yTemp - c.hauteurPerso / 2;
                    }


                    if (chemin[i + 1] != null)
                    {
                        if (chemin[i + 1].hauteur == Segment.HauteurSegment.SEG_HAUT)
                        {
                            if (chemin[i + 1].cote != Segment.CoteSegment.SEG_NONE)
                            {
                                chemin[i + 1].pointOpti = new Vector3(chemin[i + 1].p1.x, yBas, 0);
                                chemin[i + 1].pointSousOpti = new Vector3(chemin[i + 1].p1.x, (yBas + chemin[i + 1].milieu.y) / 2, 0);
                            }
                            else
                                chemin[i + 1].pointSousOpti = new Vector3(chemin[i + 1].p1.x, chemin[i].pointSousOpti.y, 0);
                        }





                        else if (chemin[i + 1].hauteur == Segment.HauteurSegment.SEG_BAS)
                        {
                            if (chemin[i + 1].cote != Segment.CoteSegment.SEG_NONE)
                            {
                                chemin[i + 1].pointOpti = new Vector3(chemin[i + 1].p1.x, yHaut, 0);
                                chemin[i + 1].pointSousOpti = new Vector3(chemin[i + 1].p1.x, (yHaut + chemin[i + 1].milieu.y) / 2, 0);
                            }
                            else
                                chemin[i + 1].pointSousOpti = new Vector3(chemin[i + 1].p1.x, chemin[i].pointSousOpti.y, 0);
                        }

                        else
                            chemin[i + 1].pointSousOpti = new Vector3(chemin[i + 1].p1.x, (chemin[i].pointSousOpti.y + chemin[i + 1].milieu.y) / 2, 0);
                    }


                    else
                    {
                        Debug.Log(chemin[i].milieu);
                    }
                }
            }
        
    }

    public void CreerToutChemin(Chunk c1, Chunk c2 = null)
    {


        c1.segments.Sort((x, y) => x.p1.x.CompareTo(y.p1.x));
        if (c2 != null)
        {

            c2.segments.Sort((x, y) => x.p1.x.CompareTo(y.p1.x));
            c2.segments[0].pointSousOpti = c1.p.PathSegments[0][c1.p.PathSegments[0].Count - 1].pointSousOpti;
            CreerChemins(c2);
            /*c2.segments[0].pointSousOpti = c1.p.PathSegments[1][c1.p.PathSegments[1].Count - 1].pointSousOpti;
            CreerChemins(c2, false);*/
            foreach (List<Segment> chemin in PathSegments)
            {
                if (chemin.Count != 0)
                    c2.p.PathSegments.Add(chemin);
            }


            LisserChemins(c2);
        }
        else
        {
            CreerChemins(c1);
            foreach(List<Segment> chemin in PathSegments)
            {
                if (chemin.Count != 0)
                    c1.p.PathSegments.Add(chemin);
            }
            LisserChemins(c1);
        }
        
    }

}
