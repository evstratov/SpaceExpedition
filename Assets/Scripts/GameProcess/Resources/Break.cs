using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Break : MonoBehaviour
{
    GameObject rocket;
    Vector3 size;
    float scale;
    Vector3 rotation;
    float speedRotate;
    // Start is called before the first frame update
    void Start()
    {
        scale = Random.Range(5, 10);
        transform.localScale = new Vector3(scale, scale, scale);
        rocket = GameObject.FindWithTag("Rocket");

        rotation = new Vector3(Random.Range(0, 5), Random.Range(0, 5), Random.Range(0, 5));
        speedRotate = 10;

        size = GetComponent<Collider>().bounds.size;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotation, speedRotate * Time.deltaTime);
        
        if (rocket.transform.position.y + size.y > transform.position.y)
        {
            Destroy(gameObject);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Rocket")
        {
            rocket.GetComponent<RocketFly>().Invoke("Break", 0);
        }
    }
}
