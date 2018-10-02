using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraRunnerScript : MonoBehaviour {

    public static float vitesse = 1.0f;

    // Update is called once per frame
    void Update () {

        var tCameraPosn = transform.localPosition;
        transform.position += (Vector3.right * Time.deltaTime * vitesse);

    }
}
