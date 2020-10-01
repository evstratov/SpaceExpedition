using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticPrefabs 
{
    public static string gameID = "3400507";
    public static string placementRewardVidioID = "rewardedVideo";
    public static string placementBannerID = "Banner";
    public static bool testMode = false;
    public static float skyIntensity = 2f;

    public static GameObject[] planets = new GameObject[40];
    public static GameObject shield;
    public static GameObject hyperJump;
    public static GameObject booster;
    public static GameObject fuel;
    public static GameObject freeze;
    public static GameObject flashlight;
    public static GameObject nebula;
    public static GameObject asteroid;
    public static GameObject blackHole;

    public static Dictionary<string, GameObject> rocketDictionary;
    public static GameObject baseRocket;
    public static GameObject usaRocket;
    public static GameObject ussrRocket;
    public static GameObject voyagerRocket;
    public static GameObject destroyerRocket;
    public static GameObject ufoRocket;
    public static GameObject fighterRocket;
    public static GameObject prometheusRocket;
    public static GameObject enterpriseRocket;
    public static GameObject starBusRocket;


    public static bool Initialize()
    {
        planets[0] = Resources.Load("Prefabs/planet1") as GameObject;
        planets[1] = Resources.Load("Prefabs/planet2") as GameObject;
        planets[2] = Resources.Load("Prefabs/planet3") as GameObject;
        planets[3] = Resources.Load("Prefabs/planet4") as GameObject;
        planets[4] = Resources.Load("Prefabs/planet5") as GameObject;
        planets[5] = Resources.Load("Prefabs/planet6") as GameObject;
        planets[6] = Resources.Load("Prefabs/planet7") as GameObject;
        planets[7] = Resources.Load("Prefabs/planet8") as GameObject;
        planets[8] = Resources.Load("Prefabs/planet9") as GameObject;
        planets[9] = Resources.Load("Prefabs/planet10") as GameObject;
        planets[10] = Resources.Load("Prefabs/planet11") as GameObject;
        planets[11] = Resources.Load("Prefabs/planet12") as GameObject;
        planets[12] = Resources.Load("Prefabs/planet13") as GameObject;
        planets[13] = Resources.Load("Prefabs/planet14") as GameObject;
        planets[14] = Resources.Load("Prefabs/planet15") as GameObject;
        planets[15] = Resources.Load("Prefabs/planet16") as GameObject;
        planets[16] = Resources.Load("Prefabs/planet17") as GameObject;
        planets[17] = Resources.Load("Prefabs/planet18") as GameObject;
        planets[18] = Resources.Load("Prefabs/planet19") as GameObject;
        planets[19] = Resources.Load("Prefabs/planet20") as GameObject;
        planets[20] = Resources.Load("Prefabs/planet21") as GameObject;
        planets[21] = Resources.Load("Prefabs/planet22") as GameObject;
        planets[22] = Resources.Load("Prefabs/planet23") as GameObject;
        planets[23] = Resources.Load("Prefabs/planet24") as GameObject;
        planets[24] = Resources.Load("Prefabs/planet25") as GameObject;
        planets[25] = Resources.Load("Prefabs/planet26") as GameObject;
        planets[26] = Resources.Load("Prefabs/planet27") as GameObject;
        planets[27] = Resources.Load("Prefabs/planet28") as GameObject;
        planets[28] = Resources.Load("Prefabs/planet29") as GameObject;
        planets[29] = Resources.Load("Prefabs/planet30") as GameObject;
        planets[30] = Resources.Load("Prefabs/planet31") as GameObject;
        planets[31] = Resources.Load("Prefabs/planet32") as GameObject;
        planets[32] = Resources.Load("Prefabs/planet33") as GameObject;
        planets[33] = Resources.Load("Prefabs/planet34") as GameObject;
        planets[34] = Resources.Load("Prefabs/planet35") as GameObject;
        planets[35] = Resources.Load("Prefabs/planet36") as GameObject;
        planets[36] = Resources.Load("Prefabs/planet38") as GameObject;
        planets[37] = Resources.Load("Prefabs/planet38") as GameObject;
        planets[38] = Resources.Load("Prefabs/planet39") as GameObject;
        planets[39] = Resources.Load("Prefabs/planet40") as GameObject;

        blackHole = Resources.Load("Prefabs/BlackHole") as GameObject;
        
        shield = Resources.Load("Prefabs/Shield") as GameObject;
        hyperJump = Resources.Load("Prefabs/HyperJump") as GameObject;
        booster = Resources.Load("Prefabs/Booster") as GameObject;
        fuel = Resources.Load("Prefabs/Fuel") as GameObject;
        asteroid = Resources.Load("Prefabs/asteroid") as GameObject;
        freeze = Resources.Load("Prefabs/Freeze") as GameObject;
        nebula = Resources.Load("Prefabs/Nebula") as GameObject;
        flashlight = Resources.Load("Prefabs/Flashlight") as GameObject;

        baseRocket = Resources.Load("Prefabs/Base") as GameObject;
        usaRocket = Resources.Load("Prefabs/USA(watch video)") as GameObject;
        ussrRocket = Resources.Load("Prefabs/USSR") as GameObject;
        voyagerRocket = Resources.Load("Prefabs/Voyager") as GameObject;
        destroyerRocket = Resources.Load("Prefabs/Destroyer") as GameObject;
        fighterRocket = Resources.Load("Prefabs/Fighter") as GameObject;
        ufoRocket = Resources.Load("Prefabs/UFO") as GameObject;
        starBusRocket = Resources.Load("Prefabs/StarBus") as GameObject;
        prometheusRocket = Resources.Load("Prefabs/Prometheus") as GameObject;
        enterpriseRocket = Resources.Load("Prefabs/Enterprise") as GameObject;

        rocketDictionary = new Dictionary<string, GameObject>();
        rocketDictionary.Add("Base", baseRocket);
        rocketDictionary.Add("USA(watch video)", usaRocket);
        rocketDictionary.Add("USSR", ussrRocket);
        rocketDictionary.Add("Voyager", voyagerRocket);
        rocketDictionary.Add("Destroyer", destroyerRocket);
        rocketDictionary.Add("Fighter", fighterRocket);
        rocketDictionary.Add("UFO", ufoRocket);
        rocketDictionary.Add("StarBus", starBusRocket);
        rocketDictionary.Add("Prometheus", prometheusRocket);
        rocketDictionary.Add("Enterprise", enterpriseRocket);

        return true;
    }
}
