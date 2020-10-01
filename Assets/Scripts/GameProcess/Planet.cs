using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    public float tmpScale;
    GameObject rocket;
    public Vector3 size;
    Vector3 rotation;
    float speedRotate;
    float scale;
    bool flagFirst;

    // Start is called before the first frame update
    void Start()
    {
        scale = tmpScale;
        flagFirst = true;
        scale = Random.Range(30, 50);
        transform.localScale = new Vector3(scale, scale, scale);
        rocket = GameObject.FindWithTag("Rocket");
        rotation = new Vector3(Random.Range(0,3), Random.Range(0, 3), Random.Range(0, 3));
        speedRotate = Random.Range(5, 25);
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        size = GetComponent<Collider>().bounds.size;
        transform.Rotate(rotation, speedRotate * Time.deltaTime);
        if (rocket != null)
        {
            if (rocket.transform.position.y > transform.position.y + size.y)
            {
                Destroy(gameObject);
            }
        }
    }
    void OnCollisionEnter(Collision other)
    {    
        if (other.gameObject.tag == "Rocket")
        { 
            if (rocket.GetComponent<RocketFly>().score > 0)
                rocket.GetComponent<RocketFly>().score--;

            rocket.GetComponent<RocketFly>().Invoke("GameOver", 0f);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Rocket" && flagFirst)
        {
            flagFirst = false;
            other.GetComponent<RocketFly>().Invoke("AddScore", 0f);
        }
    }
}
