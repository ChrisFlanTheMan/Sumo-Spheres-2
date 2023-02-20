using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSkyDome : MonoBehaviour
{
    public float skyRotationSpeed = 10;
    public float rotationFallOff = 15;//how much the island speed is divided by for the sky speed

    // Start is called before the first frame update
    void Start()
    {

        // skyRotationSpeed = GetComponent<Rotate>().worldRotationSpeed; 
        // skyRotationSpeed = -skyRotationSpeed/rotationFallOff;
    }

    // Update is called once per frame
    void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * skyRotationSpeed);
    }
}
