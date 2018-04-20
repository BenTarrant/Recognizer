using UnityEngine;
using System.Collections;

public class BillBoard : MonoBehaviour {

    public bool FaceCamera;             //Have this object face the camera


    Camera mMainCamera;


    // Use this for initialization
    void Start () {
        mMainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update () {
        if(FaceCamera) {
            transform.LookAt(transform.position + mMainCamera.transform.rotation * Vector3.forward, mMainCamera.transform.rotation * Vector3.up);        //Turn object towards camera
        }
    }
}
