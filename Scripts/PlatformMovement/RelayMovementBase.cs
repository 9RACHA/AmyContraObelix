using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RelayMovementBase : MonoBehaviour {
    public abstract void RelayToken(Vector3 velocity);

    public float period;

    protected float cycleTime;
    //Función cadrada que varía entre -1 y 1 con un período de 1
    protected float SquareWave(float t) {
        return Mathf.Clamp(PingPong(t) * 100000f - 50000f, -1f, 1f);
    }

    //Función SmoothStep con periodo 1 y que devuelve un valor entre 0 y 1
    protected float SmoothStep(float t) {
        //devolvemos un valor usando Mathf.SmoothStep y la versión local de PingPong
        return Mathf.SmoothStep(0f, 1f, Mathf.Clamp(PingPong(t), 0f, 1f));
    }

    //función PingPong con periodo 1 y que devuelve un valor entre 0 y 1
    protected float PingPong(float t) {
        //devolvemos un valor calculado con Mathf.PingPong
        return Mathf.PingPong(t*2, 1f);
    }

    protected void AddCycleTime() {
        cycleTime += Time.deltaTime;
        if(cycleTime > period) {
            cycleTime -= period;
        }
    }

    protected bool IsInPhasePoint(float phasePoint) {
        return period * phasePoint - cycleTime >=0 && period * phasePoint - cycleTime <= 1.2f*Time.deltaTime;
    }
}
