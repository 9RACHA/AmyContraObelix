using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* 
  Script que comprueba si hay un obstáculo entre la cámara y Amy y de haberlo
  intenta volverlo transparente buscando un componente ObstacleTransparency 
  para hacer el trabajo.
*/
public class CameraTransparency : MonoBehaviour {
    public Transform lookPoint;
    ObstacleTransparent ot;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update() {
        RaycastHit hit;
        Vector3 lookDirection = lookPoint.position - transform.position;
        if(Physics.Raycast(transform.position, lookDirection, out hit, lookDirection.magnitude)) {
            ObstacleTransparent newOt = hit.collider.GetComponent<ObstacleTransparent>();
            if(newOt == null || newOt != ot) {
                ot?.GoTransparent(false);
                ot = newOt;
                ot?.GoTransparent(true);
            }
            // if(ot != null) {
            //     Debug.Log("ObstacleTransparent encontrado");
            //     ot.GoTransparent(true);
            // }
        } else if(ot != null) {
            ot.GoTransparent(false);
            ot = null;
        }
        
    }
}
