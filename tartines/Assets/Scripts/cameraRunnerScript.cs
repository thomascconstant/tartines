using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraRunnerScript : MonoBehaviour {

	// Update is called once per frame
	void Update () {

        var tCameraPosn = transform.localPosition;
        transform.position += (Vector3.right * Time.deltaTime * 10);

    }
}
