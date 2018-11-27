using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using UnityEngine;
using MP = MoPubBinding;


/// <summary>
/// This class serves as a bridge to the MoPub iOS SDK (via the MoPub Unity iOS wrapper).
/// For full API documentation, See <see cref="MoPubUnityEditor"/>.
/// </summary>
[SuppressMessage("ReSharper", "UnusedMember.Global")]
public class MoPubiOS : MoPubBase
{
    static MoPubiOS()
    {
        InitManager();
    }


    private static readonly Dictionary<string, MP> PluginsDict = new Dictionary<string, MP>();


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
        _moPubInitializeSdk(
            sdkConfiguration.AdUnitId, sdkConfiguration.AdvancedBiddersString, sdkConfiguration.MediationSettingsJson,
            sdkConfiguration.NetworksToInitString);
    }


    /// See MoPubUnityEditor.<see cref="MoPubUnityEditor.LoadBannerPluginsForAdUnits(string[])"/>
    public static void LoadBannerPluginsForAdUnits(string[] adUnitIds)
    {
        LoadPluginsForAdUnits(adUnitIds);
    }


    /// See MoPubUnityEditor.<see cref="MoPubUnityEditor.LoadInterstitialPluginsForAdUnits(string[])"/>
    public static void LoadInterstitialPluginsForAdUnits(string[] adUnitIds)
    {
        LoadPluginsForAdUnits(adUnitIds);
    }


    /// See MoPubUnityEditor.<see cref="MoPubUnityEditor.LoadRewardedVideoPluginsForAdUnits(string[])"/>
    public static void LoadRewardedVideoPluginsForAdUnits(string[] adUnitIds)
    {
        LoadPluginsForAdUnits(adUnitIds);
    }


#if mopub_native_beta
    /// See MoPubUnityEditor.<see cref="MoPubUnityEditor.LoadNativePluginsForAdUnits(string[])"/>
    public static void LoadNativePluginsForAdUnits(string[] adUnitIds)
    {
        LoadPluginsForAdUnits(adUnitIds);
    }
