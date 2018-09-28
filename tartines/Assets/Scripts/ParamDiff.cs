using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParamDiff {
    
    public float hauteurTouch; //distance que le touch du joueur doit parcourir pour atteindre le milieu du prochain segment
    public float largeurSegment; //plus la largeur du segment est grande, plus c'est facile ; si la largeur du segment est inférieure à la taille du touch, challenge impossible 
    public float temps; //temps que le joueur a pour atteindre le prochain segment (= temps de défilement de la zone de jeu)

    public ParamDiff(float v, float h, float d, float l)
    {
        hauteurTouch = h;
        temps = d / v;
        largeurSegment = l;
    }









}
