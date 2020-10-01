using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddFuel : MonoBehaviour
{
    float value;
    GameObject rocket;
    Vector3 size;
    // Start is called before the first frame update
    void Start()
    {
        value = Random.Range(0.5f, 0.7f);
        rocket = GameObject.FindWithTag("Rocket");
        size = GetComponent<Collider>().bounds.size;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Rocket")
        {
            RocketFly rocketScript = other.GetComponent<RocketFly>();
            if (rocketScript.fuel + value > 1f)
                StartCoroutine(rocketScript.AddFuel(1 - rocketScript.fuel));
            //rocketScript.fuel = 1f;
            else
                StartCoroutine(rocketScript.AddFuel(value));
            //rocketScript.fuel += value;

            rocketScript.countOfFuel++;
            Destroy(gameObject);
           
        }

    }
    void LevelTwoFuel(RocketFly rocketScript)
    {
        switch (Random.Range(0, 5))
        {
            case 0:
                rocketScript.Invoke("HyperJump", 0f);
                break;
            case 1:
                rocketScript.Invoke("Shield", 0f);
                break;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (rocket.transform.position.y > transform.position.y + size.y)
        {
            Destroy(gameObject);
        }

    }
}
