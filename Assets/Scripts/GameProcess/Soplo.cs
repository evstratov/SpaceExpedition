using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soplo : MonoBehaviour
{
    Transform rocket;
    RocketFly rocketScript;
    Quaternion soploRotation;
    // Start is called before the first frame update
    void Start()
    {
        rocket = transform.parent.parent;
        rocketScript = rocket.GetComponent<RocketFly>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!rocketScript.isGameOver && rocket != null)
        {
            soploRotation = rocketScript.soploRotation;
            transform.rotation = soploRotation;
        }
    }
}
