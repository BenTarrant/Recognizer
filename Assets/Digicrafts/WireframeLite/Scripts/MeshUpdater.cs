using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Digicrafts.WireframePro;

[ExecuteInEditMode]
public class MeshUpdater : MonoBehaviour {

// #if UNITY_EDITOR
	// Use this for initialization
	void Start () {
	
		
		MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
		if(meshFilter && meshFilter.sharedMesh){
			WireframeShaderUtils.updateMesh(meshFilter.sharedMesh);
		}

	}
// #endif
	
}
