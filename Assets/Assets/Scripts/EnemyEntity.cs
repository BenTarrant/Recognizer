using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyEntity : MonoBehaviour
{

    public float Health; // all enemies have health, so this public float is shared
    public GameObject Deresolution; // all enemies instantiate the desresolution protocol upon death so this is shared
    public GameObject Rifle; //allows certain enemy types to instantiate the rifle weapon upon death
    public NavMeshAgent agent; // provides a public reference to the nav mesh for all enemies
    [HideInInspector] public Transform target; //Hides in the inspector so it isn't overidden by each enemy's start function but remains public so it can still be called
    private AudioSource SourceAudio; // reference for audio source component in IDE
    public AudioClip EnemyHit; // reference for audio clip of enemies getting hit

    // Use this for initialization
    public virtual void Start()
    {
        target = FindObjectOfType<PlayerController>().transform; //allows all iherited classes to define the player's transform as their target
        GetComponent<AudioSource>(); // fetch that audio source component
    }

    public virtual void HitByRay(int vDamage) // when the NPC registers a raycast
    {
        Health -= vDamage; // lose health equal to damage received
        AudioSource.PlayClipAtPoint(EnemyHit, transform.position); // play the defined audio clip

        if (Health <= 0) // if an enemy drops to 0 health
        {
            Instantiate(Deresolution, transform.position + (transform.up * 1), transform.rotation); //instatiate the deresolution protocol at game object location
            if ((gameObject.GetComponent("BruteEnemy") as BruteEnemy) != null) // if the enemy has the Brute Enemy component
            {
                Instantiate(Rifle, transform.position + (transform.up * 1), transform.rotation); //instatiate a rifle if a floating stance
                Destroy(gameObject); //destroy the object this is attached to
            }

            //this allows for the option to add multiple checks based on component to decide if certain enemy types will drop different objects upon death

            else
            {
                Destroy(gameObject); //destroy the object this is attached to
            }
                
        }
    }
}
