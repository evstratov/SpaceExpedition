using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    GameObject rocket;
    Vector3 size;
    GameObject shieldPanel;
    RocketFly rocketScript;
    // Start is called before the first frame update
    void Start()
    {
        shieldPanel = GameObject.FindWithTag("ShieldPanel");
        rocket = GameObject.FindWithTag("Rocket");
        rocketScript = rocket.GetComponent<RocketFly>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Rocket")
        {
            if (PlayerPrefs.GetString("Sound") == "On")
                rocketScript.resourceSound.Play();
            /*RocketFly rocketScript = other.GetComponent<RocketFly>();
            rocketScript.Invoke("Shield", 0f);*/
            shieldPanel.GetComponent<ShieldPanel>().fillArea += 0.5f;
            Destroy(gameObject);
        }
    }
    void Update()
    {
        size = GetComponent<Collider>().bounds.size;
        if (rocket.transform.position.y > transform.position.y + size.y)
        {
            Destroy(gameObject);
        }

    }
}
