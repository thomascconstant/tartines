using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineDrawer
{
    private LineRenderer lineRenderer;
    private float lineSize;
    private GameObject lineObj;
    public BoxCollider2D box = new BoxCollider2D();
    public Rigidbody2D boxRigid = new Rigidbody2D();
    public PhysicsMaterial2D material = (PhysicsMaterial2D)Resources.Load<PhysicsMaterial2D>("Slippery");


    public LineDrawer(float lineSize = 0.2f)
    {
        lineObj = new GameObject("LineObj");
        lineObj.tag = "Collider";
        lineRenderer = lineObj.AddComponent<LineRenderer>();
        
        //Particles/Additive
        lineRenderer.material = new Material(Shader.Find("Hidden/Internal-Colored"));

        this.lineSize = lineSize;
    }

    public void SetParent(Transform parent)
    {
        lineObj.transform.SetParent(parent);
    }

    private void init(float lineSize = 0.2f)
    {
        if (lineRenderer == null)
        {
            GameObject lineObj = new GameObject("LineObj");
            lineObj.tag = "Collider";
            lineRenderer = lineObj.AddComponent<LineRenderer>();
            //Particles/Additive
            lineRenderer.material = new Material(Shader.Find("Hidden/Internal-Colored"));

            this.lineSize = lineSize;
        }
    }

    //Draws lines through the provided vertices
    public void DrawLineInGameView(Vector3 start, Vector3 end, Color color, Segment s1 = null, Segment s2 = null, float diff = 0)
    {

        Vector3 midPoint = (start + end) / 2.0f;

        lineObj.transform.position = midPoint;

        if (lineRenderer == null)
        {
            init(0.2f);
        }
        

        if (s1 != null)
        {
            Text Diff = lineObj.AddComponent<Text>();
            Diff.text = "Pour le segment 1: \nLe milieu est:" + s1.milieu + "\nLa taille du segment est:" + s1.taille  + "\nLa distance à parcourir est:" 
                + s1.distanceAParcourir + "\nLa distance à l'obstacle est:" + s1.distanceObs +
                "\n\nPour le segment 2: \nLe milieu est:" + s2.milieu + "\nLa taille du segment est:" + s2.taille +
                "\n\nLa difficulté de ce segment est:" + s1.difficulte + 
                "\n\nLa difficulté du chemin est:" + diff; 
        }

        //Set color
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;

        //Set width
        lineRenderer.startWidth = lineSize;
        lineRenderer.endWidth = lineSize;

        //Set line count which is 2
        lineRenderer.positionCount = 2;

        //Set the postion of both two lines
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);

        //ajout d'un collider sur le linerenderer
        BoxCollider2D box = lineObj.AddComponent<BoxCollider2D>();
        box.sharedMaterial = material;
        //box.transform.parent = lineRenderer.transform;

        //taille du collider
        float lineWidth = lineRenderer.endWidth;
        float lineLength = Vector3.Distance(start, end);
        if (end.x == start.x)
            box.size = new Vector2(lineWidth, lineLength);
        else
            box.size = new Vector2(lineLength, lineWidth);

        //rotation du collider
        if (start.x != end.x && start.y != end.y)
        {
            
            float rad = Mathf.Atan2((end.y - start.y), (end.x - start.x));
            float angle = rad * Mathf.Rad2Deg;
            //angle *= -1;
            box.transform.Rotate(0, 0, angle);

        }

        //add rigidbody
        //Rigidbody2D boxRigid = lineObj.AddComponent<Rigidbody2D>();

    }

    public void Destroy()
    {
        if (lineRenderer != null)
        {
            UnityEngine.Object.Destroy(lineRenderer.gameObject);
        }
    }
}