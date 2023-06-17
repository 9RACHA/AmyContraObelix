using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour {
    public List<string> activeTags;
    public PresenceDetector detector;
    protected bool doorMoving = false;

    protected float timeOffset;
    protected float timeOut;
    // Start is called before the first frame update
    protected void Start() {
        timeOffset = 0;
        timeOut = 0;
        if(detector != null) {
            detector.OnGameObjectEntered += OnObjectEntered;
            detector.OnGameObjectExited += OnObjectExited;
        }        
    }

    // Update is called once per frame
    void Update() {
        
    }

    protected virtual void OnObjectEntered(string detectorIdentifierTag, GameObject detectedObject) {
        if(activeTags.Contains(detectedObject.tag)) {
            doorMoving = true;
            timeOffset += Time.time - timeOut;
        }
    }

    protected virtual void OnObjectExited(string detectorIdentifierTag, GameObject detectedObject) {
        if(activeTags.Contains(detectedObject.tag)) {
            doorMoving = false;
            timeOut = Time.time;  
        }
    }
}
