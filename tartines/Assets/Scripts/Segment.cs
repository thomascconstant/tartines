using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Segment
{
    public Vector3 p1;
    public Vector3 p2;
    public LineDrawer l;
    public Segment nextUp;
    public Segment nextDown;
    public Segment nextMilieu;
    public Vector3 milieu;
    public float taille;
    public float distanceObs;
    public float distanceAParcourir;
    public float difficulte;

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

    public Segment(HauteurSegment h, CoteSegment c)
    {
        hauteur = h;
        cote = c;

        p1 = new Vector3();
        p2 = new Vector3();
        milieu = new Vector3();
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
        if (Mathf.Abs(s.p1.x - p1.x) < 0.00001 && Mathf.Abs(s.p1.y - p1.y) < 0.00001 &&
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

    public static Vector3 GetMilieu(Segment seg)
    {
        if (seg.p1.y > seg.p2.y)
            seg.milieu.Set(seg.p1.x, (seg.p1.y + seg.p2.y) / 2, seg.p1.z);
        else
            seg.milieu.Set(seg.p1.x, (seg.p1.y + seg.p2.y) / 2, seg.p1.z);

        return seg.milieu;
    }


}



