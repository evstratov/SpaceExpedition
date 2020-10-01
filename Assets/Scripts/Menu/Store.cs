using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Monetization;

public class Store : Singleton<Store>
{
    public enum ModelType
    {
        CoinsType,
        AdsType,
        DollarType
    }
    public struct Rockets
    {
        public GameObject background;
        public ModelType type;
        public string name;
        public int cost;
        public float dollarCost;
        public string purchaseID;
        public Sprite img;
    }

    public string[] names = {"Base", "USA(watch video)", "USSR", "Enterprise", "Voyager",
        "StarBus", "Destroyer", "UFO", "Prometheus", "Fighter",
        "+99 Coins","+299 Coins", "NoAds"};
    int[] costs = { 0, 0, 8, 16, 25, 30, 40, 0, 0, 0, 0, 0, 0 };

    string[] purchaseIDs = { "", "", "", "", "", "", "", "gp_ufo", "gp_prometheus", "gp_fighter", "gp_99", "gp_299", "gp_noads" };
    ModelType[] types = { ModelType.CoinsType, ModelType.AdsType, ModelType.CoinsType, ModelType.CoinsType,
        ModelType.CoinsType, ModelType.CoinsType, ModelType.CoinsType, ModelType.DollarType, ModelType.DollarType,
        ModelType.DollarType, ModelType.DollarType, ModelType.DollarType, ModelType.DollarType };

    public Object[] images;
   
    [Header("Text")]
    public Text textCoins;
    public string loadedPrice = "$";
    [Header("Text")]
    public Button btnBuy;
    public Button btnSelect;
    public Button btnTuning;

    [Header("Controllers")]
    public int panOffset;
    public bool isScrolling;
    public float snapSpeed;
    public float scaleOffset;
    public float scaleSpeed;

    public AudioSource buySound;
    public AudioSource failSound;
    public AudioSource scrollSound;
    public AudioSource tapSound;
    [Header("Prefab")]
    public GameObject panPrefab;
    public ScrollRect scrollRect;
    private Rockets[] instPans;


    private Vector2[] pansPos;
    private RectTransform contentRect;

    private Vector2 contentVector;
    private Vector2[] panScale;
    private int panCount;

