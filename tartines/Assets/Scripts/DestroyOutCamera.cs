using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOutCamera : MonoBehaviour {
    public float distanceErase = 30;
	
	// Update is called once per frame
	void Update () {
		if(Camera.main.transform.position.x  - transform.position.x > distanceErase)
        {
            Destroy(this.gameObject);
        }
	}
}
