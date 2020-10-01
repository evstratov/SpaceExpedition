using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster : MonoBehaviour
{
    GameObject rocket;
    Vector3 size;
    GameObject boostPanel;
    RocketFly rocketScript;
    // Start is called before the first frame update
    void Start()
    {
        boostPanel = GameObject.FindWithTag("BoostPanel");
        rocket = GameObject.FindWithTag("Rocket");
        rocketScript = rocket.GetComponent<RocketFly>();
    }

    // Update is called once per frame
    void Update()
    {
        size = GetComponent<Collider>().bounds.size;
        if (rocket.transform.position.y > transform.position.y + size.y)
        {
            Destroy(gameObject);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Rocket")
        {
            if (PlayerPrefs.GetString("Sound") == "On")
                rocketScript.resourceSound.Play();
            //RocketFly rocketScript = other.GetComponent<RocketFly>();
            //rocketScript.Invoke("ScoreBoost", 0f);

            boostPanel.GetComponent<BoostPanel>().fillArea = 1f;

            Destroy(gameObject);
        }
    }
}
