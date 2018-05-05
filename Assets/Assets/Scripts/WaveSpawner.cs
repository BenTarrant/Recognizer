using UnityEngine;
using System.Collections;

public class WaveSpawner : MonoBehaviour
{
    //This allows the spawner script to be called from others
    public static WaveSpawner Spawner;


    //Enemies prefabs
    public GameObject [] Enemies; // array of enemies to be spawned


    //spawn timings and amount
    public int ToSpawn = 8; // defined int of how many enemies from array will be spawned
    public float fl_delay = 1; // spawner cooldown
    private float fl_timer;

    void Start()
    {
        fl_timer = Time.time + fl_delay; // set the timer correctly
    }


    void Update()
    {
        if (ToSpawn > 0 && fl_timer < Time.time) // if there are enemies left to spawn and the timer has passed
        {
            // Reduce the number of Objects to Spawn
            ToSpawn--;

            
            Instantiate(Enemies[Random.Range(0, Enemies.Length)], transform.position, transform.rotation); // Add enemy from array to the scene


            fl_timer = Time.time + fl_delay; // restart the timer
        }

    }

}
