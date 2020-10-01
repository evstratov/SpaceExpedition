using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoving : MonoBehaviour
{
    public GameObject rocket;

    public bool isBlackHoleAttraction;
    Vector3 offset;

    void Start()
    {
        isBlackHoleAttraction = false;
        //if(PlayerPrefs.GetString("Selected") == "USSR")
        //    offset = new Vector3(0, -20f, -4.5f);
        //else 
            offset = new Vector3(0, -15f, -2.5f);
    }

    void Update()
    {
        if (rocket != null && !isBlackHoleAttraction)
        {
            transform.position = rocket.transform.position + offset;
        }
        if (isBlackHoleAttraction)
            BlackHoleAttraction();
    }
    void BlackHoleAttraction()
    {
        Vector3 camPos = transform.position;
        camPos.y -= 0.7f;
        transform.position = camPos;
    }
}
