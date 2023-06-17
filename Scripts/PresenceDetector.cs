using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresenceDetector : MonoBehaviour {
    public delegate void OnGameObjectDetectedDelegate(string detectorIdentifierTag, GameObject detectedObject);
    public OnGameObjectDetectedDelegate OnGameObjectEntered;
    public OnGameObjectDetectedDelegate OnGameObjectExited;


    public string identifierTag;

    void OnTriggerEnter(Collider other) {
        
        if(OnGameObjectEntered != null) {
            Debug.Log("PresenceDetector.OnTriggerEntered invocado");
            OnGameObjectEntered(identifierTag, other.gameObject);
        }
    }

    void OnTriggerExit(Collider other) {
        Debug.Log("PresenceDetector.OnTriggerExited " + identifierTag);
        if(OnGameObjectExited != null) {
            OnGameObjectExited(identifierTag, other.gameObject);
        }
    }


}
