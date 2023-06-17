using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalDoorController : DoorController {
    public float amplitude;
    public float period;

    // Start is called before the first frame update
    void Start() {
        base.Start();
    }

    // Update is called once per frame
    void Update() {
        if(doorMoving) {
            //Movemos a porta
            Vector3 position = transform.position;
            position.z = DoorPosition(Time.time - timeOffset + period/4);
            transform.position = position;
        }
        
    }

    private float DoorPosition(float time) {
        return Mathf.SmoothStep(-amplitude/2, amplitude/2, Mathf.PingPong(2 * time / period, 1));
    }
/*
    private void OnObjectEntered(string detectorIdentifierTag, GameObject detectedObject) {
        
    }

    private void OnObjectExited(string detectorIdentifierTag, GameObject detectedObject) {

    }
    */
}
