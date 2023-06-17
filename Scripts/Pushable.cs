using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pushable : MonoBehaviour {
    public List<Transform> collisionCheckPoints;

    public void Push(Vector3 displacement) {
        //Chequeamos que no haya nada delante del menhir en la dirección
        //y distancia de movimiento
        if(GetComponent<Rigidbody>() == null) {
            if(CheckClearance(displacement)) {
                transform.Translate(displacement, Space.World);
            }
        } else {
            transform.Translate(displacement, Space.World);
        }
    }

    private bool CheckClearance(Vector3 displacement) {
        RaycastHit hit;
        foreach(Transform checkPoint in collisionCheckPoints) {
            
            if(Physics.Raycast(checkPoint.position, displacement, out hit, displacement.magnitude)){
                //Comprobamos que el collider detectado no sea el del propio menhir
                if( ! hit.collider.gameObject.Equals(gameObject)) {
                    return false;
                }
            }
        }
        return true;
    }
}

