using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;

public class RestartMenu : MonoBehaviour
{
    public Text textScore;
    public Text textBest;
    public AudioSource tapSound;

    void Start()
    {
        CheckAds();

        textScore.text = $"You score: {PlayerPrefs.GetInt("Score")}";
        textBest.text = $"Best score: {PlayerPrefs.GetInt("Best")}";
    }
    void CheckAds()
    {
        if (PlayerPrefs.GetString("NoAds") != "Yes")
        {
            if (Advertisement.isSupported)
            {
                Advertisement.Initialize(StaticPrefabs.gameID, StaticPrefabs.testMode);
            }
            StartCoroutine(ShowBannerWhenReady());
        }
    }
    IEnumerator ShowBannerWhenReady()
    {
        while (!Advertisement.IsReady(StaticPrefabs.placementBannerID))
        {
            yield return new WaitForSeconds(0.5f);
        }

        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
        Advertisement.Banner.Show(StaticPrefabs.placementBannerID);
    }

    public void onRestartClick()
    {
        if (PlayerPrefs.GetString("Sound") == "On")
            tapSound.Play();

        Advertisement.Banner.Hide();
        SceneManager.LoadScene(1);
    }
    public void onMenuClick()
    {
        if (PlayerPrefs.GetString("Sound") == "On")
            tapSound.Play();

        Advertisement.Banner.Hide();
        SceneManager.LoadScene(0);
    }
}
