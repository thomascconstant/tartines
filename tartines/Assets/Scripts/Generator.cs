using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour {

    public Chunk[] chunks;
    public float[] diffs;

    // Use this for initialization
    void Start() {
        chunks = new Chunk[1];
        chunks[0] = new Chunk();
        chunks[0].CreateChunk();
        chunks[0].BuildSegments();
        chunks[0].ShowSegments();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
