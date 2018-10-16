using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour {

    public static List<Chunk> chunks = new List<Chunk>();
    public float[] diffs;
    public float nb_chunks;
    public Path p = new Path();
    // Use this for initialization
    void Start() {
        nb_chunks = 20;
        Vector3 p1 = new Vector3(0,15,0);
        Vector3 p2 = new Vector3(0,5, 0);

        int seed =(int)( Random.value*99999);
        //seed = 17193;
       // seed = 80752;
        Random.InitState(seed);

        Debug.Log(seed);
              
        for (int i = 0; i < nb_chunks; i++)
        {
            chunks.Add(new Chunk());

            if (i >= 1)
            {
                p1 = chunks[i - 1].segments[chunks[i - 1].segments.Count - 1].p2;
                p2 = chunks[i - 1].segments[chunks[i - 1].segments.Count - 1].p1;
            }

            
            for (int t = 0; t < 1; t++)
            {
               /* if (i % 6 == 0)
                    chunks[i].GenerateChunkReset(p1, p2);
                    
                else*/
                chunks[i].GenerateChunk(p1, p2,5);
                chunks[i].BuildSegments();
                if (i < 1)
                    p.CreerToutChemin(chunks[i]);
                else if (chunks[i] != null)
                    p.CreerToutChemin(chunks[i - 1], chunks[i]);
            }


            
        }

        foreach(Chunk c in chunks)
        {
            c.ShowPath();
            c.ShowObstacles();
        }

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
