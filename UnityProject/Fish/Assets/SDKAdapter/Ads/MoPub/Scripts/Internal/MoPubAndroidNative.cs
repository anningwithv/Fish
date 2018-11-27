#if mopub_native_beta
using UnityEngine;


public class MoPubAndroidNative
{
    private readonly AndroidJavaObject _nativePlugin;

    public MoPubAndroidNative(string adUnitId)
    {
        _nativePlugin = new AndroidJavaObject("com.mopub.unity.MoPubNativeUnityPlugin", adUnitId);
    }


    public void RequestNativeAd()
    {
        _nativePlugin.Call("requestNativeAd");
    }
}
#endif
