using System.Collections;
using System.IO;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameController : MonoBehaviour
{
    public GameObject camera;
    public GameObject light;
    public Material skyMaterial;
    // rocket
    GameObject prefabRocket;
    GameObject rocket;
    string rocketName;
    // level
    public Text textLevelChange; 
    ILevel level;

    // Start is called before the first frame update
    void Start()
    {
        rocketName = PlayerPrefs.GetString("Selected");
        prefabRocket = StaticPrefabs.rocketDictionary[rocketName];
        //prefabRocket = Resources.Load($"Prefabs/{PlayerPrefs.GetString("Selected")}") as GameObject;
        rocket = Instantiate(prefabRocket, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
        // settings
        SetRocketSettings();

        camera.GetComponent<CameraMoving>().rocket = rocket;
        
        level = null;
        rocket.GetComponent<RocketFly>().ChangeLevelEvent += NextLevelPlay;
    }

    public void SetRocketSettings()
    {
        string path;
#if UNITY_ANDROID && !UNITY_EDITOR
        path = Path.Combine(Application.persistentDataPath, "settings.JSON");
#else
        path = Path.Combine(Application.dataPath, "settings.JSON");
#endif
        TuningSaver ts = new TuningSaver();
        if (File.Exists(path))
        {
            string[] fileLinesArr = File.ReadAllLines(path);
            int indexCurLine = Array.FindIndex(fileLinesArr, str => str.Contains(rocketName));
            ts = JsonUtility.FromJson<TuningSaver>(fileLinesArr[indexCurLine]);
        }
        RocketFly rocketScript = rocket.GetComponent<RocketFly>();
        rocketScript.speed = ts.speed;
        rocketScript.control = ts.control;
        rocketScript.resourceTime = ts.resourceTime;
        rocketScript.fuelConsumptionSpeed = ts.fuelConsumptionSpeed;
    }
    public void NextLevelPlay(byte currentLevel)
    {
        switch (currentLevel)
        {
            case 0:
                //ChangeLvlAnim(currentLevel);
                level = new LevelOne();
                level.Create(this, rocket);
                break;
            case 1:
                if (level != null)
                    level.Destroy();
                // переход
                //ChangeLvlAnim(currentLevel);
                level = new LevelTwo();
                level.Create(this, rocket);
                break;
            case 2:
                if (level != null)
                    level.Destroy();
                // переход
                //ChangeLvlAnim(currentLevel);
                level = new LevelThree();
                level.Create(this, rocket);
                break;
            case 3:
                if (level != null)
                    level.Destroy();
                // переход
                //ChangeLvlAnim(currentLevel);
                level = new LevelFour();
                level.Create(this, rocket);
                break;
            case 4:
                if (level != null)
                    level.Destroy();
                // переход
                //ChangeLvlAnim(currentLevel);
                level = new LevelFive();
                level.Create(this, rocket);
                break;
            case 5:
                if (level != null)
                    level.Destroy();
                // переход
                //ChangeLvlAnim(currentLevel);
                level = new LevelSix();
                level.Create(this, rocket);
                break;
        }
    }

    void ChangeLvlAnim(int currentLevel)
    {
        textLevelChange.text = $"Level {currentLevel + 1}";
        textLevelChange.GetComponent<Animation>().Play("LevelTextAnim");
    }
}

