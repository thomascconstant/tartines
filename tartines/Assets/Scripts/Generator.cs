using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour {

    public static List<Chunk> chunks = new List<Chunk>();
    public float[] diffs;
    public float nb_chunks;

    // Use this for initialization
    void Start() {
        nb_chunks = 10;
        chunks.Add(new Chunk());
        chunks[0].CreateChunk();
        chunks[0].BuildSegments();
        for (int i = 1; i < nb_chunks; i++)
        {
            chunks.Add(new Chunk());
            for (int t = 0; t < 100; t++)
            {
                chunks[i].GenerateChunk(chunks[i - 1].segments[chunks[i - 1].segments.Count - 1].p2, chunks[i - 1].segments[chunks[i - 1].segments.Count - 1].p1);
                chunks[i].BuildSegments();
            }
        }

        foreach(Chunk c in chunks)
        {
            //c.ShowPath();
            c.ShowObstacles();

        }

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
