using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelayCircularMovement : RelayMovementBase {

    public RelayMovementBase nextRelayForward;
    public RelayMovementBase nextRelayBackward;
    public Transform centerPoint;
    public Transform startPoint;
    public Transform endPoint;

    private Vector3 rotationAxis;

    private int movementDirection = 1;

    private float rotationSpeed;
    // Start is called before the first frame update
    
    public bool startMoving;
    private bool hasToken;
    void Start() {
        hasToken = startMoving;
        cycleTime = period / 4f;
        float amplitudeAngle = Vector3.Angle(startPoint.position - centerPoint.position, 
                                             endPoint.position - centerPoint.position);
        rotationAxis = Vector3.Cross(startPoint.position - centerPoint.position,
                                             endPoint.position - centerPoint.position).normalized;
        if(period != 0f) {
            rotationSpeed = 2 * amplitudeAngle / period;
        }
        transform.position = startPoint.position;
    }

    // Update is called once per frame
    void Update() {
        if( ! hasToken) {
            return;
        }
        if(period != 0f) {
            AddCycleTime();
            float swValue = SquareWave(cycleTime / period);
            transform.RotateAround(centerPoint.position, 
                                   rotationAxis,
                                   swValue * rotationSpeed *  Time.deltaTime);

            //Comprobamos is hemos cambiado de dirección en este frame
            //comparando los signos de swValue y movementDirection
            if(swValue * movementDirection < 0) {
                movementDirection *= -1;
                if(movementDirection == 1) {
                    GiveToken(nextRelayBackward);
                } else {
                    GiveToken(nextRelayForward);
                }
            }
/*
            if(IsInPhasePoint(3f/4f)) {
                Debug.Log("RelayCircularMovement Cambio de sentido de movemento arriba");
                
            } else if (IsInPhasePoint(1f/4f) ) {
                Debug.Log("RelayCircularMovement Cambio de sentido de movemento abaixo");
                
            }
*/
        }
    }

    
    public override void RelayToken(Vector3 velocity) {
        
        hasToken = true;
    }

    private void GiveToken(RelayMovementBase nextRelay) {
        if(nextRelay != null) {
            hasToken = false;
            nextRelay.RelayToken(Vector3.zero);
        }
    }

}
