using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yodo1.MAS;
//using UnityEngine.Advertisements;

public class PlayerCollisions : MonoBehaviour
{
    public Slider gameTime;
    public float time=0.01f;

    public GameObject winPanel;

    public static bool win=false;

    public GameObject[] deathPanel;

    public Text coinsText;
    public PlayerSoundEffects sound;

    private string gameID="4228459";
    public static int addCnt=0;

    //private AdmobController admob;

    private Yodo1U3dBannerAdView bannerAdView;

    void Start(){
        //Advertisement.Initialize(gameID,false);
        gameTime.minValue=0;
        gameTime.maxValue=100;

        win=false;

        coinsText.text=PlayerPrefs.GetInt("coins").ToString();

        InitializeSdk();
        SetPrivacy(true, false, false);
        
        InitializeInterstitialAds();

        Yodo1U3dMas.SetInitializeDelegate((bool success, Yodo1U3dAdError error) => { });
        Yodo1U3dMas.InitializeSdk();
		
        this.RequestBanner();

        /*admob=new AdmobController();

        admob.init();*/
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

    void Update(){

        if(gameTime.value==gameTime.maxValue && !winPanel.activeSelf){
            showWinPanel();
        }
        else{
            gameTime.value+=time*Time.timeScale;
        }
    }

    private bool showed=false;
    public void death(){
        if(showed)return;

        if(addCnt%2==1 && !showed){
            // if(!admob.showIntersitionalAd()){
            //     if(Advertisement.IsReady()){
            //         Advertisement.Show("Interstitial_Android");
            //     }
            // }
            //admob.showIntersitionalAd();
            if(Yodo1U3dMas.IsInterstitialAdLoaded()){
                Yodo1U3dMas.ShowInterstitialAd();
            }
            showed=true;
        }
        showed=true;
        addCnt++;

        gameTime.gameObject.SetActive(false);
        gameObject.GetComponent<SimpleSampleCharacterControl>().death();

        
    }

    public void showWinPanel(){
        if(gameTime.gameObject.activeSelf){
            win=true;
            gameTime.gameObject.SetActive(false);
            winPanel.SetActive(true);
            sound.playWin();

            PlayerPrefs.SetInt("levelCompleted"+(Application.loadedLevel-1).ToString(),1);
        }
    }


    void OnCollisionEnter(Collision other){
        if(other.gameObject.tag=="Enemy"){
            death();
            gameObject.GetComponent<Rigidbody>().freezeRotation=false;
            gameObject.GetComponent<Rigidbody>().constraints=RigidbodyConstraints.None;
            gameObject.GetComponent<Rigidbody>().AddForce(-Vector3.forward*10);
            gameObject.GetComponent<Rigidbody>().AddRelativeTorque(Vector3.forward*-100);

            sound.playDeath();

            foreach(var detail in deathPanel)detail.SetActive(true);
        }
    }

    public ParticleSystem coinGet;
    void OnTriggerEnter(Collider other){
        if(other.gameObject.tag=="Coin"){
            coinGet.Play();
            PlayerPrefs.SetInt("coins",PlayerPrefs.GetInt("coins")+1);
            coinsText.text=PlayerPrefs.GetInt("coins").ToString();

            sound.playCoinGet();

            Destroy(other.gameObject);
        }
    }
}
