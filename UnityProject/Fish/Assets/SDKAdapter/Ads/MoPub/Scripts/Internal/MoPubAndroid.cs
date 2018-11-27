using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using MPBanner = MoPubAndroidBanner;
using MPInterstitial = MoPubAndroidInterstitial;
using MPRewardedVideo = MoPubAndroidRewardedVideo;

#if mopub_native_beta
using MPNative = MoPubAndroidNative;
#endif

/// <summary>
/// This class serves as a bridge to the MoPub Android SDK (via the MoPub Unity Android wrapper).
/// For full API documentation, See <see cref="MoPubUnityEditor"/>.
/// </summary>
[SuppressMessage("ReSharper", "UnusedMember.Global")]
public class MoPubAndroid : MoPubBase
{
    static MoPubAndroid()
    {
        InitManager();
    }

    private static readonly AndroidJavaClass PluginClass = new AndroidJavaClass("com.mopub.unity.MoPubUnityPlugin");


    private static readonly Dictionary<string, MPBanner> BannerPluginsDict = new Dictionary<string, MPBanner>();

    private static readonly Dictionary<string, MPInterstitial> InterstitialPluginsDict =
        new Dictionary<string, MPInterstitial>();

    private static readonly Dictionary<string, MPRewardedVideo> RewardedVideoPluginsDict =
        new Dictionary<string, MPRewardedVideo>();

#if mopub_native_beta
    private static readonly Dictionary<string, MPNative> NativePluginsDict = new Dictionary<string, MPNative>();
#endif


    #region SdkSetup

    /// See MoPubUnityEditor.<see cref="MoPubUnityEditor.InitializeSdk(string)"/>
    public static void InitializeSdk(string anyAdUnitId)
    {
        ValidateAdUnitForSdkInit(anyAdUnitId);
        InitializeSdk(new SdkConfiguration { AdUnitId = anyAdUnitId });
    }


    /// See MoPubUnityEditor.<see cref="MoPubUnityEditor.InitializeSdk(MoPubBase.SdkConfiguration)"/>
    public static void InitializeSdk(SdkConfiguration sdkConfiguration)
    {
        ValidateAdUnitForSdkInit(sdkConfiguration.AdUnitId);
        PluginClass.CallStatic(
            "initializeSdk", sdkConfiguration.AdUnitId, sdkConfiguration.AdvancedBiddersString,
            sdkConfiguration.MediationSettingsJson, sdkConfiguration.NetworksToInitString);
    }


    /// See MoPubUnityEditor.<see cref="MoPubUnityEditor.LoadBannerPluginsForAdUnits(string[])"/>
    public static void LoadBannerPluginsForAdUnits(string[] bannerAdUnitIds)
    {
        foreach (var bannerAdUnitId in bannerAdUnitIds)
            BannerPluginsDict.Add(bannerAdUnitId, new MPBanner(bannerAdUnitId));

        Debug.Log(
            bannerAdUnitIds.Length + " banner AdUnits loaded for plugins:\n" + string.Join(", ", bannerAdUnitIds));
    }


    /// See MoPubUnityEditor.<see cref="MoPubUnityEditor.LoadInterstitialPluginsForAdUnits(string[])"/>
    public static void LoadInterstitialPluginsForAdUnits(string[] interstitialAdUnitIds)
    {
        foreach (var interstitialAdUnitId in interstitialAdUnitIds)
            InterstitialPluginsDict.Add(interstitialAdUnitId, new MPInterstitial(interstitialAdUnitId));

        Debug.Log(
            interstitialAdUnitIds.Length + " interstitial AdUnits loaded for plugins:\n"
            + string.Join(", ", interstitialAdUnitIds));
    }


