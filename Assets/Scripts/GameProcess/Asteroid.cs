using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    static float globalGravity = 98.1f;
    float scale;
    GameObject rocket;
    Rigidbody rigidbody;
    Vector3 rotation;
    Vector3 speed;
    float speedRotate;
    float speedY;


    public float radius = 5.0F;
    public float power = 20.0F;

    // Start is called before the first frame update
    void Start()
    {
        scale = Random.Range(3, 6);
        transform.localScale = new Vector3(scale, scale, scale);

        /*Vector3 explosionPos = transform.position;
        explosionPos.y += 10;
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
                rb.AddExplosionForce(power, explosionPos, radius, 3.0F);
        }*/

        
        
        rigidbody = GetComponent<Rigidbody>();
        rotation = new Vector3(Random.Range(0, 5), Random.Range(0, 5), Random.Range(0, 5));
        speedRotate = Random.Range(80, 500);
        //speedY = Random.Range(10, 20);
        rigidbody.AddForce(new Vector3(0, Random.Range(-1000, -300), 0)* globalGravity, ForceMode.Impulse);
    }

   // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(rotation, speedRotate * Time.deltaTime);
        /*speed = transform.position;
        speed.y -= speedY;
        transform.position = speed;*/

        //transform.position = Vector3.MoveTowards(transform.position, speed, 1);
        if (rocket == null)
            rocket = GameObject.FindWithTag("Rocket");

        if (rocket != null)
        {
            if (rocket.GetComponent<RocketFly>().isShieldActive)
            {
                GetComponent<Collider>().isTrigger = true;
            }
            else
            {
                GetComponent<Collider>().isTrigger = false;
            }
            if (rocket.transform.position.y > transform.position.y)
            {
                Destroy(gameObject);
            }
        }
    }
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Rocket")
        {
            rocket.GetComponent<RocketFly>().Invoke("GameOver", 0f);
        }
    }
}
