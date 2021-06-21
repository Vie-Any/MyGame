using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class AdsButton : MonoBehaviour, IUnityAdsListener
{
#if UNITY_IOS
    private string gameID = "4114281";
    private string myPlacementID = "Rewarded_iOS";
#elif UNITY_ANDROID
    private string gameID = "4114280";
    private string myPlacementID = "Rewarded_Android";
#endif

    Button adsButton;

    public bool testMode = true;


    // Start is called before the first frame update
    void Start()
    {
        adsButton = GetComponent<Button>();
        //adsButton.interactable = Advertisement.IsReady(myPlacementId);

        if (adsButton)
        {
            
            adsButton.onClick.AddListener(ShowRewardedVideo);
        }

        Advertisement.AddListener(this);
        Advertisement.Initialize(gameID, testMode);
    }

    public void ShowRewardedVideo()
    {
        Advertisement.Show(myPlacementID);
    }

    public void OnUnityAdsDidError(string message)
    {
        
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        switch (showResult)
        {
            case ShowResult.Failed:
                break;
            case ShowResult.Skipped:
                break;
            case ShowResult.Finished:
                FindObjectOfType<PlayerController>().health = 3;
                FindObjectOfType<PlayerController>().isDead = false;
                UIManager.instance.UpdateHealth(FindObjectOfType<PlayerController>().health);
                break;
        }
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        
    }

    public void OnUnityAdsReady(string placementId)
    {
        //if (placementId == myPlacementID)
        //{
        //    Debug.Log("尝试激活按钮");
        //    adsButton.interactable = Advertisement.IsReady(myPlacementID);
        //    Debug.Log("激活结果: "+ Advertisement.IsReady(myPlacementID));
        //    if (Advertisement.IsReady(myPlacementID))
        //    {
        //        Debug.Log("广告准备好了!");
        //    }
        //}
        if (Advertisement.IsReady(myPlacementID))
        {
            Debug.Log("广告准备好了");
        }
    }
}
