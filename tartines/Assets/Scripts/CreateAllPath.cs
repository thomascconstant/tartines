using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateAllPath{
    

    // Use this for initialization
    public void CreerToutChemin(Chunk c1, Chunk c2 = null)
    {
        Path p = new Path();
        if (c2 != null)
        {
            c2.segments[0].pointSousOpti.y = c1.p.PathSegments[0][c1.p.PathSegments.Count - 1].pointSousOpti.y;
            p.CreerChemins(c2);
            c2.segments[0].pointSousOpti.y = c1.p.PathSegments[1][c1.p.PathSegments.Count - 1].pointSousOpti.y;
            p.CreerChemins(c2);


        }
        else
        {
            p.CreerChemins(c1);
        }
        
	}
}
