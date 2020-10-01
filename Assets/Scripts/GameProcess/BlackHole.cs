using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackHole : MonoBehaviour
{
    Text textBlackHole;
    const float G = 67.4f;
    // smth game object 
    HashSet<Rigidbody> listToAttract;
    Rigidbody objectRigitbody;
    Rigidbody myRigitbody;
    GameObject rocket;
    GameObject camera;
    float saveRocketSpeed;
    float currentRocketSpeed;
    float scale;

    void Start()
    {
        myRigitbody = GetComponent<Rigidbody>();
        rocket = GameObject.FindWithTag("Rocket");
        camera = GameObject.FindWithTag("MainCamera");
        listToAttract = new HashSet<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (rocket.transform.position.y > transform.position.y + GetComponent<Collider>().bounds.size.y)
        {
            Destroy(gameObject);
        }
        foreach(Rigidbody objToAttract in listToAttract)
        {
            if (objToAttract != null)
            {
                Attract(objToAttract);
            }
        }

    }
    void Attract(Rigidbody rbToAttract)
    {
        Vector3 direction = myRigitbody.position - rbToAttract.position;
        if (direction != Vector3.zero)
        {
            //Debug.Log($"direction {direction}");
            float distance = direction.magnitude;
            //Debug.Log($"distance {distance}");
            //if(distance != 0 && currentRocketSpeed > 0)
                //currentRocketSpeed -= 0.0001f;
            float forceMagnitude = G * (myRigitbody.mass * rbToAttract.mass) / Mathf.Pow(distance, 2);
            //Debug.Log($"force magnitude {forceMagnitude}");
            Vector3 force;
            if (rbToAttract.tag == "Rocket")
            {
                force = direction.normalized * forceMagnitude * rocket.GetComponent<RocketFly>().speed;
            }
            else
                force = direction.normalized * forceMagnitude;
            //Debug.Log($"force {force}");
            rbToAttract.AddForce(force);
        }
    }
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "BlackHole")
            return;
        if (other.gameObject.tag == "Rocket")
        {
            rocket.GetComponent<RocketFly>().Invoke("GameOver", 0f);
        }
        else
        {
            try
            {
                listToAttract.Remove(other.gameObject.GetComponent<Rigidbody>());
            }
            finally
            {
                Destroy(other.gameObject);
            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "BlackHole")
            return;
        if (other.attachedRigidbody != null && listToAttract != null)
        {
            listToAttract.Add(other.attachedRigidbody);
        }
        if (other.gameObject.tag == "Rocket" && camera != null)
        {
            rocket.GetComponent<RocketFly>().Invoke("BlackHoleDeath", 0f);
            camera.GetComponent<CameraMoving>().isBlackHoleAttraction = true;
        }
    }
}
