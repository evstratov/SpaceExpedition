using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Text textCoins;

    public Sprite soundOffImg;
    public Sprite soundOnImg;
    public Button soundBtn;

    public GameObject LoadingPanel;
    public AudioSource tapSound;

    bool startBtn = false;

    private string path;

    void Start()
    {
        if (PlayerPrefs.GetString("Sound") == "On")
        {
            soundBtn.GetComponent<Image>().sprite = soundOnImg;
        }
        else if (PlayerPrefs.GetString("Sound") == "Off")
        {
            soundBtn.GetComponent<Image>().sprite = soundOffImg;
        }
        bool result = StaticPrefabs.Initialize();
        while (!result) { }
        SetFirstSettings();
        ShowCoins();
        //StartCoroutine(LoadPreviewScene());
    }
    void Update()
    {
        ShowCoins();
    }
    void ShowCoins()
    {
        if (PlayerPrefs.HasKey("Coins"))
            textCoins.text = $"Coins: {PlayerPrefs.GetInt("Coins")}";
        else
            textCoins.text = $"Coins: 0";
    }
    public void onStartClick()
    {
        if (PlayerPrefs.GetString("Sound") == "On")
            tapSound.Play();
        LoadingPanel.SetActive(true);
        SceneManager.LoadScene(1);
        startBtn = true;
    }
    public void onQuitClick()
    {
        if (PlayerPrefs.GetString("Sound") == "On")
            tapSound.Play();
        Application.Quit();
    }
    public void onStoreClick()
    {
        if (PlayerPrefs.GetString("Sound") == "On")
            tapSound.Play();
    }
    public void  onSoundClick()
    {
        if(PlayerPrefs.GetString("Sound") == "Off")
        {
            tapSound.Play();
            PlayerPrefs.SetString("Sound", "On");
            soundBtn.GetComponent<Image>().sprite = soundOnImg;
        } else if (PlayerPrefs.GetString("Sound") == "On")
        {
            PlayerPrefs.SetString("Sound", "Off");
            soundBtn.GetComponent<Image>().sprite = soundOffImg;
        }
    }
    void SetFirstSettings()
    {
        if (!PlayerPrefs.HasKey("Base"))
        {
            PlayerPrefs.SetString("Sound", "On");
            PlayerPrefs.SetInt("Base", 1);
            PlayerPrefs.SetString("Selected", "Base");
        }
#if UNITY_ANDROID && !UNITY_EDITOR
        path = Path.Combine(Application.persistentDataPath, "settings.JSON");
#else
        path = Path.Combine(Application.dataPath, "settings.JSON");
#endif

        if (!File.Exists(path))
        {
            List<string> settingsList = new List<string>();
            TuningSaver ts = new TuningSaver();
            foreach(var rocket in StaticPrefabs.rocketDictionary)
            {
                ts.name = rocket.Key;
                ts.speed = 2;
                ts.fuelConsumptionSpeed = 1;
                ts.control = 1;
                ts.resourceTime = 1;
                ts.levelSpeed = 0;
                ts.levelFuel = 0;
                ts.levelControl = 0;
                ts.levelResource = 0;
                settingsList.Add(JsonUtility.ToJson(ts));
            }

            File.WriteAllLines(path, settingsList);
        }
    }

    IEnumerator LoadPreviewScene()
    {
        yield return null;

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(3);
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            //Debug.Log(asyncLoad.isDone);
            if (asyncLoad.progress >= 0.9f)
            {
                //Debug.Log(asyncLoad.progress);
                if (startBtn)
                {
                    //Debug.Log(startBtn);
                    asyncLoad.allowSceneActivation = true;
                }
            }
            yield return null;
        }
    }
}
