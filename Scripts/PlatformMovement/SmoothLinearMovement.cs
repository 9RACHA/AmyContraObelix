using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothLinearMovement : MonoBehaviour {
    public Transform startPoint;
    public Transform endPoint;
    public float period;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()  {
        
        //Mediante Vector3.Lerp y SmoothStep, movemos el objeto entre los puntos
        //startPoint y endPoint, tardando period segundos en la vuelta completa
        if(period != 0f) {
            transform.position = Vector3.Lerp(startPoint.position, endPoint.position, SmoothStep(Time.time/period));        
        }
    }


    //Función SmoothStep con periodo 1 y que devuelve un valor entre 0 y 1
    private float SmoothStep(float t) {
        //devolvemos un valor usando Mathf.SmoothStep y la versión local de PingPong
        return Mathf.SmoothStep(0f, 1f, Mathf.Clamp(PingPong(t), 0f, 1f));
    }

    //función PingPong con periodo 1 y que devuelve un valor entre 0 y 1
    private float PingPong(float t) {
        //devolvemos un valor calculado con Mathf.PingPong
        return Mathf.PingPong(t*2, 1f);
    }
}
