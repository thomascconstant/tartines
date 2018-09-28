using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraRunnerScript : MonoBehaviour {

    public float vitesse = 10;

    // Update is called once per frame
    void Update () {

        var tCameraPosn = transform.localPosition;
        transform.position += (Vector3.right * Time.deltaTime * vitesse);

    }
}
