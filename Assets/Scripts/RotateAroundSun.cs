using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAroundSun : MonoBehaviour
{
    public float degreesPerSecond = 45;

    private GameObject sun;

    // Start is called before the first frame update
    void Start()
    {
       sun = GameObject.Find("Sun");
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(sun.transform.position, Vector3.up, degreesPerSecond * Time.deltaTime);
    }
}