    /// See MoPubUnityEditor.<see cref="MoPubUnityEditor.LoadRewardedVideoPluginsForAdUnits(string[])"/>
    public static void LoadRewardedVideoPluginsForAdUnits(string[] rewardedVideoAdUnitIds)
    {
        foreach (var rewardedVideoAdUnitId in rewardedVideoAdUnitIds)
            RewardedVideoPluginsDict.Add(rewardedVideoAdUnitId, new MPRewardedVideo(rewardedVideoAdUnitId));

        Debug.Log(
            rewardedVideoAdUnitIds.Length + " rewarded video AdUnits loaded for plugins:\n"
            + string.Join(", ", rewardedVideoAdUnitIds));
    }


#if mopub_native_beta
    /// See MoPubUnityEditor.<see cref="MoPubUnityEditor.LoadNativePluginsForAdUnits(string[])"/>
    public static void LoadNativePluginsForAdUnits(string[] nativeAdUnitIds)
    {
        foreach (var nativeAdUnitId in nativeAdUnitIds)
            NativePluginsDict.Add(nativeAdUnitId, new MPNative(nativeAdUnitId));

        Debug.Log(
            nativeAdUnitIds.Length + " native AdUnits loaded for plugins:\n" + string.Join(", ", nativeAdUnitIds));
    }
#endif


    /// See MoPubUnityEditor.<see cref="MoPubUnityEditor.IsSdkInitialized"/>
    public static bool IsSdkInitialized {
        get { return PluginClass.CallStatic<bool>("isSdkInitialized"); }
    }


    /// See MoPubUnityEditor.<see cref="MoPubUnityEditor.AdvancedBiddingEnabled"/>
    public static bool AdvancedBiddingEnabled {
        get { return PluginClass.CallStatic<bool>("isAdvancedBiddingEnabled"); }

        set { PluginClass.CallStatic("setAdvancedBiddingEnabled", value); }
    }


    /// See MoPubUnityEditor.<see cref="MoPubUnityEditor.EnableLocationSupport(bool)"/>
    public static void EnableLocationSupport(bool shouldUseLocation)
    {
        PluginClass.CallStatic("setLocationAwareness", LocationAwareness.NORMAL.ToString());
    }


    /// See MoPubUnityEditor.<see cref="MoPubUnityEditor.ReportApplicationOpen(string)"/>
    public static void ReportApplicationOpen(string iTunesAppId = null)
    {
        PluginClass.CallStatic("reportApplicationOpen");
    }


    /// See MoPubUnityEditor.<see cref="MoPubUnityEditor.GetSdkName"/>
    protected static string GetSdkName()
    {
        return "Android SDK v" + PluginClass.CallStatic<string>("getSDKVersion");
    }

    #endregion SdkSetup


    #region AndroidOnly

    /// <summary>
    /// The different kinds of location awareness that can be enabled for the SDK.
    /// </summary>
    /// <remarks>These enum names need to match the ones in the MoPub Android SDK, since we pass them by string value.
    /// </remarks>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public enum LocationAwareness
    {
        TRUNCATED,
        DISABLED,
        NORMAL
    }


    /// <summary>
    /// Registers the given device as a Facebook Ads test device.
    /// </summary>
    /// <param name="hashedDeviceId">String with the hashed ID of the device.</param>
    /// <remarks>See https://developers.facebook.com/docs/reference/android/current/class/AdSettings/ for details
    /// </remarks>
    public static void AddFacebookTestDeviceId(string hashedDeviceId)
    {
        PluginClass.CallStatic("addFacebookTestDeviceId", hashedDeviceId);
    }

    #endregion AndroidOnly


    #region Banners


