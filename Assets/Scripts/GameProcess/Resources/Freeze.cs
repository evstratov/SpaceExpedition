using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freeze : MonoBehaviour
{
    GameObject rocket;
    Vector3 size;
    GameObject freezePanel;
    RocketFly rocketScript;
    // Start is called before the first frame update
    void Start()
    {
        freezePanel = GameObject.FindWithTag("FreezePanel");
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
            rocketScript.Invoke("Freeze", 0f);*/

            freezePanel.GetComponent<FreezePanel>().fillArea += 1f;
            Destroy(gameObject);
        }
    }
    void Update()
    {
        if (rocket == null)
            rocket = GameObject.FindWithTag("Rocket");

        size = GetComponent<Collider>().bounds.size;
        if (rocket != null && rocket.transform.position.y > transform.position.y + size.y)
        {
            Destroy(gameObject);
        }

    }
}
