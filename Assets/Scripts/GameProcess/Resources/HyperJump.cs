using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HyperJump : MonoBehaviour
{
    GameObject rocket;
    Vector3 size;
    GameObject hyperJumpPanel;
    RocketFly rocketScript;
    // Start is called before the first frame update
    void Start()
    {
        hyperJumpPanel = GameObject.FindWithTag("HyperJumpPanel");
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
            rocketScript.Invoke("HyperJump", 0f);*/

            hyperJumpPanel.GetComponent<HyperJumpPanel>().fillArea += 0.25f;

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
