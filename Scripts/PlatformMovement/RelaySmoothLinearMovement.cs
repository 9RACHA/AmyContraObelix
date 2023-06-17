using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelaySmoothLinearMovement : RelayMovementBase {
    public RelayMovementBase nextRelay;
    public Transform startPoint;
    public Transform endPoint;
    

    public bool startMoving;
    private bool hasToken;

    
    // Start is called before the first frame update
    void Start() {
        cycleTime = 0f;
        hasToken = startMoving;       
    }

    // Update is called once per frame
    void Update()  {
        if( ! hasToken) {
            return;
        }
        
        //Mediante Vector3.Lerp y SmoothStep, movemos el objeto entre los puntos
        //startPoint y endPoint, tardando period segundos en la vuelta completa
        if(period != 0f) {
            AddCycleTime();
            
            transform.position = Vector3.Lerp(startPoint.position, endPoint.position, SmoothStep(cycleTime/period));        
            //En la mitad del recorrido paramos y le damos el relevo a otro componente
            if(IsInPhasePoint(1f/4f)) {
                hasToken = false;
                if(nextRelay != null) {
                    nextRelay.RelayToken(Vector3.zero);
                }
            }
        }
    }



    public override void RelayToken(Vector3 velocity) {
        //TODO retomar o movemento
        hasToken = true;
        cycleTime = 0.75f * period;
    }
}
