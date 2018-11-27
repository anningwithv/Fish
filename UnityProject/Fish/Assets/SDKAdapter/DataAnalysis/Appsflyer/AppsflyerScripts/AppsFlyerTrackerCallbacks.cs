using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Qarth;

namespace Qarth
{
    [TMonoSingletonAttribute("AppsFlyerTrackerCallbacks")]
    public class AppsFlyerTrackerCallbacks : TMonoSingleton<AppsFlyerTrackerCallbacks>
    {
        public class JsonText
        {
            public string af_status;
        }
        public void didReceiveConversionData(string conversionData)
        {
            
#if UNITY_ANDROID
            var JsonText = JsonUtility.FromJson<JsonText>(conversionData);

            AndroidJavaClass jc = new AndroidJavaClass("com.kids.adsdk.AdSdk");

            AndroidJavaObject context = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject jo1 = jc.CallStatic<AndroidJavaObject>("get", context);
            jo1.Call("setAttribution", JsonText.af_status, null);

            string appsFlyerAttr = PlayerPrefs.GetString("AppsFlyerAttr","");
            if (string.IsNullOrEmpty(appsFlyerAttr))
            {
                PlayerPrefs.SetString("AppsFlyerAttr", JsonText.af_status.Trim().ToLower());
            }

            Debug.LogError("didReceiveConversionData:" + JsonText.af_status);
#endif
        }

        public void didReceiveConversionDataWithError(string error)
        {
        }

        public void didFinishValidateReceipt(string validateResult)
        {
        }

        public void didFinishValidateReceiptWithError(string error)
        {

        }

        public void onAppOpenAttribution(string validateResult)
        {
        }

        public void onAppOpenAttributionFailure(string error)
        {

        }

        public void onInAppBillingSuccess()
        {
        }

        public void onInAppBillingFailure(string error)
        {

        }
    }

}



