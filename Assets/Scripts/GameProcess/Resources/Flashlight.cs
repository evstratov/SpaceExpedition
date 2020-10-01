using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    GameObject rocket;
    Vector3 size;
    RocketFly rocketScript;
    // Start is called before the first frame update
    void Start()
    {
        rocket = GameObject.FindWithTag("Rocket");
        size = GetComponent<Collider>().bounds.size;
        rocketScript = rocket.GetComponent<RocketFly>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rocket.transform.position.y > transform.position.y + size.y)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Rocket")
        {
           StartCoroutine(rocketScript.AddFlashlight(0.5f));

            Destroy(gameObject);
        }
    }
}
