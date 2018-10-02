using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParamDiff {
  
    /**
     * v: vitesse de defilement du niveau
     * h: distance que le joueur doit parcourir pour atteindre le milieu du prochain segment
     * d: distance à l'obstacle le plus proche
     * l: taille du segment, plus la taille du segment est grande, plus c'est facile ; si la taille du segment est inférieure à la taille du touch, challenge impossible
     * */
    public static float CalculDiff(Segment s)
    {
        //temps que le joueur a pour atteindre le prochain segment (= temps de défilement de la zone de jeu)
        float temps = s.distanceObs / cameraRunnerScript.vitesse;

        //le paramètre de taille du segment (compris entre 0, impossible, et 1, trop facile) est multiplié par le paramètre liant temps et hauteur => l*(h/t)
        s.difficulte = (1/s.taille) * (s.distanceAParcourir / temps);

        return s.difficulte;  

    }

    public static float CalculDiffChemin(List<Segment> seg)
    {
        float diffChemin = 0;
        foreach(Segment s in seg)
        {
            diffChemin += CalculDiff(s);
        }
        return diffChemin;
    }









}
