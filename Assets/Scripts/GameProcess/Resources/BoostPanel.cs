using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoostPanel : MonoBehaviour
{
    public float fillArea = 0;
    float commonResTime;
    float curTime;
    bool isActive = false;
    Image image;
    Transform timeImage;
    Image imgDecrement;
    Text countText;
    GameObject rocket;
    RocketFly rocketScript;
    // Start is called before the first frame update
    void Start()
    {
        countText = gameObject.GetComponentInChildren<Text>();
        image = gameObject.GetComponentsInChildren<Image>()[1];
        timeImage = gameObject.GetComponentsInChildren<Transform>()[2];
        imgDecrement = timeImage.gameObject.GetComponent<Image>();
        timeImage.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        image.fillAmount = fillArea;

        if (rocket == null)
        {
            rocket = GameObject.FindWithTag("Rocket");
            rocketScript = rocket.GetComponent<RocketFly>();
            commonResTime = rocketScript.boostTime * rocketScript.resourceTime;
        }

        if (fillArea >= 1f)
        {
            curTime = 0;
            fillArea -= 1f;
            if (!isActive)
            {
                isActive = true;
                timeImage.gameObject.SetActive(true);
                StartCoroutine("ResourceTimeCoroutine");
            }
            timeImage.GetComponent<Image>().fillAmount = 1;
        }
        else
        {
            countText.text = "";
        }
    }
    /*public void onButtonClick()
    {
        if (!isActive && rocketScript != null && fillArea >= 1f)
        {
            isActive = true;
            rocketScript.Invoke("ScoreBoost", 0f);
            timeImage.gameObject.SetActive(true);
            timeImage.GetComponent<Image>().fillAmount = 1;
            StartCoroutine(ResourceTimeCoroutine());
            fillArea -= 1f;
        }
    }*/

    public IEnumerator ResourceTimeCoroutine()
    {
        float deltaTime = 0.25f;
        float decrement = 1 / (commonResTime / deltaTime);

        rocketScript.boost = 2;

        while (curTime <= commonResTime)
        {
            Debug.Log(curTime);
            curTime += deltaTime;
            imgDecrement.fillAmount -= decrement;
            yield return new WaitForSeconds(deltaTime);
        }
        timeImage.gameObject.SetActive(false);
        isActive = false;
        rocketScript.boost = 1;
    }

}