#endif


    /// See MoPubUnityEditor.<see cref="MoPubUnityEditor.IsSdkInitialized"/>
    public static bool IsSdkInitialized {
        get { return _moPubIsSdkInitialized(); }
    }


    /// See MoPubUnityEditor.<see cref="MoPubUnityEditor.AdvancedBiddingEnabled"/>
    public static bool AdvancedBiddingEnabled {
        get { return _moPubIsAdvancedBiddingEnabled(); }

        set { _moPubSetAdvancedBiddingEnabled(value); }
    }


    /// See MoPubUnityEditor.<see cref="MoPubUnityEditor.EnableLocationSupport(bool)"/>
    public static void EnableLocationSupport(bool shouldUseLocation)
    {
        _moPubEnableLocationSupport(true);
    }


    /// See MoPubUnityEditor.<see cref="MoPubUnityEditor.ReportApplicationOpen(string)"/>
    public static void ReportApplicationOpen(string iTunesAppId = null)
    {
        _moPubReportApplicationOpen(iTunesAppId);
    }


    /// See MoPubUnityEditor.<see cref="MoPubUnityEditor.GetSdkName"/>
    protected static string GetSdkName()
    {
        return "iOS SDK v" + _moPubGetSDKVersion();
    }


    private static void LoadPluginsForAdUnits(string[] adUnitIds)
    {
        foreach (var adUnitId in adUnitIds)
            PluginsDict.Add(adUnitId, new MP(adUnitId));
        Debug.Log(adUnitIds.Length + " AdUnits loaded for plugins:\n" + string.Join(", ", adUnitIds));
    }

    #endregion SdkSetup


    #region iOSOnly

    /// <summary>
    /// MoPub SDK log level. The default value is `MPLogLevelInfo`.
    /// See MoPubBase.<see cref="MoPubBase.LogLevel"/> for all possible options.
    /// </summary>
    public static LogLevel SdkLogLevel {
        get { return (LogLevel) _moPubGetLogLevel(); }
        set { _moPubSetLogLevel((int) value); }
    }


    /// <summary>
    /// Forces the usage of WKWebView, if able.
    /// </summary>
    /// <param name="shouldForce">Whether to attempt to force the usage of WKWebView or not.</param>
    public static void ForceWKWebView(bool shouldForce)
    {
        _moPubForceWKWebView(shouldForce);
    }

    #endregion


    #region Banners


    /// See MoPubUnityEditor.<see cref="MoPubUnityEditor.CreateBanner(string,MoPubBase.AdPosition,MoPubBase.BannerType)"/>
    public static void CreateBanner(string adUnitId, AdPosition position, BannerType bannerType = BannerType.Size320x50)
    {
        MP plugin;
        if (PluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.CreateBanner(bannerType, position);
        else
            ReportAdUnitNotFound(adUnitId);
    }


    /// See MoPubUnityEditor.<see cref="MoPubUnityEditor.ShowBanner(string,bool)"/>
    public static void ShowBanner(string adUnitId, bool shouldShow)
    {
        MP plugin;
        if (PluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.ShowBanner(shouldShow);
        else
            ReportAdUnitNotFound(adUnitId);
    }


    /// See MoPubUnityEditor.<see cref="MoPubUnityEditor.RefreshBanner(string,string,string)"/>
    public static void RefreshBanner(string adUnitId, string keywords, string userDataKeywords = "")
    {
        MP plugin;
        if (PluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.RefreshBanner(keywords, userDataKeywords);
        else
            ReportAdUnitNotFound(adUnitId);
    }


    /// See MoPubUnityEditor.<see cref="MoPubUnityEditor.SetAutorefresh(string,bool)"/>
    public void SetAutorefresh(string adUnitId, bool enabled)
    {
        MP plugin;
        if (PluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.SetAutorefresh(enabled);
        else
            ReportAdUnitNotFound(adUnitId);
    }


    /// See MoPubUnityEditor.<see cref="MoPubUnityEditor.ForceRefresh(string)"/>
    public void ForceRefresh(string adUnitId)
    {
        MP plugin;
        if (PluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.ForceRefresh();
        else
            ReportAdUnitNotFound(adUnitId);
    }


    /// See MoPubUnityEditor.<see cref="MoPubUnityEditor.ForceRefresh(string)"/>
    public static void DestroyBanner(string adUnitId)
    {
        MP plugin;
        if (PluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.DestroyBanner();
        else
            ReportAdUnitNotFound(adUnitId);
    }


    #endregion Banners


    #region Interstitials


    /// See MoPubUnityEditor.<see cref="MoPubUnityEditor.RequestInterstitialAd(string,string,string)"/>
    public static void RequestInterstitialAd(string adUnitId, string keywords = "", string userDataKeywords = "")
    {
        MP plugin;
        if (PluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.RequestInterstitialAd(keywords, userDataKeywords);
        else
            ReportAdUnitNotFound(adUnitId);
    }


    /// See MoPubUnityEditor.<see cref="MoPubUnityEditor.ShowInterstitialAd(string)"/>
    public static void ShowInterstitialAd(string adUnitId)
    {
        MP plugin;
        if (PluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.ShowInterstitialAd();
        else
            ReportAdUnitNotFound(adUnitId);
    }


    /// See MoPubUnityEditor.<see cref="MoPubUnityEditor.IsInterstialReady(string)"/>
    public bool IsInterstialReady(string adUnitId)
    {
        MP plugin;
        if (PluginsDict.TryGetValue(adUnitId, out plugin))
            return plugin.IsInterstitialReady;
        ReportAdUnitNotFound(adUnitId);
        return false;
    }


    /// See MoPubUnityEditor.<see cref="MoPubUnityEditor.DestroyInterstitialAd(string)"/>
    public void DestroyInterstitialAd(string adUnitId)
    {
        MP plugin;
        if (PluginsDict.TryGetValue(adUnitId, out plugin))
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
        MP plugin;
        if (PluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.RequestRewardedVideo(mediationSettings, keywords, userDataKeywords, latitude, longitude, customerId);
        else
            ReportAdUnitNotFound(adUnitId);
    }


    /// See MoPubUnityEditor.<see cref="MoPubUnityEditor.ShowRewardedVideo(string,string)"/>
    public static void ShowRewardedVideo(string adUnitId, string customData = null)
    {
        MP plugin;
        if (PluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.ShowRewardedVideo(customData);
        else
            ReportAdUnitNotFound(adUnitId);
    }


    /// See MoPubUnityEditor.<see cref="MoPubUnityEditor.HasRewardedVideo(string)"/>
    public static bool HasRewardedVideo(string adUnitId)
    {
        MP plugin;
        if (PluginsDict.TryGetValue(adUnitId, out plugin))
            return plugin.HasRewardedVideo();
        ReportAdUnitNotFound(adUnitId);
        return false;
    }


    /// See MoPubUnityEditor.<see cref="MoPubUnityEditor.GetAvailableRewards(string)"/>
    public static List<Reward> GetAvailableRewards(string adUnitId)
    {
        MP plugin;
        if (!PluginsDict.TryGetValue(adUnitId, out plugin)) {
            ReportAdUnitNotFound(adUnitId);
            return null;
        }

        var rewards = plugin.GetAvailableRewards();
        Debug.Log(string.Format("GetAvailableRewards found {0} rewards for ad unit {1}", rewards.Count, adUnitId));
        return rewards;
    }


    /// See MoPubUnityEditor.<see cref="MoPubUnityEditor.SelectReward(string,MoPubBase.Reward)"/>
    public static void SelectReward(string adUnitId, Reward selectedReward)
    {
        MP plugin;
        if (PluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.SelectedReward = selectedReward;
        else
            ReportAdUnitNotFound(adUnitId);
    }

    #endregion RewardedVideos


#if mopub_native_beta

    #region NativeAds


    public static void RequestNativeAd(string adUnitId) { }


    #endregion NativeAds

#endif


    #region UserConsent

    /// See MoPubUnityEditor.<see cref="MoPubUnityEditor.CanCollectPersonalInfo"/>
    public static bool CanCollectPersonalInfo {
        get { return _moPubCanCollectPersonalInfo(); }
    }


    /// See MoPubUnityEditor.<see cref="MoPubUnityEditor.CurrentConsentStatus"/>
    public static Consent.Status CurrentConsentStatus {
        get { return (Consent.Status) _moPubCurrentConsentStatus(); }
    }


    /// See MoPubUnityEditor.<see cref="MoPubUnityEditor.ShouldShowConsentDialog"/>
    public static bool ShouldShowConsentDialog {
        get { return _moPubShouldShowConsentDialog(); }
    }


    /// See MoPubUnityEditor.<see cref="MoPubUnityEditor.LoadConsentDialog()"/>
    public static void LoadConsentDialog()
    {
        _moPubLoadConsentDialog();
    }


    /// See MoPubUnityEditor.<see cref="MoPubUnityEditor.IsConsentDialogReady"/>
    public static bool IsConsentDialogReady {
        get { return _moPubIsConsentDialogReady(); }
    }


    [Obsolete("Use the property name IsConsentDialogReady instead.")]
    public static bool IsConsentDialogLoaded {
        get { return IsConsentDialogReady; }
    }


    /// See MoPubUnityEditor.<see cref="MoPubUnityEditor.ShowConsentDialog()"/>
    public static void ShowConsentDialog()
    {
        _moPubShowConsentDialog();
    }


    /// See MoPubUnityEditor.<see cref="MoPubUnityEditor.IsGdprApplicable"/>
    public static bool? IsGdprApplicable {
        get {
            var value = _moPubIsGDPRApplicable();
            return value == 0 ? null : value > 0 ? (bool?) true : false;
        }
    }


    /// See MoPubUnityEditor.<see cref="MoPubUnityEditor.ForceGdprApplicable"/>
    public static void ForceGdprApplicable() {
        _moPubForceGDPRApplicable();
    }


    /// See MoPubUnityEditor.<see cref="MoPubUnityEditor.PartnerApi"/>
    public static class PartnerApi
    {
        /// See MoPubUnityEditor.PartnerApi.<see cref="MoPubUnityEditor.PartnerApi.GrantConsent()"/>
        public static void GrantConsent()
        {
            _moPubGrantConsent();
        }


        /// See MoPubUnityEditor.PartnerApi.<see cref="MoPubUnityEditor.PartnerApi.RevokeConsent()"/>
        public static void RevokeConsent()
        {
            _moPubRevokeConsent();
        }


        /// See MoPubUnityEditor.PartnerApi.<see cref="MoPubUnityEditor.PartnerApi.CurrentConsentPrivacyPolicyUrl"/>
        public static Uri CurrentConsentPrivacyPolicyUrl {
            get {
                var url = _moPubCurrentConsentPrivacyPolicyUrl(ConsentLanguageCode);
                return UrlFromString(url);
            }
        }

        /// See MoPubUnityEditor.PartnerApi.<see cref="MoPubUnityEditor.PartnerApi.CurrentVendorListUrl"/>
        public static Uri CurrentVendorListUrl {
            get {
                var url = _moPubCurrentConsentVendorListUrl(ConsentLanguageCode);
                return UrlFromString(url);
            }
        }


        /// See MoPubUnityEditor.PartnerApi.<see cref="MoPubUnityEditor.PartnerApi.CurrentConsentIabVendorListFormat"/>
        public static string CurrentConsentIabVendorListFormat {
            get { return _moPubCurrentConsentIabVendorListFormat(); }
        }


        /// See MoPubUnityEditor.PartnerApi.<see cref="MoPubUnityEditor.PartnerApi.CurrentConsentPrivacyPolicyVersion"/>
        public static string CurrentConsentPrivacyPolicyVersion {
            get { return _moPubCurrentConsentPrivacyPolicyVersion(); }
        }


        /// See MoPubUnityEditor.PartnerApi.<see cref="MoPubUnityEditor.PartnerApi.CurrentConsentVendorListVersion"/>
        public static string CurrentConsentVendorListVersion {
            get { return _moPubCurrentConsentVendorListVersion(); }
        }


        /// See MoPubUnityEditor.PartnerApi.<see cref="MoPubUnityEditor.PartnerApi.PreviouslyConsentedIabVendorListFormat"/>
        public static string PreviouslyConsentedIabVendorListFormat {
            get { return _moPubPreviouslyConsentedIabVendorListFormat(); }
        }


        /// See MoPubUnityEditor.PartnerApi.<see cref="MoPubUnityEditor.PartnerApi.PreviouslyConsentedPrivacyPolicyVersion"/>
        public static string PreviouslyConsentedPrivacyPolicyVersion {
            get { return _moPubPreviouslyConsentedPrivacyPolicyVersion(); }
        }


        /// See MoPubUnityEditor.PartnerApi.<see cref="MoPubUnityEditor.PartnerApi.PreviouslyConsentedVendorListVersion"/>
        public static string PreviouslyConsentedVendorListVersion {
            get { return _moPubPreviouslyConsentedVendorListVersion(); }
        }
    }

    #endregion UserConsent


    #region DllImports
#if ENABLE_IL2CPP && UNITY_ANDROID
    // IL2CPP on Android scrubs DllImports, so we need to provide stubs to unblock compilation
    private static void _moPubInitializeSdk(string adUnitId, string advancedBiddersString,
                                            string mediationSettingsJson, string networksToInitString) {}
    private static bool _moPubIsSdkInitialized() { return false; }
    private static void _moPubSetAdvancedBiddingEnabled(bool advancedBiddingEnabled) {}
    private static bool _moPubIsAdvancedBiddingEnabled() { return false; }
    private static string _moPubGetSDKVersion() { return null; }
    private static void _moPubEnableLocationSupport(bool shouldUseLocation) {}
    private static int _moPubGetLogLevel() { return -1; }
    private static void _moPubSetLogLevel(int logLevel) {}
    private static void _moPubForceWKWebView(bool shouldForce) {}
    private static void _moPubReportApplicationOpen(string iTunesAppId) {}
    private static bool _moPubCanCollectPersonalInfo() { return false; }
    private static int _moPubCurrentConsentStatus() { return -1; }
    private static int _moPubIsGDPRApplicable() { return -1; }
    private static int _moPubForceGDPRApplicable() { return -1; }
    private static bool _moPubShouldShowConsentDialog() { return false; }
    private static bool _moPubIsConsentDialogReady() { return false; }
    private static void _moPubLoadConsentDialog() {}
    private static void _moPubShowConsentDialog() {}
    private static string _moPubCurrentConsentPrivacyPolicyUrl(string isoLanguageCode = null) { return null; }
    private static string _moPubCurrentConsentVendorListUrl(string isoLanguageCode = null) { return null; }
    private static void _moPubGrantConsent() {}
    private static void _moPubRevokeConsent() {}
    private static string _moPubCurrentConsentIabVendorListFormat() { return null; }
    private static string _moPubCurrentConsentPrivacyPolicyVersion() { return null; }
    private static string _moPubCurrentConsentVendorListVersion() { return null; }
    private static string _moPubPreviouslyConsentedIabVendorListFormat() { return null; }
    private static string _moPubPreviouslyConsentedPrivacyPolicyVersion() { return null; }
    private static string _moPubPreviouslyConsentedVendorListVersion() { return null; }
#else
    [DllImport("__Internal")]
    private static extern void _moPubInitializeSdk(string adUnitId, string advancedBiddersString,
                                                   string mediationSettingsJson, string networksToInitString);


    [DllImport("__Internal")]
    private static extern bool _moPubIsSdkInitialized();


    [DllImport("__Internal")]
    private static extern void _moPubSetAdvancedBiddingEnabled(bool advancedBiddingEnabled);


    [DllImport("__Internal")]
    private static extern bool _moPubIsAdvancedBiddingEnabled();


    [DllImport("__Internal")]
    private static extern string _moPubGetSDKVersion();


    [DllImport("__Internal")]
    private static extern void _moPubEnableLocationSupport(bool shouldUseLocation);


    [DllImport("__Internal")]
    private static extern int _moPubGetLogLevel();


    [DllImport("__Internal")]
    private static extern void _moPubSetLogLevel(int logLevel);


    [DllImport("__Internal")]
    private static extern void _moPubForceWKWebView(bool shouldForce);


    [DllImport("__Internal")]
    private static extern void _moPubReportApplicationOpen(string iTunesAppId);


    [DllImport("__Internal")]
    private static extern bool _moPubCanCollectPersonalInfo();


    [DllImport("__Internal")]
    private static extern int _moPubCurrentConsentStatus();


    [DllImport("__Internal")]
    private static extern int _moPubIsGDPRApplicable();


    [DllImport("__Internal")]
    private static extern int _moPubForceGDPRApplicable();


    [DllImport("__Internal")]
    private static extern bool _moPubShouldShowConsentDialog();


    [DllImport("__Internal")]
    private static extern bool _moPubIsConsentDialogReady();


    [DllImport("__Internal")]
    private static extern void _moPubLoadConsentDialog();


    [DllImport("__Internal")]
    private static extern void _moPubShowConsentDialog();


    [DllImport("__Internal")]
    private static extern string _moPubCurrentConsentPrivacyPolicyUrl(string isoLanguageCode = null);


    [DllImport("__Internal")]
    private static extern string _moPubCurrentConsentVendorListUrl(string isoLanguageCode = null);


    [DllImport("__Internal")]
    private static extern void _moPubGrantConsent();


    [DllImport("__Internal")]
    private static extern void _moPubRevokeConsent();


    [DllImport("__Internal")]
    private static extern string _moPubCurrentConsentIabVendorListFormat();


    [DllImport("__Internal")]
    private static extern string _moPubCurrentConsentPrivacyPolicyVersion();


    [DllImport("__Internal")]
    private static extern string _moPubCurrentConsentVendorListVersion();


    [DllImport("__Internal")]
    private static extern string _moPubPreviouslyConsentedIabVendorListFormat();


    [DllImport("__Internal")]
    private static extern string _moPubPreviouslyConsentedPrivacyPolicyVersion();


    [DllImport("__Internal")]
    private static extern string _moPubPreviouslyConsentedVendorListVersion();
#endif
    #endregion DllImports
}
