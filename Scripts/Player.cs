using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour {
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float freeMovementSpeed = 3.5f;
    private float pushSpeed = 2f;
    private float playerSpeed;
    private float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;

    private float pickedObjectSpeed = 1.5f;
    private float pickedObjectLowPosition = 0.8f;
    private float pickedObjectHighPosition = 1.2f;
    private bool pickedObjectTooHigh = false;

    private PlayerState state;
    private Animator animator;
    private Transform cameraTrasform;

    private bool autoplay = false;
    private Vector3 targetDestination;

    private Transform pickedObject;

    private void Start() {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        SetState(PlayerState.Idle);
        cameraTrasform = Camera.main.transform;        
    }

    void Update() {
        if(autoplay) {
            Autoplay();
        }

        if(GameManager.instance.LevelLocked) {           
            return;
        }
        groundedPlayer = controller.isGrounded;

        playerSpeed = freeMovementSpeed;
        if(state == PlayerState.Push) {
            playerSpeed = pushSpeed;
        }
        
        if (groundedPlayer && playerVelocity.y < 0) {
            //playerVelocity.y = 0f;
        }

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        //Transformamos el movimiento en la direccion de la cámara
        move = cameraTrasform.forward * move.z + cameraTrasform.right * move.x;
        move.y = 0;
        move = move.normalized;


        if(pickedObjectTooHigh) {
            move = Vector3.zero;
        }
        Vector3 displacement = move * Time.deltaTime * playerSpeed;
        controller.Move(displacement);
        CheckForPushables(displacement);

        if (move != Vector3.zero) {
            gameObject.transform.forward = move;
            //SetState(PlayerState.Run);
        } else {
            //SetState(PlayerState.Idle);
        }

        if(Input.GetButtonDown("Interaction") && groundedPlayer) {
            PickObject();
        }

        if(Input.GetButton("PushUp")) {
            MovePickedObject(Vector3.up);
        }
        if(Input.GetButton("PushDown")) {
            MovePickedObject(Vector3.down);
        }

        if(pickedObjectTooHigh) {
            //Si Amy ha elevado el objeto que tiene agarrado, no puede moverse ni saltar,
            //y solo puede ejecutar la animación idle
            SetState(PlayerState.Idle);
        } else {
            if (Input.GetButtonDown("Jump") && groundedPlayer && state != PlayerState.Push) {
                //playerVelocity.y = 0f;
                Debug.Log("Salta Amy, salta");
                playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
                SetState(PlayerState.Jump);
            } else if(groundedPlayer) {
                if(move == Vector3.zero) {
                    SetState(PlayerState.Idle);
                } else if(state != PlayerState.Push) {
                    SetState(PlayerState.Run);
                }
            }
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
  
        if(playerVelocity.y < 0 && !  groundedPlayer) {
            SetState(PlayerState.Fall);
        }

        controller.Move(playerVelocity * Time.deltaTime);
    }

    private void Autoplay() {
        float distanceToTarget = (targetDestination - transform.position).magnitude;
        Vector3 move = (targetDestination - transform.position) / distanceToTarget;
        Vector3 displacement = move * Time.deltaTime * playerSpeed;
        controller.Move(displacement);

        if(distanceToTarget < 0.08f) {
            autoplay = false;
            SetState(PlayerState.Idle);
        }
    }

    public void ActivateAutoplay(Vector3 targetDestination) {
        this.targetDestination = targetDestination;
        autoplay = true;
    }

    private void CheckForPushables(Vector3 displacement) {
        if( ! groundedPlayer || displacement == Vector3.zero) {
            return;
        }
        //public static bool Raycast(Vector3 origin, Vector3 direction, out RaycastHit hitInfo, float maxDistance, int layerMask, QueryTriggerInteraction queryTriggerInteraction); 
        RaycastHit hit;
        if(Physics.Raycast(transform.position + Vector3.up + 0.1f*transform.forward, transform.forward, out hit, 0.5f)) {
            if(hit.collider.gameObject.CompareTag("Pushable")) {
                hit.collider.GetComponent<Pushable>().Push(displacement);
                SetState(PlayerState.Push);
            }
        } else {
            if(state == PlayerState.Push && displacement.sqrMagnitude > 0.0001f) {
                SetState(PlayerState.Run);
            } 
        }
    }

    private void MovePickedObject(Vector3 direction) {
        if(pickedObject == null) {
            return;
        }
        if(direction.y < 0 && pickedObject.localPosition.y > pickedObjectLowPosition) {
            pickedObject.Translate(direction * pickedObjectSpeed * Time.deltaTime);
        } else if(direction.y > 0 && pickedObject.localPosition.y < pickedObjectHighPosition) {
            pickedObject.Translate(direction * pickedObjectSpeed * Time.deltaTime);
        }
        if(pickedObject.localPosition.y > pickedObjectLowPosition) {
            pickedObjectTooHigh = true;
        } else {
            pickedObjectTooHigh = false;
        }
    }

    private void PickObject() {
        if(pickedObject != null) {
            //Liberamos el objeto
            pickedObject.GetComponent<Rigidbody>().isKinematic=false;
            pickedObject.parent = null;
            pickedObject = null;
            pickedObjectTooHigh = false;
        } else {
            //Miramos se temos un obxecto Pickeable ó alcance
            pickedObject = SearchPickeableObject();
            //Si encontramos algo, lo cogemos
            if(pickedObject != null) {
                Debug.Log("Pickable a la vista!");
                pickedObject.parent = transform;
                pickedObject.GetComponent<Rigidbody>().isKinematic=true;
                pickedObject.localPosition = new Vector3 (0, pickedObjectLowPosition, 0.4f);
                pickedObject.localRotation = Quaternion.identity;
            } else {
                Debug.Log("Nada que coger");
            }
        }
    }

    private Transform SearchPickeableObject() {
        RaycastHit hit;
        if(Physics.Raycast(transform.position + 0.1f*transform.forward + 0.7f*transform.up, transform.forward, out hit, 0.5f )) {
            //Comprobamos si el objeto es Pickable
            if(hit.collider.gameObject.CompareTag("Pickable")) {
                return hit.collider.transform;
            }
        }
        if(Physics.Raycast(transform.position + 0.1f*transform.forward + 0.35f*transform.up, transform.forward, out hit, 0.5f )) {
            //Comprobamos si el objeto es Pickable
            if(hit.collider.gameObject.CompareTag("Pickable")) {
                return hit.collider.transform;
            }
        }
        return null;
    }

    private void SetState(PlayerState newState) {
        if(state != newState ) {
            animator.ResetTrigger("Idle");
            animator.ResetTrigger("Run");
            animator.ResetTrigger("Jump");
            animator.ResetTrigger("Fall");
            animator.ResetTrigger("Push");
            state = newState;
            animator.SetTrigger($"{newState}");
        }
    }

    void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Platform")) {
            transform.parent = other.transform;
        }
    }

    void OnTriggerExit(Collider other) {
        if(other.gameObject.CompareTag("Platform")) {
            transform.parent = null;
        }
    }
}

public enum PlayerState {
    Idle,
    Run,
    Jump,
    Fall,
    Push
}
