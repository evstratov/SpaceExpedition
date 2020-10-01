using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashlightPanel : MonoBehaviour
{
    public float fillArea = 0;
    float commonResTime;
    bool isActive = false;
    Image image;

    Text statusText;
    GameObject rocket;
    RocketFly rocketScript;
    Light light;
    // Start is called before the first frame update
    void Start()
    {
        statusText = gameObject.GetComponentInChildren<Text>();
        image = gameObject.GetComponentsInChildren<Image>()[1];
    }

    // Update is called once per frame
    void Update()
    {
        if (rocket == null)
        {
            rocket = GameObject.FindWithTag("Rocket");
            rocketScript = rocket.GetComponent<RocketFly>();
            light = rocketScript.flashlight.GetComponent<Light>();
            commonResTime = rocketScript.flashlightTime * rocketScript.resourceTime;
        }
        else
        {
            fillArea = rocketScript.lightPower;
        }

        image.fillAmount = fillArea;

        if (isActive)
        {
            statusText.text = "on";
        }
        else
        {
            statusText.text = "off";
        }

        if (fillArea <= 1f && fillArea >= 0.25f)
            light.intensity = fillArea;
    }
    public void onButtonClick()
    {
        if (isActive)
        {
            isActive = false;
            rocketScript.flashlight.SetActive(false);
        } else if (!isActive && rocketScript != null && fillArea > 0)
        {
            isActive = true;
            rocketScript.flashlight.SetActive(true);
            StartCoroutine(FlashlightCoroutine());
        }
    }
    IEnumerator FlashlightCoroutine()
    {
        float delay = 0.25f;
        float speed = commonResTime / delay;
        float decrement = 1 / speed;
        
        while (isActive && rocketScript.lightPower >= 0 )
        {
            rocketScript.lightPower -= decrement;
            yield return new WaitForSeconds(delay);
        }
        isActive = false;
        rocketScript.flashlight.SetActive(false);
    }
}