    public int selectedPanID;
    void Start()
    {
        StartCoroutine(LoadPriceRuotine());
        if (Monetization.isSupported)
        {
            Monetization.Initialize(StaticPrefabs.gameID, StaticPrefabs.testMode);
        }
        ShowCoins();
        images = Resources.LoadAll("Sprites", typeof(Sprite));
        Paint();
    }
    void Paint()
    {
        panCount = names.Length;
        panScale = new Vector2[panCount];
        contentRect = GetComponent<RectTransform>();
        instPans = new Rockets[panCount];
        pansPos = new Vector2[panCount];

        for (int i = 0; i < panCount; i++)
        {
            instPans[i].background = Instantiate(panPrefab, transform, false);
            instPans[i].name = names[i];
            instPans[i].cost = costs[i];
            //instPans[i].dollarCost = dollarCosts[i];
            instPans[i].purchaseID = purchaseIDs[i];
            instPans[i].type = types[i];

            /*instPans[i].speed = speeds[i];
            instPans[i].fuel = fuels[i];
            if (instPans[i].speed > 0 && instPans[i].fuel > 0)
            {
                instPans[i].background.GetComponentsInChildren<Text>()[2].text = "speed: " + instPans[i].speed.ToString();
                instPans[i].background.GetComponentsInChildren<Text>()[3].text = "fuel: " + instPans[i].fuel.ToString();
            } else
            {
                instPans[i].background.GetComponentsInChildren<Text>()[2].text = "";
                instPans[i].background.GetComponentsInChildren<Text>()[3].text = "";
            }*/

            instPans[i].img = (Sprite)images[i];

            instPans[i].background.GetComponentsInChildren<Text>()[0].text = instPans[i].name;
            switch (instPans[i].type)
            {
                case ModelType.AdsType:
                    instPans[i].background.GetComponentsInChildren<Text>()[1].text = "";
                    break;
                case ModelType.CoinsType:
                    if (instPans[i].cost > 0)
                        instPans[i].background.GetComponentsInChildren<Text>()[1].text = instPans[i].cost.ToString() + " coins";
                    else
                        instPans[i].background.GetComponentsInChildren<Text>()[1].text = "";
                    break;
                case ModelType.DollarType:
                    instPans[i].background.GetComponentsInChildren<Text>()[1].text = "";
                    break;
            }

            instPans[i].background.GetComponent<Image>().sprite = instPans[i].img;

            if (i == 0) continue;

            instPans[i].background.transform.localPosition = new Vector2(instPans[i - 1].background.transform.localPosition.x + panPrefab.GetComponent<RectTransform>().sizeDelta.x + panOffset,
                instPans[i].background.transform.localPosition.y);
            pansPos[i] = -instPans[i].background.transform.localPosition;
        }
    }
    void FixedUpdate()
    {
        if (contentRect.anchoredPosition.x >= pansPos[0].x && !isScrolling || contentRect.anchoredPosition.x <= pansPos[pansPos.Length - 1].x && !isScrolling)
            scrollRect.inertia = false;

        float nearestPos = float.MaxValue;

        for (int i = 0; i < panCount; i++)
        {
            float distance = Mathf.Abs(contentRect.anchoredPosition.x - pansPos[i].x);
            if (distance < nearestPos)
            {
                nearestPos = distance;
                selectedPanID = i;
                if(PlayerPrefs.HasKey(instPans[selectedPanID].name) && instPans[selectedPanID].name != "NoAds")
                    btnTuning.gameObject.SetActive(true);
                else
                    btnTuning.gameObject.SetActive(false);
            }
            float scale = Mathf.Clamp(1 / (distance / panOffset) * scaleOffset, 0.5f, 1f);
            panScale[i].x = Mathf.SmoothStep(instPans[i].background.transform.localScale.x, scale, scaleSpeed * Time.fixedDeltaTime);
            panScale[i].y = Mathf.SmoothStep(instPans[i].background.transform.localScale.y, scale, scaleSpeed * Time.fixedDeltaTime);
            instPans[i].background.transform.localScale = panScale[i];
        }

        float scrollVelocity = Mathf.Abs(scrollRect.velocity.x);
        if (scrollVelocity < 400 && !isScrolling) scrollRect.inertia = false;

        if (isScrolling || scrollVelocity > 400) return;
        contentVector.x = Mathf.SmoothStep(contentRect.anchoredPosition.x, pansPos[selectedPanID].x, snapSpeed * Time.fixedDeltaTime);
        contentRect.anchoredPosition = contentVector;

        SetSelect();
        CheckBuy();
    }
    public void Scrolling(bool scroll)
    {
        isScrolling = scroll;
        scrollRect.inertia = true;
    }

