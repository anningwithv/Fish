using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using MoPubInternal.ThirdParty.MiniJSON;
using UnityEngine;

[SuppressMessage("ReSharper", "AccessToStaticMemberViaDerivedType")]
public class MoPubAndroidRewardedVideo
{
    private static readonly AndroidJavaClass PluginClass =
        new AndroidJavaClass("com.mopub.unity.MoPubRewardedVideoUnityPlugin");

    private readonly AndroidJavaObject _plugin;

    private readonly Dictionary<MoPub.Reward, AndroidJavaObject> _rewardsDict =
        new Dictionary<MoPub.Reward, AndroidJavaObject>();


    public MoPubAndroidRewardedVideo(string adUnitId)
    {
        _plugin = new AndroidJavaObject("com.mopub.unity.MoPubRewardedVideoUnityPlugin", adUnitId);
    }


    public static void InitializeRewardedVideo()
    {
        PluginClass.CallStatic("initializeRewardedVideo");
    }


    public static void InitializeRewardedVideoWithSdkConfiguration(MoPubBase.SdkConfiguration sdkConfiguration)
    {
        PluginClass.CallStatic(
            "initializeRewardedVideoWithSdkConfiguration", sdkConfiguration.AdUnitId,
            sdkConfiguration.AdvancedBiddersString, sdkConfiguration.MediationSettingsJson,
            sdkConfiguration.NetworksToInitString);
    }


    public static void InitializeRewardedVideoWithNetworks(IEnumerable<string> networks)
    {
        PluginClass.CallStatic("initializeRewardedVideoWithNetworks", string.Join(",", networks.ToArray()));
    }


    public void RequestRewardedVideo(List<MoPub.MediationSetting> mediationSettings = null,
                                     string keywords = null, string userDataKeywords = null,
                                     double latitude = MoPub.LatLongSentinel, double longitude = MoPub.LatLongSentinel,
                                     string customerId = null)
    {
        var json = mediationSettings != null ? Json.Serialize(mediationSettings) : null;
        _plugin.Call("requestRewardedVideo", json, keywords, userDataKeywords, latitude, longitude, customerId);
    }


    public void ShowRewardedVideo(string customData)
    {
        _plugin.Call("showRewardedVideo", customData);
    }


    public bool HasRewardedVideo()
    {
        return _plugin.Call<bool>("hasRewardedVideo");
    }


    public List<MoPub.Reward> GetAvailableRewards()
    {
        // Clear any existing reward object mappings between Unity and Android Java
        _rewardsDict.Clear();

        using (var obj = _plugin.Call<AndroidJavaObject>("getAvailableRewards")) {
            var rewardsJavaObjArray = AndroidJNIHelper.ConvertFromJNIArray<AndroidJavaObject[]>(obj.GetRawObject());
            if (rewardsJavaObjArray.Length <= 1)
                return new List<MoPub.Reward>(_rewardsDict.Keys);

            foreach (var r in rewardsJavaObjArray) {
                _rewardsDict.Add(
                    new MoPub.Reward { Label = r.Call<string>("getLabel"), Amount = r.Call<int>("getAmount") }, r);
            }
        }

        return new List<MoPub.Reward>(_rewardsDict.Keys);
    }


    public void SelectReward(MoPub.Reward selectedReward)
    {
        AndroidJavaObject rewardJavaObj;
        if (_rewardsDict.TryGetValue(selectedReward, out rewardJavaObj))
            _plugin.Call("selectReward", rewardJavaObj);
        else
            Debug.LogWarning(string.Format("Selected reward {0} is not available.", selectedReward));
    }
}
