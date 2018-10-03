using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DotPlayerController : MonoBehaviour
{

    private Vector3 previousTouch;
    private Vector3 previousDir;
   
    public List<Chunk> listChunk = Generator.chunks;

    void FixedUpdate()
    {

        Vector3 clickPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
        bool touch = true;
        if (Input.touchSupported)
        {
            touch = false;
            if (Input.touchCount > 0)
            {
                // Get movement of the finger since last frame
                Vector2 touchPosition = Input.GetTouch(0).position;
                clickPos = new Vector3(touchPosition.x, touchPosition.y, 10);
                touch = true;
            }
        }
        clickPos = Camera.main.ScreenToWorldPoint(clickPos);

       


       if (touch)
        {
            //On compute la direction du touch
            Vector3 dirTouch = new Vector3();
            if (previousTouch.magnitude == 0)
            {
                previousDir = new Vector3();
                previousTouch = clickPos;
            }
            else
            {
                float distance = (clickPos - previousTouch).magnitude;
                if(distance > 0.2)
                {
                    dirTouch = Vector3.Slerp(previousDir, (clickPos - previousTouch).normalized, 0.5f).normalized;
                    previousDir = dirTouch;
                    previousTouch = clickPos;
                }
                
            }
            


            RaycastHit2D[] hits = Physics2D.RaycastAll(clickPos, Vector2.zero);
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider != null)
                {
                    bool collide = false;

                    //S'il a touché un obstacle
                    foreach (Chunk c in listChunk)
                    {
                        foreach (Obstacle o in c.obstacles)
                        {
                            for (int i = 0; i < o.sommets.Length; i++)
                            {
                               
                                if (Mathf.Abs(this.transform.position.x - o.sommets[i].x) < c.largeurMax / 2)
                                {
                                    float x1 = o.sommets[i].x;
                                    float x2 = o.sommets[i + 1 % o.sommets.Length - 1].x;
                                    float xTemp = 0;

                                    if (x1 > x2)
                                    {
                                        x1 = xTemp;
                                        x1 = x2;
                                        x2 = xTemp;
                                    }

                                    if (x1 < this.transform.position.x && x2 > this.transform.position.x)
                                    {
                                        collide = true;
                                        continue;
                                    }

                                    float y1 = o.sommets[i].y;
                                    float y2 = o.sommets[i + 1 % o.sommets.Length - 1].y;
                                    float yTemp = 0;

                                    if (y1 > y2)
                                    {
                                        y1 = yTemp;
                                        y1 = y2;
                                        y2 = yTemp;
                                    }

                                    if (y1 < this.transform.position.y && y2 > this.transform.position.y)
                                    {
                                        collide = true;
                                        continue;
                                    }
                                }

                            }
                        }


                              
                    }
                    
                    if (collide)
                    {
                        SpriteRenderer Color = this.GetComponent<SpriteRenderer>();
                        Color.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
                        Debug.Log("touchay");
                    }

                    
                }
            }
        }
    }
     

}

