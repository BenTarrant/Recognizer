using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;



public class MovePlayerCharacter : FakePhysics {


    private Text  mDebugText;
    private Text  mUIPlayerName;

    [SerializeField]
    private Transform   BulletStart;

    [SerializeField]
    private GameObject  BulletPrefab;
    
    [SerializeField]
    private Text PlayerNameText;      //Link in IDE

    [SyncVar(hook = "OnUpdatePlayerName")]
    protected string mPlayerName = "No set";

    public override void OnStartClient() {
        base.OnStartClient();
        mDebugText = FindUITextByName("DebugText");
        mUIPlayerName = FindUITextByName("PlayerName");
        OnUpdatePlayerName(mPlayerName);    //Hook not called on init
     }

    Text    FindUITextByName(string vName) {
        GameObject tTextGO = GameObject.Find(vName);
        if (tTextGO != null)
        {
            return  tTextGO.GetComponent<Text>();
        }
        Debug.Log("FindUITextByName() Unable to find:" + vName);
        return null;
    }

    //This only runs when its the local player
    public override void OnStartLocalPlayer() {
        base.OnStartLocalPlayer();
        mController = GetComponent<CharacterController>();
        gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;        //Turn local player blue
        Camera.main.transform.SetParent(transform, false); //Parent Camera to Local Player
        CmdSetPlayerName(System.Environment.UserName + " NetID:" + GetComponent<NetworkIdentity>().netId.ToString());
    }


    void OnUpdatePlayerName(string vNewName) {
        Debug.Assert(PlayerNameText != null, "NameText Not linked");
        mPlayerName = vNewName;
        PlayerNameText.text = mPlayerName;
        if (mUIPlayerName != null) {
            mUIPlayerName.text = mPlayerName;
        }
        Debug.LogFormat("OnUpdatePlayerName {0}", vNewName);
    }

    [Command]
    void CmdSetPlayerName(string vNewName) {
        mPlayerName = vNewName;
        Debug.LogFormat("SetNewName {0}", vNewName);
    }

    [Command]
    void    CmdFire() {
        GameObject tGO = Instantiate(BulletPrefab, BulletStart.position,Quaternion.identity);
        tGO.GetComponent<Rigidbody>().velocity = transform.forward * 5.0f;
        NetworkServer.Spawn(tGO);
        Destroy(tGO, 2.0f);
    }



    float mFireThrottle = 0.0f;
    //Update Character movement based on control input
    protected override void UpdatePhysicsInput() {
        float   tSpeed= Input.GetAxis("Vertical") * MoveSpeed;
        transform.rotation *= Quaternion.Euler(0, GetRotationControl() * RotateSpeed, 0);
//        tVelocity.x += Input.GetAxis("Horizontal") * MoveSpeed;
        mVelocity += transform.rotation*Vector3.forward * MoveSpeed* tSpeed;
        mJumpInput = mController.isGrounded && Input.GetButton("Jump");

        if(mFireThrottle<0.0f) {
            mFireThrottle = 0.15f;
            if (Input.GetAxis("Fire1") > 0.1f) {
                CmdFire();
            }
        } else {
            mFireThrottle -= Time.deltaTime;
        }
    }
    //Get rotation either from mouse or XBox controller, not a great hack!
    float GetRotationControl() {
        //Really poor way of doing this, but Unity has no easy way to check if an axis has been set up in Input
        //It just crashes the script, this catches the crash and sues the mosue if there is no Xbox controller set up
        try {
            float tControl = Input.GetAxis("Horizontal1");      //If XBox controller has input use this
            if (Mathf.Abs(tControl) > Mathf.Epsilon) return tControl;
            else  return Input.GetAxis("Mouse X");      //If not use Mouse X
        }
        catch {
            return Input.GetAxis("Mouse X");        //If the check for the XBox axis caused an excpetion catch it and use Mouse
        }
    }


    protected override void GotHit(GameObject vGO) {
        base.GotHit(vGO);
        RpcTakeHit(vGO.name);
        Destroy(vGO);
    }

    [ClientRpc]
    void    RpcTakeHit(string vText) {
        if (mDebugText != null) {
            mDebugText.text = string.Format("Hit by {0}", vText);
        }
    }
}
