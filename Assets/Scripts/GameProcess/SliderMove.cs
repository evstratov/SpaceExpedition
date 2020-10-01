using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderMove : MonoBehaviour
{
    GameObject rocket;
    RocketFly rocketScript;
    Slider slider;
    
    void Start()
    {
        
        /*slider = gameObject.GetComponent<Slider>();
        slider.minValue = 0.5f;*/
    }
    void Update()
    {
        /*if  (rocket == null)
        {
            rocket = GameObject.FindWithTag("Rocket");
            rocketScript = rocket.GetComponent<RocketFly>();
        }

        if (rocketScript != null && slider.maxValue != rocketScript.maxSpeed)
        {
            slider.maxValue = rocketScript.maxSpeed;
            slider.value = slider.maxValue;
        }*/
    }
    public void onSliderMove()
    {
        //rocketScript.speed = slider.value;
    }
}
