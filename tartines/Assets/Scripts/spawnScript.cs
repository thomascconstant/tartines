using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnScript : MonoBehaviour {

    public GameObject[] obj;
    public float spawnMin = 2f;
    public float spawnMax = 3f;

    float x;
    float y;
    Vector2 position;

    // Use this for initialization
    void Start () {

        Spawn();

	}
	
    void Spawn() {
        /*
        x = Random.Range(-130, 130);
        y = Random.Range(-135, 50);
        position = new Vector2(x, y);
        transform.position = position;
        */

        Instantiate(obj[Random.Range(0, obj.GetLength(0))], position, Quaternion.identity);
        Invoke("Spawn", Random.Range(spawnMin, spawnMax));

    }

}
