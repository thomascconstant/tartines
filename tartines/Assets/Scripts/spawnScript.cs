using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnScript : MonoBehaviour {

    public GameObject[] obj;
    public float spawnMin = 2f;
    public float spawnMax = 3f;
    GameObject obstacle;

    float timer = 0.0f;
    float waitingTime = 0.7f;


    // Use this for initialization
    void Start()
    {
        Spawn();
    }

    void Update () {

        timer += Time.deltaTime;
        if (timer > waitingTime)
        {
            timer = 0.0f;
            DestroyGameObject(obstacle);
            Spawn();

        }

    }
	
    void Spawn() {

        float y;
        y = transform.position.y;

        float delta = Random.Range(-4f, 4f);

        Vector3 position = transform.position;
        position.y = y + delta;

        obstacle = Instantiate(obj[Random.Range(0, obj.GetLength(0))], position, Quaternion.identity);


    }

    void DestroyGameObject(GameObject obstacle)
    {
        Destroy(obstacle,2);
    }


}
