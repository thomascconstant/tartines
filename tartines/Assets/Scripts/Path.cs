using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Path {
    
    Segment lastUp = null;
    Segment lastDown = null;
    Segment lastMilieu = null;
    Segment lastSortieDown = null;
    Segment lastSortieUp = null;

    float poidsMilieu = 0.3f;
    float poidsOpti = 0.7f;
    float poidsMaxDist = 8.0f;
    float distanceEntreSegment = 0f;
    

    List<LineDrawer> PathView = new List<LineDrawer>();


    public List<List<Segment>> PathSegments = new List<List<Segment>>();

    public List<List<Segment>> FinalPath = new List<List<Segment>>();

    int nbEntree = 2;

    public void CreerChemins(Chunk c, bool clear = true)
    {
        if (clear)
            PathSegments.Clear();
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

            /*Text t = s.l.lineObj.AddComponent<Text>();
            t.text = "" + (s.nextUp != null ? "up" : "") + " " + (s.nextDown != null ? "down" : "") + " " + (s.nextMilieu != null ? "milieu" : "");*/
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

    public void AddSegment(List<Segment> chemin, Segment seg, float distCumul = 0)
    {
        bool segJump = false;
        float dist = 0;
        if (seg.cote == Segment.CoteSegment.SEG_NONE)
        {
            //Debug.Log("Check seg : " + seg.milieu.x);
            //on regarde la distance au précédent
            if (chemin.Count > 0)
            {
                Segment previous = chemin[chemin.Count - 1];
                float distPrevious = Mathf.Abs(seg.p1.x - previous.p1.x);
                if (distPrevious + distCumul < distanceEntreSegment)
                {
                    segJump = true;
                    Debug.Log("Before too close ! " + distPrevious );
                    dist = distCumul + distPrevious;
                }

            }

            //on regarde le suivant, mais que si on est pas le premier (cause debut de chunk obligatoire pour pas
            //exploser le nombre de chemins)
            if (!segJump && chemin.Count > 0)
            {
                Segment closestNext = seg.nextUp;
                if (closestNext == null)
                    closestNext = seg.nextDown;
                if (closestNext != null)
                {
                    float distNext = Mathf.Abs(seg.p1.x - closestNext.p1.x);
                    if (distNext < distanceEntreSegment)
                    {
                        segJump = true;
                        Debug.Log("Next too close ! " + distNext);
                    }
                }
            }
        }

        //Si je suis le dernier on me saute pas
        if (seg.nextDown == null && seg.nextMilieu == null && seg.nextUp == null)
            segJump = false;

        if (!segJump)
        {
            chemin.Add(new Segment(seg));
        }
        else
        {
            Debug.Log("jump " + seg.p1.x);
        }

        if (seg.nextUp != null && seg.nextDown != null)
        {
            List<Segment> cheminBis = new List<Segment>(chemin);
            AddSegment(cheminBis, seg.nextUp, dist);
            AddSegment(chemin, seg.nextDown, dist);

        }
        else if (seg.nextUp != null)
        {
            AddSegment(chemin, seg.nextUp, dist);
        }
        else if (seg.nextDown != null)
        {
            AddSegment(chemin, seg.nextDown, dist);
        }
        else if (seg.nextMilieu != null)
        {
            AddSegment(chemin, seg.nextMilieu, dist);
        }
        else
        {
            PathSegments.Add(new List<Segment>(chemin));
            chemin.Clear();
        }
            
    

        
       /* else if (seg.nextMilieu != null)
        {
            

           *if (seg.nextMilieu.nextUp != null && seg.nextMilieu.nextDown != null && Mathf.Abs(seg.nextMilieu.p1.x - seg.nextMilieu.nextUp.p1.x) < distanceEntreSegment
                && Mathf.Abs(seg.nextMilieu.p1.x - seg.nextMilieu.nextDown.p1.x) < distanceEntreSegment)
            {
                chemin.Add(new Segment(seg));
                List<Segment> cheminSec = new List<Segment>(chemin);
                AddSegment(cheminSec, seg.nextMilieu.nextUp);
                AddSegment(chemin, seg.nextMilieu.nextDown);
            }

            else if (seg.nextMilieu.nextUp != null && Mathf.Abs(seg.nextMilieu.nextUp.p1.x - seg.nextMilieu.p1.x) < distanceEntreSegment)
            {
                chemin.Add(new Segment(seg));
                AddSegment(chemin, seg.nextMilieu.nextUp);
            }
            else if (seg.nextMilieu.nextDown != null && Mathf.Abs(seg.nextMilieu.nextDown.p1.x - seg.nextMilieu.p1.x) < distanceEntreSegment)
            {
                chemin.Add(new Segment(seg));
                AddSegment(chemin, seg.nextMilieu.nextDown);
            }
            else if (seg.nextMilieu.nextMilieu != null && Mathf.Abs(seg.nextMilieu.nextMilieu.p1.x - seg.nextMilieu.p1.x) < distanceEntreSegment)
            {
                chemin.Add(new Segment(seg));
                AddSegment(chemin, seg.nextMilieu);
            }
            else if (Mathf.Abs(seg.nextMilieu.p1.x - seg.p1.x) < distanceEntreSegment && (seg.cote == Segment.CoteSegment.SEG_ENTREE || seg.cote == Segment.CoteSegment.SEG_SORTIE))
            {
                chemin.Add(new Segment(seg));
                AddSegment(chemin, seg.nextMilieu.nextMilieu);
            }
            else
            {
                chemin.Add(new Segment(seg));
                AddSegment(chemin, seg.nextMilieu);
            }
        }*/
       
        
               
    }

    private void CalculParam(List<Segment> chemin)
    {
        for (int i = 0; i < chemin.Count - 1; i++)
        {
            chemin[i].distanceObs = Mathf.Abs(chemin[i].p1.x - chemin[i + 1].p1.x);
            chemin[i].distanceAParcourir = Mathf.Abs(chemin[i].pointSousOpti.y - chemin[i + 1].pointSousOpti.y);
        }
    }

    /// <summary>
    /// C2 = chunk précédent
    /// </summary>
    /// <param name="c"></param>
    /// <param name="c2">chunk précédent </param>
    public void LisserChemins(Chunk c, Chunk c2 = null)
    {
        List<List<Segment>> Temp = new List<List<Segment>>();
        foreach (List<Segment> chemin in c.p.PathSegments)
        {

            //On va modifier les segments (point sous opti) donc copie
            List<Segment> cheminTemp = new List<Segment>();
            foreach (Segment segTemp in chemin)
                cheminTemp.Add(new Segment(segTemp));
            Temp.Add(cheminTemp);
            //On raccorde les chunks si pas fait
            if (chemin[0].pointSousOpti.y == 0)
            {
                //Si chunck precedent
                if (c2 != null)
                    chemin[0].pointSousOpti = c2.p.PathSegments[c2.p.PathSegments.Count - 1][c2.p.PathSegments[c2.p.PathSegments.Count - 1].Count - 1].pointSousOpti;
                else
                    chemin[0].pointSousOpti = chemin[0].milieu;
            }

            for (int i = 0; i < chemin.Count - 1; i++)
            {
                //Si suivant pas encore set
                if (chemin[i + 1].pointSousOpti.y == 0)
                {
                    chemin[i + 1].pointSousOpti = chemin[i + 1].milieu;

                    float yBas = chemin[i + 1].p1.y;
                    float yHaut = chemin[i + 1].p2.y;

                    if (yBas > yHaut)
                    {
                        float yTemp = yBas;
                        yBas = yHaut;
                        yHaut = yTemp;
                    }
                    yBas += c.hauteurPerso / 2;
                    yHaut -= c.hauteurPerso / 2;

                    float yOptim = chemin[i].pointSousOpti.y;
                    yOptim = Mathf.Clamp(yOptim, yBas, yHaut);

                    float dist = Mathf.Abs(chemin[i ].p1.x - chemin[i + 1].p1.x);
                    float lPoidsMilieu = Mathf.Lerp(0, poidsMilieu, dist / poidsMaxDist);
                    float lPoidsOpti = Mathf.Lerp(1, poidsOpti, dist / poidsMaxDist);

                    chemin[i + 1].pointSousOpti = new Vector3(chemin[i + 1].p1.x, (yOptim * lPoidsOpti + chemin[i + 1].milieu.y * lPoidsMilieu), 0);
                }
            }
        }
        foreach (List<Segment> chemin in Temp)
        {
            if (chemin[0].pointSousOpti.y == 0)
            {
                //Si chunck precedent
                if (c2 != null)
                    chemin[0].pointSousOpti = c2.p.PathSegments[0][c2.p.PathSegments[0].Count - 1].pointSousOpti;
                else
                    chemin[0].pointSousOpti = chemin[0].milieu;
            }

            for (int i = 0; i < chemin.Count - 1; i++)
            {
                //Si suivant pas encore set
                if (chemin[i + 1].pointSousOpti.y == 0)
                {
                    chemin[i + 1].pointSousOpti = chemin[i + 1].milieu;

                    float yBas = chemin[i + 1].p1.y;
                    float yHaut = chemin[i + 1].p2.y;

                    if (yBas > yHaut)
                    {
                        float yTemp = yBas;
                        yBas = yHaut;
                        yHaut = yTemp;
                    }
                    yBas += c.hauteurPerso / 2;
                    yHaut -= c.hauteurPerso / 2;

                    float yOptim = chemin[i].pointSousOpti.y;
                    yOptim = Mathf.Clamp(yOptim, yBas, yHaut);

                    float dist = Mathf.Abs(chemin[i].p1.x - chemin[i + 1].p1.x);
                    float lPoidsMilieu = Mathf.Lerp(0, poidsMilieu, dist / poidsMaxDist);
                    float lPoidsOpti = Mathf.Lerp(1, poidsOpti, dist / poidsMaxDist);

                    chemin[i + 1].pointSousOpti = new Vector3(chemin[i + 1].p1.x, (yOptim * lPoidsOpti + chemin[i + 1].milieu.y * lPoidsMilieu), 0);
                }
            }
        }
        foreach (List<Segment> t in Temp)
            if (!c.p.PathSegments.Contains(t))
                c.p.PathSegments.Add(t);


        /*List<List<Segment>> Temp = new List<List<Segment>>();
        foreach (List<Segment> chemin in c.p.PathSegments)
        {
            List<Segment> cheminTemp = new List<Segment>();
            foreach (Segment segTemp in chemin)
                cheminTemp.Add(new Segment(segTemp));
            for (int i = 0; i < chemin.Count - 1; i++)
            {
                if (chemin[i + 1].pointSousOpti.y == 0)
                {
                    if (chemin[0].pointSousOpti.y == 0)
                    {
                        if (c2 != null)
                           chemin[0].pointSousOpti = c2.p.PathSegments[c2.p.PathSegments.Count - 1][c2.p.PathSegments[c2.p.PathSegments.Count - 1].Count - 1].pointSousOpti;
                        else
                            chemin[0].pointSousOpti = chemin[0].milieu;
                    }
                    chemin[i + 1].pointSousOpti = chemin[i + 1].milieu;

                    float yBas = chemin[i + 1].p1.y;
                    float yHaut = chemin[i + 1].p2.y;

                    if (yBas > yHaut)
                    {
                        float yTemp = yBas;
                        yBas = yHaut;
                        yHaut = yTemp;
                    }
                    yBas += c.hauteurPerso / 2;
                    yHaut -= c.hauteurPerso / 2;

                    if (chemin[i + 1] != null)
                    {
                        if (chemin[i + 1].hauteur == Segment.HauteurSegment.SEG_HAUT)
                        {
                            if (chemin[i + 1].cote != Segment.CoteSegment.SEG_NONE)
                            {
                                chemin[i + 1].pointOpti = new Vector3(chemin[i + 1].p1.x, yBas, 0);
                                chemin[i + 1].pointSousOpti = new Vector3(chemin[i + 1].p1.x, (yBas * poidsOpti + chemin[i + 1].milieu.y * poidsMilieu), 0);
                            }
                            else
                                chemin[i + 1].pointSousOpti = new Vector3(chemin[i + 1].p1.x, chemin[i].pointSousOpti.y, 0);
                        }





                        else if (chemin[i + 1].hauteur == Segment.HauteurSegment.SEG_BAS)
                        {
                            if (chemin[i + 1].cote != Segment.CoteSegment.SEG_NONE)
                            {
                                chemin[i + 1].pointOpti = new Vector3(chemin[i + 1].p1.x, yHaut, 0);
                                chemin[i + 1].pointSousOpti = new Vector3(chemin[i + 1].p1.x, (yHaut * poidsOpti + chemin[i + 1].milieu.y * poidsMilieu), 0);
                            }
                            else
                                chemin[i + 1].pointSousOpti = new Vector3(chemin[i + 1].p1.x, chemin[i].pointSousOpti.y, 0);
                        }

                        else
                            chemin[i + 1].pointSousOpti = new Vector3(chemin[i + 1].p1.x, (chemin[i].pointSousOpti.y * poidsOpti + chemin[i + 1].milieu.y * poidsMilieu), 0);
                    }
                }
            }

            if (c2 != null)
                Temp.Add(cheminTemp);
        }
       if(c2 != null)
        foreach (List<Segment> chemin in Temp)
        {
            for (int i = 0; i < chemin.Count - 1; i++)
            {
               if (chemin[i + 1].pointSousOpti.y == 0)
                {
                    if (chemin[0].pointSousOpti.y == 0)
                    {
                        if (c2 != null)
                           chemin[0].pointSousOpti = c2.p.PathSegments[0][c2.p.PathSegments[0].Count - 1].pointSousOpti;
                        else
                            chemin[0].pointSousOpti = chemin[0].milieu;
                    }
                    chemin[i + 1].pointSousOpti = chemin[i + 1].milieu;

                    float yBas = chemin[i + 1].p1.y;
                    float yHaut = chemin[i + 1].p2.y;

                    if (yBas > yHaut)
                    {
                        float yTemp = yBas;
                        yBas = yHaut;
                        yHaut = yTemp;
                    }
                    yBas += c.hauteurPerso / 2;
                    yHaut -= c.hauteurPerso / 2;

                    if (chemin[i + 1] != null)
                    {
                        if (chemin[i + 1].hauteur == Segment.HauteurSegment.SEG_HAUT)
                        {
                            if (chemin[i + 1].cote != Segment.CoteSegment.SEG_NONE)
                            {
                                chemin[i + 1].pointOpti = new Vector3(chemin[i + 1].p1.x, yBas, 0);
                                chemin[i + 1].pointSousOpti = new Vector3(chemin[i + 1].p1.x, (yBas * poidsOpti + chemin[i + 1].milieu.y * poidsMilieu), 0);
                            }
                            else
                                chemin[i + 1].pointSousOpti = new Vector3(chemin[i + 1].p1.x, chemin[i].pointSousOpti.y, 0);
                        }





                        else if (chemin[i + 1].hauteur == Segment.HauteurSegment.SEG_BAS)
                        {
                            if (chemin[i + 1].cote != Segment.CoteSegment.SEG_NONE)
                            {
                                chemin[i + 1].pointOpti = new Vector3(chemin[i + 1].p1.x, yHaut, 0);
                                chemin[i + 1].pointSousOpti = new Vector3(chemin[i + 1].p1.x, (yHaut * poidsOpti + chemin[i + 1].milieu.y * poidsMilieu), 0);
                            }
                            else
                                chemin[i + 1].pointSousOpti = new Vector3(chemin[i + 1].p1.x, chemin[i].pointSousOpti.y, 0);
                        }

                        else
                            chemin[i + 1].pointSousOpti = new Vector3(chemin[i + 1].p1.x, (chemin[i].pointSousOpti.y * poidsOpti + chemin[i + 1].milieu.y * poidsMilieu), 0);
                    }
                }
            }
        }

        foreach (List<Segment> t in Temp)
            if(!c.p.PathSegments.Contains(t))
                c.p.PathSegments.Add(t);
        */
    }


    public void LisserCheminsForRealNoBuilding(List<Segment> chemin)
    {
        for(int i=1;i< chemin.Count-1; i++)
        {
            chemin[i].pointSousOpti.y = (chemin[i - 1].pointSousOpti.y * 0.5f + chemin[i].pointSousOpti.y + 0.5f * chemin[i + 1].pointSousOpti.y) / 2;
        }
    }



        public void CreerToutChemin(Chunk c1, Chunk c2 = null)
    {
        if (c2 != null)
        {
            c2.p.PathSegments.Clear();
            CreerChemins(c2, true);
            foreach (List<Segment> chemin in PathSegments)
            {
                if (chemin.Count != 0)
                    c2.p.PathSegments.Add(chemin);
            }

            PathSegments.Clear();
            LisserChemins(c2, c1);

            foreach (List<Segment> chemin in c2.p.PathSegments)
                LisserCheminsForRealNoBuilding(chemin);


        }
        else
        {

            c1.p.PathSegments.Clear();
            CreerChemins(c1, true);
            foreach(List<Segment> chemin in PathSegments)
            {
                if (chemin.Count != 0)
                    c1.p.PathSegments.Add(chemin);
            }
            PathSegments.Clear();
            LisserChemins(c1);
            foreach (List<Segment> chemin in c1.p.PathSegments)
                LisserCheminsForRealNoBuilding(chemin);
        }
        
    }

}
