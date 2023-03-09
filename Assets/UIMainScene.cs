using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yodo1.MAS;
//using UnityEngine.Advertisements;

public class UIMainScene : ScenesController
{
    public GameObject pausePanel;

    private string gameID="4228459";
    private Yodo1U3dBannerAdView bannerAdView;

    //private AdmobController admob;
    void Start(){
        //Advertisement.Initialize(gameID,false);

        //admob=new AdmobController();

        //admob.init();
        InitializeSdk();
        SetPrivacy(true, false, false);
        
        InitializeInterstitialAds();

        Yodo1U3dMas.SetInitializeDelegate((bool success, Yodo1U3dAdError error) => { });
        Yodo1U3dMas.InitializeSdk();
		
    }

    private void RequestBanner()
    {
        // Clean up banner before reusing
        if (bannerAdView != null)
        {
            bannerAdView.Destroy();
        }

        // Create a 320x50 banner at top of the screen
        bannerAdView = new Yodo1U3dBannerAdView(Yodo1U3dBannerAdSize.Banner, Yodo1U3dBannerAdPosition.BannerTop | Yodo1U3dBannerAdPosition.BannerHorizontalCenter);
    }

    private void OnInterstitialAdOpenedEvent()
    {
        Debug.Log("[Yodo1 Mas] Interstitial ad opened");
    }

    private void OnInterstitialAdClosedEvent()
    {
        Debug.Log("[Yodo1 Mas] Interstitial ad closed");
    }

    private void OnInterstitialAdErorEvent(Yodo1U3dAdError adError)
    {
        Debug.Log("[Yodo1 Mas] Interstitial ad error - " + adError.ToString());
    }

    public void pause(){
        Time.timeScale=0;
        pausePanel.SetActive(true);

        
        if(PlayerCollisions.addCnt%2==1){
            if(Yodo1U3dMas.IsInterstitialAdLoaded()){
                Yodo1U3dMas.ShowInterstitialAd();
            }
            
        }

        PlayerCollisions.addCnt++;
        
    }

    private void SetPrivacy(bool gdpr, bool coppa, bool ccpa)
    {
        Yodo1U3dMas.SetGDPR(gdpr);
        Yodo1U3dMas.SetCOPPA(coppa);
        Yodo1U3dMas.SetCCPA(ccpa);
    }

    private void InitializeSdk()
    {
        Yodo1U3dMas.InitializeSdk();
    }

    private void InitializeInterstitialAds()
    {
        Yodo1U3dMasCallback.Interstitial.OnAdOpenedEvent +=    
        OnInterstitialAdOpenedEvent;
        Yodo1U3dMasCallback.Interstitial.OnAdClosedEvent +=      
        OnInterstitialAdClosedEvent;
        Yodo1U3dMasCallback.Interstitial.OnAdErrorEvent +=      
        OnInterstitialAdErorEvent;
    }

    public void resume(){
        Time.timeScale=1;
        pausePanel.SetActive(false);
    }

    public void nextLevel(){
        openScene(Application.loadedLevel+1);
    }

    public void restart(){
        openScene(Application.loadedLevel);
    }
    
}
