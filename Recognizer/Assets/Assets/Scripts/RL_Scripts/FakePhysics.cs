using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

[RequireComponent(typeof(CharacterController))]
public class FakePhysics : NetworkBehaviour {   //Needed for network play

    //Get Controller at runtime
    protected CharacterController mController;

    [SerializeField] //Will Show in inspector
    protected float JumpHeight = 10.0f;

    [SerializeField] //Will Show in inspector
    protected float MoveSpeed = 1.0f;

    [SerializeField] //Will Show in inspector
    protected float RotateSpeed = 10.0f;


    [SerializeField] //Will Show in inspector
    protected float GroundDrag = 0.2f;

    [SerializeField] //Will Show in inspector
    protected float AirDrag = 0.05f;



    //Used to move and rotate the character, this iwll be applied on Update
    [SerializeField] //Will Show in inspector
    public Vector3 mVelocity = Vector3.zero;

    protected bool mJumpInput = false;

    //This only runs when its the local player
    private void Start() {
        StartPhysics();
    }


    protected virtual void StartPhysics() { }

    //Allows character to be moved by derived class
    protected virtual void UpdatePhysicsInput() { }

    // Update is called once per frame
    void Update() {
        if (isLocalPlayer) {
            LocalPlayerMove();
        }
    }

    private void LocalPlayerMove() {
        UpdatePhysicsInput();
        if (mJumpInput) {
            mVelocity.y += -Physics.gravity.y * JumpHeight;
        }
        if (!mController.isGrounded) {
            mVelocity += Physics.gravity;
        }
        ApplyDrag();
        mController.Move(mVelocity * Time.deltaTime);
    }

    //Apply air or ground drag
    protected void ApplyDrag() {
        if (mController.isGrounded) {
            mVelocity.x -= mVelocity.x * GroundDrag;      //Slow Down on ground
            mVelocity.z -= mVelocity.z * GroundDrag;      //Slow Down on ground
            mVelocity.y -= mVelocity.y * AirDrag;       //Slow Down in air
        } else {
            mVelocity.x -= mVelocity.x * AirDrag;      //Slow Down in air
            mVelocity.z -= mVelocity.z * AirDrag;      //Slow Down in air
            mVelocity.y -= mVelocity.y * AirDrag;      //Slow Down in air
        }
    }

    protected virtual void    GotHit(GameObject vGO) {
        Debug.LogFormat("Hit {0}",vGO.name);
    }

    private void OnTriggerEnter(Collider other) {
        if(isServer) {
            GotHit(other.gameObject);
        }
    }
}
