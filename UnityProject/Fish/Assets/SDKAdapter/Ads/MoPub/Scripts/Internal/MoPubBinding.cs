using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using MoPubInternal.ThirdParty.MiniJSON;

[SuppressMessage("ReSharper", "AccessToStaticMemberViaDerivedType")]
public class MoPubBinding
{
    public MoPub.Reward SelectedReward;

    private readonly string _adUnitId;


    public MoPubBinding(string adUnitId)
    {
        _adUnitId = adUnitId;
        SelectedReward = new MoPub.Reward { Label = string.Empty };
    }


    public void CreateBanner(MoPub.BannerType bannerType, MoPub.AdPosition position)
    {
        _moPubCreateBanner((int) bannerType, (int) position, _adUnitId);
    }


    public void DestroyBanner()
    {
        _moPubDestroyBanner(_adUnitId);
    }


    public void ShowBanner(bool shouldShow)
    {
        _moPubShowBanner(_adUnitId, shouldShow);
    }


    public void RefreshBanner(string keywords = "", string userDataKeywords = "")
    {
        _moPubRefreshBanner(_adUnitId, keywords, userDataKeywords);
    }


    public void SetAutorefresh(bool enabled)
    {
        _moPubSetAutorefreshEnabled(_adUnitId, enabled);
    }


    public void ForceRefresh()
    {
        _moPubForceRefresh(_adUnitId);
    }


    public void RequestInterstitialAd(string keywords = "", string userDataKeywords = "")
    {
        _moPubRequestInterstitialAd(_adUnitId, keywords, userDataKeywords);
    }


    public bool IsInterstitialReady {
        get { return _moPubIsInterstitialReady(_adUnitId); }
    }


    public void ShowInterstitialAd()
    {
        _moPubShowInterstitialAd(_adUnitId);
    }


    public void DestroyInterstitialAd()
    {
        _moPubDestroyInterstitialAd(_adUnitId);
    }


    public void RequestRewardedVideo(List<MoPub.MediationSetting> mediationSettings = null, string keywords = null,
                                     string userDataKeywords = null, double latitude = MoPub.LatLongSentinel,
                                     double longitude = MoPub.LatLongSentinel, string customerId = null)
    {
        var json = mediationSettings != null ? Json.Serialize(mediationSettings) : null;
        _moPubRequestRewardedVideo(_adUnitId, json, keywords, userDataKeywords, latitude, longitude, customerId);
    }


    // Queries if a rewarded video ad has been loaded for the given ad unit id.
    public bool HasRewardedVideo()
    {
        return _mopubHasRewardedVideo(_adUnitId);
    }


    // Queries all of the available rewards for the ad unit. This is only valid after
    // a successful requestRewardedVideo() call.
    public List<MoPub.Reward> GetAvailableRewards()
    {
        var amount = 0;
        var rewardList = _mopubGetAvailableRewards(_adUnitId) ?? string.Empty;
        var rewards = from rewardString in rewardList.Split(',')
                      select rewardString.Split(':')
                      into rewardComponents
                      where rewardComponents.Length == 2
                      where int.TryParse(rewardComponents[1], out amount)
                      select new MoPub.Reward { Label = rewardComponents[0], Amount = amount };
        return rewards.ToList();
    }


    // If a rewarded video ad is loaded this will take over the screen and show the ad
    public void ShowRewardedVideo(string customData)
    {
        _moPubShowRewardedVideo(_adUnitId, SelectedReward.Label, SelectedReward.Amount, customData);
    }


    #region DllImports
#if ENABLE_IL2CPP && UNITY_ANDROID
    // IL2CPP on Android scrubs DllImports, so we need to provide stubs to unblock compilation
    private static void _moPubCreateBanner(int bannerType, int position, string adUnitId) { }
    private static void _moPubDestroyBanner(string adUnitId) {}
    private static void _moPubShowBanner(string adUnitId, bool shouldShow) {}
    private static void _moPubRefreshBanner(string adUnitId, string keywords, string userDataKeywords) {}
    private static void _moPubSetAutorefreshEnabled(string adUnitId, bool enabled) {}
    private static void _moPubForceRefresh(string adUnitId) {}
    private static void _moPubRequestInterstitialAd(string adUnitId, string keywords, string userDataKeywords) {}
    private static bool _moPubIsInterstitialReady(string adUnitId) { return false; }
    private static void _moPubShowInterstitialAd(string adUnitId) {}
    private static void _moPubDestroyInterstitialAd(string adUnitId) {}
    private static void _moPubRequestRewardedVideo(string adUnitId, string json, string keywords,
                                                   string userDataKeywords, double latitude, double longitude,
                                                   string customerId) {}
    private static bool _mopubHasRewardedVideo(string adUnitId) { return false; }
    private static string _mopubGetAvailableRewards(string adUnitId) { return null; }
    private static void _moPubShowRewardedVideo(string adUnitId, string currencyName, int currencyAmount,
                                                string customData) {}
#else
    [DllImport("__Internal")]
    private static extern void _moPubCreateBanner(int bannerType, int position, string adUnitId);


    [DllImport("__Internal")]
    private static extern void _moPubDestroyBanner(string adUnitId);


    [DllImport("__Internal")]
    private static extern void _moPubShowBanner(string adUnitId, bool shouldShow);


    [DllImport("__Internal")]
    private static extern void _moPubRefreshBanner(string adUnitId, string keywords, string userDataKeywords);


    [DllImport("__Internal")]
    private static extern void _moPubSetAutorefreshEnabled(string adUnitId, bool enabled);


    [DllImport("__Internal")]
    private static extern void _moPubForceRefresh(string adUnitId);


    [DllImport("__Internal")]
    private static extern void _moPubRequestInterstitialAd(string adUnitId, string keywords, string userDataKeywords);


    [DllImport("__Internal")]
    private static extern bool _moPubIsInterstitialReady(string adUnitId);


    [DllImport("__Internal")]
    private static extern void _moPubShowInterstitialAd(string adUnitId);


    [DllImport("__Internal")]
    private static extern void _moPubDestroyInterstitialAd(string adUnitId);


    [DllImport("__Internal")]
    private static extern void _moPubRequestRewardedVideo(string adUnitId, string json, string keywords,
                                                          string userDataKeywords, double latitude, double longitude,
                                                          string customerId);


    [DllImport("__Internal")]
    private static extern bool _mopubHasRewardedVideo(string adUnitId);


    [DllImport("__Internal")]
    private static extern string _mopubGetAvailableRewards(string adUnitId);


    [DllImport("__Internal")]
    private static extern void _moPubShowRewardedVideo(string adUnitId, string currencyName, int currencyAmount,
                                                       string customData);
#endif
    #endregion
}
