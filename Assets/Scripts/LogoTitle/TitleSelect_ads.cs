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
        
        //���� �ʱ�ȭ
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

    //���鱤�� �ʱ�ȭ
    private void InitializeInterstitial()
    {
        this.interstitial = new InterstitialAd(this.adUnitId);
        AdRequest request = new AdRequest.Builder().Build();
        this.interstitial.LoadAd(request);

        //�̺�Ʈ �ʱ�ȭ
        this.interstitial.OnAdLoaded += HandleOnAdLoaded;
        this.interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        this.interstitial.OnAdClosed += HandleOnAdClosed;

        StartCoroutine(this.WaitForLoadAd());
    }

    //���� �ε�ɶ����� ��ٸ��� �ڵ�
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
