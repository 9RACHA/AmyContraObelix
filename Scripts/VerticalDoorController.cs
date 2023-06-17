using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalDoorController : DoorController {
    public float amplitude;
    public float period;
    // Start is called before the first frame update
    void Start() {
        base.Start();
    }

    // Update is called once per frame
    void Update() {
        if(doorMoving) {
            Vector3 position = transform.position;
            position.y = DoorPosition(Time.time - timeOffset);
            transform.position = position;

        }
        
    }

    private float DoorPosition(float time) {
        return Mathf.SmoothStep(0, amplitude, Mathf.PingPong(2 * time / period, 1)) + 1;
    }
}
