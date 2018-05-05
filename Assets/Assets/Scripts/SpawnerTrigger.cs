using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerTrigger : MonoBehaviour {

    public GameObject [] Spawners; // array of spawners in the IDE


    private void OnTriggerEnter(Collider collider)// when the collider detects a trigger collider
    {
        if (collider.gameObject.tag == "Player") // and if that trigger collider is tagged the player
        {
            Destroy(gameObject); // destory the trigger collider

            foreach (GameObject SpawnPoints in Spawners) // and for each spawner in the specified array
            {
                SpawnPoints.SetActive(true); // set to active (mock instatiate)
            }
        }
    }
}