    public void ShowCoins()
    {
        if (PlayerPrefs.HasKey("Coins"))
            textCoins.text = $"Coins: {PlayerPrefs.GetInt("Coins")}";
        else
            textCoins.text = $"Coins: 0";
    }
    private void CheckBuy()
    {
        if (PlayerPrefs.HasKey(instPans[selectedPanID].name))
        {
            if (instPans[selectedPanID].name == "NoAds" && PlayerPrefs.GetString("NoAds") == "Yes")
            {
                btnBuy.gameObject.SetActive(false);
                btnSelect.gameObject.SetActive(false);
            }
            else
            {
                btnBuy.gameObject.SetActive(false);
                btnSelect.gameObject.SetActive(true);
                if (PlayerPrefs.HasKey("Selected") && PlayerPrefs.GetString("Selected") == instPans[selectedPanID].name)
                {
                    SetSelected();
                }
            }
        } else
        {
            switch(instPans[selectedPanID].type)
            {
                case ModelType.AdsType:
                    btnBuy.gameObject.GetComponentsInChildren<Text>()[0].text = "Watch";
                    btnBuy.gameObject.GetComponentsInChildren<Text>()[0].color = new Color32(14, 181, 0, 255);
                    btnBuy.gameObject.SetActive(true);
                    btnSelect.gameObject.SetActive(false);
                    break;
                case ModelType.CoinsType:
                    int coins = PlayerPrefs.GetInt("Coins");
                    btnBuy.gameObject.GetComponentsInChildren<Text>()[0].text = "Buy";
                    if (instPans[selectedPanID].cost <= coins)
                    {
                        // green color
                        btnBuy.gameObject.GetComponentsInChildren<Text>()[0].color = new Color32(14, 181, 0, 255);
                    }
                    else
                    {
                        // red color
                        btnBuy.gameObject.GetComponentsInChildren<Text>()[0].color = new Color32(181, 0, 6, 255);
                    }
                    btnBuy.gameObject.SetActive(true);
                    btnSelect.gameObject.SetActive(false);
                    break;
                case ModelType.DollarType:
                    btnBuy.gameObject.GetComponentsInChildren<Text>()[0].color = new Color32(243, 216, 17, 255);
                    //btnBuy.gameObject.GetComponentsInChildren<Text>()[0].text = $"{instPans[selectedPanID].dollarCost}$"; 

                    btnBuy.gameObject.GetComponentsInChildren<Text>()[0].text = loadedPrice;
                    btnBuy.gameObject.SetActive(true);
                    btnSelect.gameObject.SetActive(false);
                    break;
            }
        }
    }
    public void onBuyClick()
    {
        switch(instPans[selectedPanID].type)
        {
            case ModelType.AdsType:
                if (PlayerPrefs.GetString("Sound") == "On")
                    tapSound.Play();
                if (Monetization.isSupported)
                {
                    StartCoroutine(ShowVideoForModel());
                }
                break;
            case ModelType.CoinsType:
                int coins = PlayerPrefs.GetInt("Coins");
                if (instPans[selectedPanID].cost <= coins)
                {
                    coins -= instPans[selectedPanID].cost;
                    PlayerPrefs.SetInt("Coins", coins);
                    PlayerPrefs.SetInt(instPans[selectedPanID].name, 1);
                    ShowCoins();
                    btnBuy.gameObject.SetActive(false);
                    btnSelect.gameObject.SetActive(true);
                    if (PlayerPrefs.GetString("Sound") == "On")
                        buySound.Play();
                } else
                {
                    if (PlayerPrefs.GetString("Sound") == "On")
                        failSound.Play();
                }
                break;
             case ModelType.DollarType:
                switch(instPans[selectedPanID].name)
                {
                    /*case "Destroyer":
                        Purchaser.Instance.BuyDestroyer();
                        break;*/
                    case "UFO":
                        Purchaser.Instance.BuyUFO();
                        break;
                    case "Prometheus":
                        Purchaser.Instance.BuyPrometheus();
                        break;
                    case "Fighter":
                        Purchaser.Instance.BuyFighter();
                        break;
                    case "NoAds":
                        Purchaser.Instance.BuyNoAds();
                        break;
                    case "+99 Coins":
                        Purchaser.Instance.BuyCoins99();
                        break;
                    case "+299 Coins":
                        Purchaser.Instance.BuyCoins299();
                        break;
                }
                break;
        }
    }
    #region purchaches 
    public void AddCoins(int addCoins)
    {
        int coins = PlayerPrefs.GetInt("Coins");
        coins += addCoins;
        PlayerPrefs.SetInt("Coins", coins);
        ShowCoins();
        if (PlayerPrefs.GetString("Sound") == "On")
            buySound.Play();
    }
    public void NoAds()
    {
        PlayerPrefs.SetString("NoAds", "Yes");
        btnBuy.gameObject.SetActive(false);
        btnSelect.gameObject.SetActive(false);
        if (PlayerPrefs.GetString("Sound") == "On")
            buySound.Play();
    }
    /*public void UnlockDestroyer()
    {
        PlayerPrefs.SetInt("Destroyer", 1);
        btnBuy.gameObject.SetActive(false);
        btnSelect.gameObject.SetActive(true);
    }*/
    public void UnlockFighter()
    {
        PlayerPrefs.SetInt("Fighter", 1);
        btnBuy.gameObject.SetActive(false);
        btnSelect.gameObject.SetActive(true);
        if (PlayerPrefs.GetString("Sound") == "On")
            buySound.Play();
    }
    public void UnlockUFO()
    {
        PlayerPrefs.SetInt("UFO", 1);
        btnBuy.gameObject.SetActive(false);
        btnSelect.gameObject.SetActive(true);
        if (PlayerPrefs.GetString("Sound") == "On")
            buySound.Play();
    }
    public void UnlockPrometheus()
    {
        PlayerPrefs.SetInt("Prometheus", 1);
        btnBuy.gameObject.SetActive(false);
        btnSelect.gameObject.SetActive(true);
        if (PlayerPrefs.GetString("Sound") == "On")
            buySound.Play();
    }
    private IEnumerator LoadPriceRuotine()
    {
        while (!Purchaser.Instance.IsInitialized())
            yield return null;

        while (instPans == null)
        {
            yield return null;
        }

        if (instPans[selectedPanID].purchaseID != "")
            loadedPrice = Purchaser.Instance.GetPriceFromStore(instPans[selectedPanID].purchaseID);
        
    }
    #endregion
    public void onSelectClick()
    {
        if (PlayerPrefs.GetString("Sound") == "On")
            tapSound.Play();

        PlayerPrefs.SetString("Selected", instPans[selectedPanID].name);
        SetSelected();
    }
    public void onBackClick()
    {
        if (PlayerPrefs.GetString("Sound") == "On")
            tapSound.Play();
    }
    private void SetSelected()
    {
        // green color
        btnSelect.gameObject.GetComponentsInChildren<Text>()[0].text = "Selected";
        btnSelect.gameObject.GetComponentsInChildren<Text>()[0].color = new Color32(14, 181, 0, 255);
    }
    private void SetSelect()
    {
        btnSelect.gameObject.GetComponentsInChildren<Text>()[0].text = "Select";
        btnSelect.gameObject.GetComponentsInChildren<Text>()[0].color = new Color32(243, 216, 17, 255);
    }
    #region ADS
    public void onWatchVideoClick()
    {
        if (Monetization.isSupported)
        {
            StartCoroutine(ShowVideoForCoins());
        }
    }
    IEnumerator ShowVideoForCoins()
    {
        while (!Monetization.IsReady(StaticPrefabs.placementRewardVidioID))
        {
            yield return new WaitForSeconds(0.5f);
            //yield return null;
        }
        ShowAdCallbacks options = new ShowAdCallbacks();
        options.finishCallback = HandleCoinsShowResult;
        ShowAdPlacementContent ad = Monetization.GetPlacementContent(StaticPrefabs.placementRewardVidioID) as ShowAdPlacementContent;
        ad.Show(options);
    }
    void HandleCoinsShowResult(UnityEngine.Monetization.ShowResult result)
    {
        if (result == UnityEngine.Monetization.ShowResult.Finished)
        {
            int coins = PlayerPrefs.GetInt("Coins");
            coins += 15;
            PlayerPrefs.SetInt("Coins", coins);
            ShowCoins();
        }
        else if (result == UnityEngine.Monetization.ShowResult.Skipped)
        {
            Debug.LogWarning("The player skipped the video - DO NOT REWARD!");
        }
        else if (result == UnityEngine.Monetization.ShowResult.Failed)
        {
            Debug.LogError("Video failed to show");
        }
    }
    IEnumerator ShowVideoForModel()
    {
        while (!Monetization.IsReady(StaticPrefabs.placementRewardVidioID))
        {
            //yield return new WaitForSeconds(0.5f);
            yield return null;
        }
        ShowAdCallbacks options = new ShowAdCallbacks();
        options.finishCallback = HandleModelShowResult;
        ShowAdPlacementContent ad = Monetization.GetPlacementContent(StaticPrefabs.placementRewardVidioID) as ShowAdPlacementContent;
        ad.Show(options);
    }
    void HandleModelShowResult(UnityEngine.Monetization.ShowResult result)
    {
        if (result == UnityEngine.Monetization.ShowResult.Finished)
        {
            PlayerPrefs.SetInt(instPans[selectedPanID].name, 1);
            btnBuy.gameObject.SetActive(false);
            btnSelect.gameObject.SetActive(true);
        }
        else if (result == UnityEngine.Monetization.ShowResult.Skipped)
        {
            Debug.LogWarning("The player skipped the video - DO NOT REWARD!");
        }
        else if (result == UnityEngine.Monetization.ShowResult.Failed)
        {
            Debug.LogError("Video failed to show");
        }
    }
    #endregion
}
