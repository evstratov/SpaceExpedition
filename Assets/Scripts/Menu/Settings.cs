using System.Collections;
using System.IO;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public AudioSource buySound;
    public AudioSource failSound;
    public AudioSource tapSound;

    public GameObject rocketImgPanel;
    public Text textCoins;
    public Text speedText;
    public Text fuelText;
    public Text controlText;
    public Text resourceText;

    public Button btnSpeed;
    public Button btnFuel;
    public Button btnControl;
    public Button btnResource;

    private TuningSaver tsaver;
    private int currentRocketIdx;
    private Store storeScript;
    private string path;
    private string[] fileLinesArr;
    private int indexCurLine;

    Dictionary<string, int[]> costs = new Dictionary<string, int[]>()
    {
        {"speed", new int[] { 15, 30, 45 } },
        {"fuel", new int[] { 20, 40, 65 } },
        {"control", new int[] { 5, 10, 20 } },
        {"resource", new int[] { 5, 7, 10 } }
    };



    // Start is called before the first frame update
    public void SetStartParams()
    {
        if (PlayerPrefs.GetString("Sound") == "On")
            tapSound.Play();

        ShowCoins();
        storeScript = GameObject.FindWithTag("Store").GetComponent<Store>();
        tsaver = new TuningSaver();
        currentRocketIdx = storeScript.selectedPanID;
        rocketImgPanel.GetComponent<Image>().sprite = (Sprite)storeScript.images[currentRocketIdx];

#if UNITY_ANDROID && !UNITY_EDITOR
        path = Path.Combine(Application.persistentDataPath, "settings.JSON");
#else
        path = Path.Combine(Application.dataPath, "settings.JSON");
#endif
        if (File.Exists(path))
        {
            fileLinesArr = File.ReadAllLines(path);
            indexCurLine = Array.FindIndex(fileLinesArr, str => str.Contains(storeScript.names[currentRocketIdx]));
            tsaver = JsonUtility.FromJson<TuningSaver>(fileLinesArr[indexCurLine]);
        }
        ShowSettings();
    }

    // Update is called once per frame
    void Update()
    {
        ShowPrice();
    }
    void ShowPrice()
    {
        if (tsaver.levelSpeed < 3)
            CheckAvailability(btnSpeed, costs["speed"][tsaver.levelSpeed]);
        else
            SetMaxLvlText(btnSpeed);

        if (tsaver.levelFuel < 3)
            CheckAvailability(btnFuel, costs["fuel"][tsaver.levelFuel]);
        else
             SetMaxLvlText(btnFuel);

        if (tsaver.levelControl < 3)
            CheckAvailability(btnControl, costs["control"][tsaver.levelControl]);
        else
            SetMaxLvlText(btnControl);

        if (tsaver.levelResource < 3)
            CheckAvailability(btnResource, costs["resource"][tsaver.levelResource]);
        else
            SetMaxLvlText(btnResource);

        switch (tsaver.levelSpeed)
        {
            case 0:
                btnSpeed.gameObject.GetComponentsInChildren<Text>()[0].text = costs["speed"][0] + "c";
                break;
            case 1:
                btnSpeed.gameObject.GetComponentsInChildren<Text>()[0].text = costs["speed"][1] + "c";
                break;
            case 2:
                btnSpeed.gameObject.GetComponentsInChildren<Text>()[0].text = costs["speed"][2] + "c";
                break;
        }
        switch (tsaver.levelFuel)
        {
            case 0:
                btnFuel.gameObject.GetComponentsInChildren<Text>()[0].text = costs["fuel"][0] + "c";
                break;
            case 1:
                btnFuel.gameObject.GetComponentsInChildren<Text>()[0].text = costs["fuel"][1] + "c";
                break;
            case 2:
                btnFuel.gameObject.GetComponentsInChildren<Text>()[0].text = costs["fuel"][2] + "c";
                break;
        }
        switch (tsaver.levelControl)
        {
            case 0:
                btnControl.gameObject.GetComponentsInChildren<Text>()[0].text = costs["control"][0] + "c";
                break;
            case 1:
                btnControl.gameObject.GetComponentsInChildren<Text>()[0].text = costs["control"][1] + "c";
                break;
            case 2:
                btnControl.gameObject.GetComponentsInChildren<Text>()[0].text = costs["control"][2] + "c";
                break;
        }
        switch (tsaver.levelResource)
        {
            case 0:
                btnResource.gameObject.GetComponentsInChildren<Text>()[0].text = costs["resource"][0] + "c";
                break;
            case 1:
                btnResource.gameObject.GetComponentsInChildren<Text>()[0].text = costs["resource"][1] + "c";
                break;
            case 2:
                btnResource.gameObject.GetComponentsInChildren<Text>()[0].text = costs["resource"][2] + "c";
                break;
        }
    }
    void SetMaxLvlText(Button btn)
    {
        btn.gameObject.GetComponentsInChildren<Text>()[0].text = "max";
        btn.gameObject.GetComponentsInChildren<Text>()[0].color = new Color32(243, 216, 17, 255);
    }
    void CheckAvailability(Button btn, int price)
    {
        if (price <= PlayerPrefs.GetInt("Coins"))
            // green
            btn.gameObject.GetComponentsInChildren<Text>()[0].color = new Color32(14, 181, 0, 255);
        else
            // red
            btn.gameObject.GetComponentsInChildren<Text>()[0].color = new Color32(181, 0, 6, 255);
    }
    void ShowSettings()
    {
        speedText.text = "Speed: " + tsaver.speed.ToString();
        fuelText.text = "Fuel Consumption: " + tsaver.fuelConsumptionSpeed.ToString();
        controlText.text = "Control: " + tsaver.control.ToString();
        resourceText.text = "Resource Time: " + tsaver.resourceTime.ToString();

        if (tsaver.levelSpeed > 0)
            speedText.text += ". Level " + tsaver.levelSpeed.ToString();
        if (tsaver.levelFuel > 0)
            fuelText.text += ". Level " + tsaver.levelFuel.ToString();
        if (tsaver.levelControl > 0)
            controlText.text += ". Level " + tsaver.levelControl.ToString();
        if (tsaver.levelResource > 0)
            resourceText.text += ". Level " + tsaver.levelResource.ToString();
    }
    public void onSpeedClick()
    {
        if (tsaver.levelSpeed > 2)
            return;
        if (Buy(costs["speed"][tsaver.levelSpeed]))
        {
            switch (tsaver.levelSpeed)
            {
                case 0:
                    tsaver.levelSpeed++;
                    tsaver.speed = 3f;
                    break;
                case 1:
                    tsaver.levelSpeed++;
                    tsaver.speed = 4f;
                    break;
                case 2:
                    tsaver.levelSpeed++;
                    tsaver.speed = 5f;
                    break;
            }
        }
        ShowSettings();
    }
    public void onFuelClick()
    {
        if (tsaver.levelFuel > 2)
            return;
        if (Buy(costs["fuel"][tsaver.levelFuel]))
        {
            switch (tsaver.levelFuel)
            {
                case 0:
                    tsaver.levelFuel++;
                    tsaver.fuelConsumptionSpeed = 0.85f;
                    break;
                case 1:
                    tsaver.levelFuel++;
                    tsaver.fuelConsumptionSpeed = 0.6f;
                    break;
                case 2:
                    tsaver.levelFuel++;
                    tsaver.fuelConsumptionSpeed = 0.4f;
                    break;
            }
        }
        ShowSettings();
    }
    public void onControlClick()
    {
        if (tsaver.levelControl > 2)
            return;
        if (Buy(costs["control"][tsaver.levelControl]))
        {
            switch (tsaver.levelControl)
            {
                case 0:
                    tsaver.levelControl++;
                    tsaver.control = 1.2f;
                    break;
                case 1:
                    tsaver.levelControl++;
                    tsaver.control = 1.5f;
                    break;
                case 2:
                    tsaver.levelControl++;
                    tsaver.control = 2f;
                    break;
            }
        }
        ShowSettings();
    }
    public void onResourceClick()
    {
        if (tsaver.levelResource > 2)
            return;
        if (Buy(costs["resource"][tsaver.levelResource]))
        {
            switch (tsaver.levelResource)
            {
                case 0:
                    tsaver.levelResource++;
                    tsaver.resourceTime = 1.5f;
                    break;
                case 1:
                    tsaver.levelResource++;
                    tsaver.resourceTime = 2f;
                    break;
                case 2:
                    tsaver.levelResource++;
                    tsaver.resourceTime = 3f;
                    break;
            }
        }
        ShowSettings();
    }
    bool Buy(int price)
    {
        int coins = PlayerPrefs.GetInt("Coins");
        if (price <= coins)
        {
            coins -= price;
            PlayerPrefs.SetInt("Coins", coins);
            ShowCoins();
            if (PlayerPrefs.GetString("Sound") == "On")
                buySound.Play();
            SaveSettings();
            return true;
        }
        if (PlayerPrefs.GetString("Sound") == "On")
            failSound.Play();
        return false;
    }
    void SaveSettings()
    {
        fileLinesArr[indexCurLine] = JsonUtility.ToJson(tsaver);
        File.WriteAllLines(path, fileLinesArr);
    }
    public void onBackClick()
    {
        if (PlayerPrefs.GetString("Sound") == "On")
            tapSound.Play();
        SaveSettings();
        gameObject.SetActive(false);
    }
    private void ShowCoins()
    {
        if (PlayerPrefs.HasKey("Coins"))
            textCoins.text = $"Coins: {PlayerPrefs.GetInt("Coins")}";
        else
            textCoins.text = $"Coins: 0";
    }
}
