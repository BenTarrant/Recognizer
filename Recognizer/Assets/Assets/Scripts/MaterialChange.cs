using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialChange : MonoBehaviour {

    public Material[] materials; // creates a public array for the materials in the IDE

    void Start()
    {
        //Fetch the Material from the Renderer of the GameObject
        materials = GetComponent<Renderer>().materials;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))//
        {
            materials[1].color = Color.cyan;//change the colour of the material [1] in the array to Cyan
        }
    }

 }
