using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;

public class PauseMenu : MonoBehaviour
{
    public AudioSource tapSound;
    public AudioSource loopRocketSound;
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
            //yield return new WaitForSeconds(0.5f);
            yield return null;
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
        Time.timeScale = 1;
    }
    public void onMenuClick()
    {
        if (PlayerPrefs.GetString("Sound") == "On")
            tapSound.Play();
        Time.timeScale = 1;
        Advertisement.Banner.Hide();
        SceneManager.LoadScene(0);
    }
    public void onResumeClick()
    {
        if (PlayerPrefs.GetString("Sound") == "On")
        {
            tapSound.Play();
            loopRocketSound.Play();
        }
        Time.timeScale = 1;
        StopCoroutine(ShowBannerWhenReady());
        Advertisement.Banner.Hide();
    }
    public void onPauseClick()
    {
        if (PlayerPrefs.GetString("Sound") == "On")
        {
            tapSound.Play();
            loopRocketSound.Stop();
        }
        CheckAds();
        Time.timeScale = 0;
    }
}
