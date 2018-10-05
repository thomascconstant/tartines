using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class detectCollision: MonoBehaviour {

    public List<Chunk> listChunk = Generator.chunks;
   // public List<LineDrawer> listObstacle = Path.PathView;

    void OnCollisionEnter(Collision col)
    {
        Debug.Log(col.gameObject.name);
        if (col.gameObject.tag == "Collider")
        {
            Destroy(col.gameObject);
        }
    }

    // Update is called once per frame
    void FixedUpdate () {




        bool collide = false;

        //S'il a touché un obstacle
        foreach (Chunk c in listChunk)
        {
            foreach (Obstacle o in c.obstacles)
            {
                for (int i = 0; i < o.sommets.Length; i++)
                {
                    /*if (Mathf.Abs(this.transform.position.x - o.sommets[i].x) < c.largeurMax)
                    {*/
                    Vector3 s1 = o.sommets[i];
                    Vector3 s2 = o.sommets[(i + 1) % (o.sommets.Length)];

                    float x1 = s1.x;
                    float x2 = s2.x;
                    float xTemp = 0;

                    if (x1 > x2)
                    {
                        xTemp = x1;
                        x1 = x2;
                        x2 = xTemp;
                    }

                    float y1 = s1.y;
                    float y2 = s2.y;
                    float yTemp = 0;

                    if (y1 > y2)
                    {
                        yTemp = y1;
                        y1 = y2;
                        y2 = yTemp;
                    }
                   // Debug.DrawLine(Vector3.Lerp(s1, s2, 0.1f), Vector3.Lerp(s1, s2, 0.9f), Color.red, 5f, false);
                    
                    if (x1 < this.transform.position.x && x2 > this.transform.position.x)
                    {
                       // Debug.DrawLine(this.transform.position, Vector3.Lerp(s1, s2, (this.transform.position.x % c.largeurMax )/ c.largeurMax ), Color.blue, 5f, false);

                        if (this.transform.position.y < Mathf.Lerp(y1, y2, 1 - (this.transform.position.x % c.largeurMax) / (x2 % c.largeurMax)) &&
                            this.transform.position.y > Mathf.Lerp(y1, y2, 1 - (this.transform.position.x % c.largeurMax) / (x2 % c.largeurMax)))
                        {
                            collide = true;
                            continue;
                        }

                        if(o.typeObstacle != Obstacle.TypeObstacle.OBS_TRIANGLE)
                        {
                            if (this.transform.position.y < (y1 + 0.6) && this.transform.position.y > (y1 - 0.6))
                            {
                                collide = true;
                                continue;
                            }
                        }
                    }
                    if (y1 < this.transform.position.y && y2 > this.transform.position.y)
                    {
                        if(this.transform.position.x < Mathf.Lerp(x1, x2, 1 - (this.transform.position.y % c.largeurMax) / (y2 % c.largeurMax)) &&
                            this.transform.position.x > Mathf.Lerp(x1, x2, 1 - (this.transform.position.y % c.largeurMax) / (x2 % c.largeurMax)))
                        {
                            collide = true;
                            continue;
                        }


                        if (o.typeObstacle != Obstacle.TypeObstacle.OBS_TRIANGLE)
                        {
                            if (this.transform.position.x < (x1 + 0.6) && this.transform.position.x > (x1 - 0.6))
                            {
                                collide = true;
                                continue;
                            }
                        }
                        /*if (this.transform.position.x < (x1 + 1) && this.transform.position.x > (x1 - 1))
                        {
                            collide = true;
                            continue;
                        }*/

                    }
                // }

                }
            }



        }

        if (collide)
        {
            SpriteRenderer Color = this.GetComponent<SpriteRenderer>();
            Color.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            Debug.Log("touchay");
            collide = false;
        }

    }
}
