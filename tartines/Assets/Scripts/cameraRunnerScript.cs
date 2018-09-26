using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraRunnerScript : MonoBehaviour {

    public Transform player;

	// Update is called once per frame
	void Update () {

        //transform.position = new Vector3(player.position.x , 0, -10);
        // Get the camera's current position
        var tCameraPosn = transform.localPosition;
        transform.position += (Vector3.right * Time.deltaTime * 10);



        // Move the camera forward
        //transform.Translate((Vector3.forward * (Time.deltaTime * 5.0)));





    }
}
