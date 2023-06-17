using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingDoorController : DoorController {
    public float rotationSpeed;

    // Start is called before the first frame update
    void Start() {
        base.Start();        
    }

    // Update is called once per frame
    void Update() {
        if(doorMoving) {
            transform.Rotate(transform.up * rotationSpeed * Time.deltaTime);
            /*
            Se podría hacer de forma similar al movimiento de las puertas
            horizontales, usando una función que devuelve el ángulo de giro absoluto
            en función del tiempo y usando Time.time - timeOffset como valor de entrada
            para esa función, pero en este caso resulta más complejo de hacer que la
            otra solución propuesta
            
            Vector3 eulerAngles = transform.localEulerAngles;
            eulerAngles.y = rotationSpeed* (Time.time-timeOffset);
            transform.localEulerAngles = eulerAngles;
            */

        }
        
    }
}
