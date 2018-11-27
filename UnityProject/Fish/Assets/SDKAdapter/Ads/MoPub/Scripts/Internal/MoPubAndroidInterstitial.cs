using UnityEngine;

public class MoPubAndroidInterstitial
{
    private readonly AndroidJavaObject _interstitialPlugin;


    public MoPubAndroidInterstitial(string adUnitId)
    {
        _interstitialPlugin = new AndroidJavaObject("com.mopub.unity.MoPubInterstitialUnityPlugin", adUnitId);
    }


    public void RequestInterstitialAd(string keywords = "", string userDataKeywords = "")
    {
        _interstitialPlugin.Call("request", keywords, userDataKeywords);
    }


    public void ShowInterstitialAd()
    {
        _interstitialPlugin.Call("show");
        Debug.Log("------------------ShowMopubInterAndroid:");
    }


    public bool IsInterstitialReady {
        get { return _interstitialPlugin.Call<bool>("isReady"); }
    }


    public void DestroyInterstitialAd()
    {
        _interstitialPlugin.Call("destroy");
    }
}
