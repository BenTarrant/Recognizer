using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {

    public NavMeshAgent agent;
    public Transform target;

	// Use this for initialization
	void Start () {

        
	}
	
	// Update is called once per frame
	void Update () {

            agent.SetDestination(target.transform.position);
    }
}
