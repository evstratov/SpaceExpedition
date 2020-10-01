using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HyperJumpPanel : MonoBehaviour
{
    public float fillArea = 0f;
    float commonResTime;
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
            commonResTime = rocketScript.jumpTime;
        }

        if (fillArea >= 1f)
        {
            countText.text = Mathf.FloorToInt(fillArea).ToString();
        }else
        {
            countText.text = "";
        }
    }
    public void onButtonClick()
    {
        if (!isActive && rocketScript != null && fillArea >= 1f)
        {
            isActive = true;
            rocketScript.Invoke("HyperJump", 0f);
            timeImage.gameObject.SetActive(true);
            timeImage.GetComponent<Image>().fillAmount = 1;
            StartCoroutine(ResourceTimeCoroutine());
            fillArea -= 1f;
        }
    }
    public IEnumerator ResourceTimeCoroutine()
    {
        float curTime = 0;
        float deltaTime = 0.25f;
        float decrement = 1 / (commonResTime / deltaTime);

        while (curTime <= commonResTime)
        {
            curTime += deltaTime;
            imgDecrement.fillAmount -= decrement;
            yield return new WaitForSeconds(deltaTime);
        }
        timeImage.gameObject.SetActive(false);
        isActive = false;
    }

}
