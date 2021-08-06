using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public partial class TitleSelect : MonoBehaviour
{
    public enum eAdStatus
    {
        TEST, BUILD
    }
    public eAdStatus adStatus;

    private InterstitialAd interstitial;
    private string adUnitId;
    private const string adsAppId = "ca-app-pub-5665407580778058~3944084758";
    private const string adUnitId_test = "ca-app-pub-3940256099942544/1033173712";
    private const string adUnitId_build = "ca-app-pub-5665407580778058/7117043007";
    [SerializeField] 
    private string testDeviceId;
    public void Init()
    {
        
        //광고 초기화
        MobileAds.Initialize(initStatus => { });    
        if(this.adStatus == eAdStatus.TEST)
        {
            List<string> deviceIds = new List<string>();
            deviceIds.Add(this.testDeviceId);
            RequestConfiguration requestConfiguration = new RequestConfiguration
                .Builder()
                .SetTestDeviceIds(deviceIds)
                .build();
            MobileAds.SetRequestConfiguration(requestConfiguration);
            this.adUnitId = adUnitId_test;
        }
        else
        {
            this.adUnitId = adUnitId_build;
        }
        this.InitializeInterstitial();
    }

    //전면광고를 초기화
    private void InitializeInterstitial()
    {
        this.interstitial = new InterstitialAd(this.adUnitId);
        AdRequest request = new AdRequest.Builder().Build();
        this.interstitial.LoadAd(request);

        //이벤트 초기화
        this.interstitial.OnAdLoaded += HandleOnAdLoaded;
        this.interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        this.interstitial.OnAdClosed += HandleOnAdClosed;

        StartCoroutine(this.WaitForLoadAd());
    }

    //광고가 로드될때까지 기다리는 코드
    private IEnumerator WaitForLoadAd()
    {
        while (true)
        {
            if (this.interstitial.IsLoaded()) break;
            yield return null;
        }
        this.interstitial.Show();
    }

    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        Debug.Log("HandleAdLoaded event received");
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.Log("HandleFailedToReceiveAd event received with message: " + args.LoadAdError.GetMessage());
    }

    public void HandleOnAdOpened(object sender, EventArgs args)
    {
        Debug.Log("HandleAdOpened event received");
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        this.InitImpl();
    }

    public void HandleOnAdLeavingApplication(object sender, EventArgs args)
    {
        Debug.Log("HandleAdLeavingApplication event received");
    }
}
