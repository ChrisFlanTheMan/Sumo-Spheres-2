using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float worldRotationSpeed = 15;//degrees per second
        
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        transform.Rotate(0, worldRotationSpeed * Time.deltaTime, 0 ); //rotates 50 degrees per second around z axis
    }
}
