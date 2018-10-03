using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class detectCollision: MonoBehaviour {

    //public List<Chunk> listChunk = Generator.chunks;
    //public List<LineDrawer> listObstacle = Path.PathView;

    void OnCollisionEnter(Collision col)
    {
        Debug.Log(col.gameObject.name);
        if (col.gameObject.name == "LineObj")
        {
            Destroy(col.gameObject);
        }
    }

    // Update is called once per frame
    /*void FixedUpdate () {




        bool collide = false;

        //S'il a touché un obstacle
        foreach (Chunk c in listChunk)
        {
            foreach (Obstacle o in c.obstacles)
            {
                for (int i = 0; i < o.sommets.Length; i++)
                {

                    if (Mathf.Abs(this.transform.position.x - o.sommets[i].x) < c.largeurMax)
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

    }*/
}