    /// See MoPubUnityEditor.<see cref="MoPubUnityEditor.CreateBanner(string,MoPubBase.AdPosition,MoPubBase.BannerType)"/>
    public static void CreateBanner(string adUnitId, AdPosition position)
    {
        MPBanner plugin;
        if (BannerPluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.CreateBanner(position);
        else
            ReportAdUnitNotFound(adUnitId);
    }


    /// See MoPubUnityEditor.<see cref="MoPubUnityEditor.ShowBanner(string,bool)"/>
    public static void ShowBanner(string adUnitId, bool shouldShow)
    {
        MPBanner plugin;
        if (BannerPluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.ShowBanner(shouldShow);
        else
            ReportAdUnitNotFound(adUnitId);
    }


    /// See MoPubUnityEditor.<see cref="MoPubUnityEditor.RefreshBanner(string,string,string)"/>
    public static void RefreshBanner(string adUnitId, string keywords, string userDataKeywords = "")
    {
        MPBanner plugin;
        if (BannerPluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.RefreshBanner(keywords, userDataKeywords);
        else
            ReportAdUnitNotFound(adUnitId);
    }


    /// See MoPubUnityEditor.<see cref="MoPubUnityEditor.SetAutorefresh(string,bool)"/>
    public void SetAutorefresh(string adUnitId, bool enabled)
    {
        MPBanner plugin;
        if (BannerPluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.SetAutorefresh(enabled);
        else
            ReportAdUnitNotFound(adUnitId);
    }


    /// See MoPubUnityEditor.<see cref="MoPubUnityEditor.ForceRefresh(string)"/>
    public void ForceRefresh(string adUnitId)
    {
        MPBanner plugin;
        if (BannerPluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.ForceRefresh();
        else
            ReportAdUnitNotFound(adUnitId);
    }


    /// See MoPubUnityEditor.<see cref="MoPubUnityEditor.DestroyBanner(string)"/>
    public static void DestroyBanner(string adUnitId)
    {
        MPBanner plugin;
        if (BannerPluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.DestroyBanner();
        else
            ReportAdUnitNotFound(adUnitId);
    }


    #endregion Banners


    #region Interstitials


    /// See MoPubUnityEditor.<see cref="MoPubUnityEditor.RequestInterstitialAd(string,string,string)"/>
    public static void RequestInterstitialAd(string adUnitId, string keywords = "", string userDataKeywords = "")
    {
        MPInterstitial plugin;
        if (InterstitialPluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.RequestInterstitialAd(keywords, userDataKeywords);
        else
            ReportAdUnitNotFound(adUnitId);
    }


    /// See MoPubUnityEditor.<see cref="MoPubUnityEditor.ShowInterstitialAd(string)"/>
    public static void ShowInterstitialAd(string adUnitId)
    {
        MPInterstitial plugin;
        if (InterstitialPluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.ShowInterstitialAd();
        else
            ReportAdUnitNotFound(adUnitId);
    }


    /// See MoPubUnityEditor.<see cref="MoPubUnityEditor.IsInterstialReady(string)"/>
    public static bool IsInterstialReady(string adUnitId)
    {
        MPInterstitial plugin;
        if (InterstitialPluginsDict.TryGetValue(adUnitId, out plugin))
            return plugin.IsInterstitialReady;
        ReportAdUnitNotFound(adUnitId);
        return false;
    }


    /// See MoPubUnityEditor.<see cref="MoPubUnityEditor.DestroyInterstitialAd(string)"/>
    public static void DestroyInterstitialAd(string adUnitId)
    {
        MPInterstitial plugin;
        if (InterstitialPluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.DestroyInterstitialAd();
        else
            ReportAdUnitNotFound(adUnitId);
    }



    #endregion Interstitials


    #region RewardedVideos


    /// See MoPubUnityEditor.<see cref="MoPubUnityEditor.RequestRewardedVideo(string,System.Collections.Generic.List{MoPubBase.MediationSetting},string,string,double,double,string)"/>
    public static void RequestRewardedVideo(string adUnitId, List<MediationSetting> mediationSettings = null,
                                            string keywords = null, string userDataKeywords = null,
                                            double latitude = LatLongSentinel, double longitude = LatLongSentinel,
                                            string customerId = null)
    {
        MPRewardedVideo plugin;
        if (RewardedVideoPluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.RequestRewardedVideo(
                mediationSettings, keywords, userDataKeywords, latitude, longitude, customerId);
        else
            ReportAdUnitNotFound(adUnitId);
    }


    /// See MoPubUnityEditor.<see cref="MoPubUnityEditor.ShowRewardedVideo(string,string)"/>
    public static void ShowRewardedVideo(string adUnitId, string customData = null)
    {
        MPRewardedVideo plugin;
        if (RewardedVideoPluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.ShowRewardedVideo(customData);
        else
            ReportAdUnitNotFound(adUnitId);
    }


    /// See MoPubUnityEditor.<see cref="MoPubUnityEditor.HasRewardedVideo(string)"/>
    public static bool HasRewardedVideo(string adUnitId)
    {
        MPRewardedVideo plugin;
        if (RewardedVideoPluginsDict.TryGetValue(adUnitId, out plugin))
            return plugin.HasRewardedVideo();
        ReportAdUnitNotFound(adUnitId);
        return false;
    }



    /// See MoPubUnityEditor.<see cref="MoPubUnityEditor.GetAvailableRewards(string)"/>
    public static List<Reward> GetAvailableRewards(string adUnitId)
    {
        MPRewardedVideo plugin;
        if (RewardedVideoPluginsDict.TryGetValue(adUnitId, out plugin))
            return plugin.GetAvailableRewards();
        ReportAdUnitNotFound(adUnitId);
        return null;
    }


    /// See MoPubUnityEditor.<see cref="MoPubUnityEditor.SelectReward(string,MoPubBase.Reward)"/>
    public static void SelectReward(string adUnitId, Reward selectedReward)
    {
        MPRewardedVideo plugin;
        if (RewardedVideoPluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.SelectReward(selectedReward);
        else
            ReportAdUnitNotFound(adUnitId);
    }

    #endregion RewardedVideos


#if mopub_native_beta

    #region NativeAds


    public static void RequestNativeAd(string adUnitId)
    {
        MPNative plugin;
        if (NativePluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.RequestNativeAd();
        else
            ReportAdUnitNotFound(adUnitId);
    }

    #endregion NativeAds

#endif

    #region UserConsent


    /// See MoPubUnityEditor.<see cref="MoPubUnityEditor.CanCollectPersonalInfo"/>
    public static bool CanCollectPersonalInfo {
        get { return PluginClass.CallStatic<bool>("canCollectPersonalInfo"); }
    }


    /// See MoPubUnityEditor.<see cref="MoPubUnityEditor.CurrentConsentStatus"/>
    public static Consent.Status CurrentConsentStatus {
        get {
            return Consent.FromString(PluginClass.CallStatic<string>("getPersonalInfoConsentState"));
        }
    }


    /// See MoPubUnityEditor.<see cref="MoPubUnityEditor.ShouldShowConsentDialog"/>
    public static bool ShouldShowConsentDialog {
        get { return PluginClass.CallStatic<bool>("shouldShowConsentDialog"); }
    }


    /// See MoPubUnityEditor.<see cref="MoPubUnityEditor.LoadConsentDialog()"/>
    public static void LoadConsentDialog()
    {
        PluginClass.CallStatic("loadConsentDialog");
    }


    /// See MoPubUnityEditor.<see cref="MoPubUnityEditor.IsConsentDialogReady"/>
    public static bool IsConsentDialogReady {
        get { return PluginClass.CallStatic<bool>("isConsentDialogReady"); }
    }


    [Obsolete("Use the property name IsConsentDialogReady instead.")]
    public static bool IsConsentDialogLoaded {
        get { return IsConsentDialogReady; }
    }


    /// See MoPubUnityEditor.<see cref="MoPubUnityEditor.ShowConsentDialog()"/>
    public static void ShowConsentDialog()
    {
        PluginClass.CallStatic("showConsentDialog");
    }


    /// See MoPubUnityEditor.<see cref="MoPubUnityEditor.IsGdprApplicable"/>
    public static bool? IsGdprApplicable {
        get {
            var gdpr = PluginClass.CallStatic<int>("gdprApplies");
            return gdpr == 0 ? null : gdpr > 0 ? (bool?) true : false;
        }
    }


    /// See MoPubUnityEditor.<see cref="MoPubUnityEditor.ForceGdprApplicable"/>
    public static void ForceGdprApplicable() {
        PluginClass.CallStatic("forceGdprApplies");
    }


    /// See MoPubUnityEditor.<see cref="MoPubUnityEditor.PartnerApi"/>
    public static class PartnerApi
    {
        /// See MoPubUnityEditor.PartnerApi.<see cref="MoPubUnityEditor.PartnerApi.GrantConsent()"/>
        public static void GrantConsent()
        {
            PluginClass.CallStatic("grantConsent");
        }


        /// See MoPubUnityEditor.PartnerApi.<see cref="MoPubUnityEditor.PartnerApi.RevokeConsent()"/>
        public static void RevokeConsent()
        {
            PluginClass.CallStatic("revokeConsent");
        }


        /// See MoPubUnityEditor.PartnerApi.<see cref="MoPubUnityEditor.PartnerApi.CurrentConsentPrivacyPolicyUrl"/>
        public static Uri CurrentConsentPrivacyPolicyUrl {
            get {
                return UrlFromString(PluginClass.CallStatic<string>("getCurrentPrivacyPolicyLink", ConsentLanguageCode));
            }
        }


        /// See MoPubUnityEditor.PartnerApi.<see cref="MoPubUnityEditor.PartnerApi.CurrentVendorListUrl"/>
        public static Uri CurrentVendorListUrl {
            get {
                return UrlFromString(PluginClass.CallStatic<string>("getCurrentVendorListLink", ConsentLanguageCode));
            }
        }


        /// See MoPubUnityEditor.PartnerApi.<see cref="MoPubUnityEditor.PartnerApi.CurrentConsentIabVendorListFormat"/>
        public static string CurrentConsentIabVendorListFormat {
            get { return PluginClass.CallStatic<string>("getCurrentVendorListIabFormat"); }
        }


        /// See MoPubUnityEditor.PartnerApi.<see cref="MoPubUnityEditor.PartnerApi.CurrentConsentPrivacyPolicyVersion"/>
        public static string CurrentConsentPrivacyPolicyVersion {
            get { return PluginClass.CallStatic<string>("getCurrentPrivacyPolicyVersion"); }
        }


        /// See MoPubUnityEditor.PartnerApi.<see cref="MoPubUnityEditor.PartnerApi.CurrentConsentVendorListVersion"/>
        public static string CurrentConsentVendorListVersion {
            get { return PluginClass.CallStatic<string>("getCurrentVendorListVersion"); }
        }


        /// See MoPubUnityEditor.PartnerApi.<see cref="MoPubUnityEditor.PartnerApi.PreviouslyConsentedIabVendorListFormat"/>
        public static string PreviouslyConsentedIabVendorListFormat {
            get { return PluginClass.CallStatic<string>("getConsentedVendorListIabFormat"); }
        }


        /// See MoPubUnityEditor.PartnerApi.<see cref="MoPubUnityEditor.PartnerApi.PreviouslyConsentedPrivacyPolicyVersion"/>
        public static string PreviouslyConsentedPrivacyPolicyVersion {
            get { return PluginClass.CallStatic<string>("getConsentedPrivacyPolicyVersion"); }
        }


        /// See MoPubUnityEditor.PartnerApi.<see cref="MoPubUnityEditor.PartnerApi.PreviouslyConsentedVendorListVersion"/>
        public static string PreviouslyConsentedVendorListVersion {
            get { return PluginClass.CallStatic<string>("getConsentedVendorListVersion"); }
        }
    }

    #endregion UserConsent
}
