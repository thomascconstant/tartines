using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour {

    public Chunk[] chunks;
    public float[] diffs;

    // Use this for initialization
    void Start() {
        chunks = new Chunk[10];
        chunks[0] = new Chunk();
        chunks[0].CreateChunk();
        chunks[0].BuildSegments();
        //chunks[0].ShowSegments();
        for (int i = 1; i < chunks.Length; i++)
        {
            chunks[i] = new Chunk();
            chunks[i].GenerateChunk(chunks[i-1].segments[chunks[i - 1].segments.Count - 1].p2, chunks[i - 1].segments[chunks[i - 1].segments.Count - 1].p1);
            chunks[i].BuildSegments();
            //chunks[i].ShowSegments();

        }



        // chunks[0].ShowSegments();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
