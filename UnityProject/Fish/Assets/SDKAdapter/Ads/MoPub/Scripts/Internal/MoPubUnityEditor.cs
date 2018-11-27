#if UNITY_EDITOR

using System;
using System.Collections;
using System.Collections.Generic;
using MoPubInternal.ThirdParty.MiniJSON;
using UnityEngine;

/// <summary>
/// Stub implementation with mock methods for Editor runs to prevent incompatibility issues.
/// This class also serves as the central location for MoPub Unity API documentation.
/// <para>
/// For platform-specific implementations, see
/// <see cref="MoPubAndroid"/> and
/// <see cref="MoPubiOS"/>.
/// </para>
/// </summary>
/// <remarks>
/// Some properties have added public setters in order to facilitate testing in Play mode.
/// </remarks>
public class MoPubUnityEditor : MoPubBase
{
    static MoPubUnityEditor()
    {
        InitManager();
    }


    #region SdkSetup


    /// <summary>
    /// Asynchronously initializes the relevant (Android or iOS) MoPub SDK.
    /// See <see cref="MoPubManager.OnSdkInitializedEvent"/> for resulting triggered event.
    /// <para>
    /// For platform-specific implementations, see
    /// MoPubAndroid.<see cref="MoPubAndroid.InitializeSdk(string)"/> and
    /// MoPubiOS.<see cref="MoPubiOS.InitializeSdk(string)"/>.
    /// </para>
    /// </summary>
    /// <param name="anyAdUnitId">String with any ad unit id used by this app.</param>
    /// <remarks>The MoPub SDK needs to be initialized on Start() to ensure all other objects have been enabled first.
    /// (Start() rather than Awake() so that MoPubManager has had time to Awake() and OnEnable() in order to receive
    /// event callbacks.)</remarks>
    public static void InitializeSdk(string anyAdUnitId)
    {
        InitializeSdk(new SdkConfiguration { AdUnitId = anyAdUnitId });
    }


    /// <summary>
    /// Asynchronously initializes the relevant (Android or iOS) MoPub SDK. Call this before making any rewarded ads or
    /// advanced bidding requests. This will do the rewarded video custom event initialization any number of times, but
    /// the SDK itself can only be initialized once, and the rewarded ads module can only be initialized once.
    /// See <see cref="MoPubManager.OnSdkInitializedEvent"/> for the resulting triggered event.
    /// <para>
    /// For platform-specific implementations, see
    /// MoPubAndroid.<see cref="MoPubAndroid.InitializeSdk(string)"/> and
    /// MoPubiOS.<see cref="MoPubiOS.InitializeSdk(string)"/>.
    /// </para>
    /// </summary>
    /// <param name="sdkConfiguration">The configuration including at least an ad unit.
    /// See <see cref="MoPubBase.SdkConfiguration"/> for details.</param>
    /// <remarks>The MoPub SDK needs to be initialized on Start() to ensure all other objects have been enabled first.
    /// (Start() rather than Awake() so that MoPubManager has had time to Awake() and OnEnable() in order to receive
    /// event callbacks.)</remarks>
    public static void InitializeSdk(SdkConfiguration sdkConfiguration)
    {
        WaitOneFrame(() => {
            _isInitialized = true;
            MoPubManager.Instance.EmitSdkInitializedEvent(ArgsToJson(sdkConfiguration.AdUnitId));
        });
    }


    /// <summary>
    /// Initializes a platform-specific MoPub SDK plugin for each given ad unit.
    /// <para>
    /// For platform-specific implementations, see
    /// MoPubAndroid.<see cref="MoPubAndroid.LoadBannerPluginsForAdUnits(string[])"/> and
    /// MoPubiOS.<see cref="MoPubiOS.LoadBannerPluginsForAdUnits(string[])"/>.
    /// </para>
    /// </summary>
    /// <param name="bannerAdUnitIds">The ad units to initialize plugins for</param>
    public static void LoadBannerPluginsForAdUnits(string[] bannerAdUnitIds) { }


    /// <summary>
    /// Initializes a platform-specific MoPub SDK plugin for each given ad unit.
    /// <para>
    /// For platform-specific implementations, see
    /// MoPubAndroid.<see cref="MoPubAndroid.LoadInterstitialPluginsForAdUnits(string[])"/> and
    /// MoPubiOS.<see cref="MoPubiOS.LoadInterstitialPluginsForAdUnits(string[])"/>.
    /// </para>
    /// </summary>
    /// <param name="interstitialAdUnitIds">The ad units to initialize plugins for</param>
    public static void LoadInterstitialPluginsForAdUnits(string[] interstitialAdUnitIds) { }


    /// <summary>
    /// Initializes a platform-specific MoPub SDK plugin for each given ad unit.
    /// <para>
    /// For platform-specific implementations, see
    /// MoPubAndroid.<see cref="MoPubAndroid.LoadRewardedVideoPluginsForAdUnits(string[])"/> and
    /// MoPubiOS.<see cref="MoPubiOS.LoadRewardedVideoPluginsForAdUnits(string[])"/>.
    /// </para>
    /// </summary>
    /// <param name="rewardedVideoAdUnitIds">The ad units to initialize plugins for</param>
    public static void LoadRewardedVideoPluginsForAdUnits(string[] rewardedVideoAdUnitIds) { }


#if mopub_native_beta
    /// <summary>
    /// Initializes a platform-specific MoPub SDK plugin for each given ad unit.
    /// <para>
    /// For platform-specific implementations, see
    /// MoPubAndroid.<see cref="MoPubAndroid.LoadNativePluginsForAdUnits(string[])"/> and
    /// MoPubiOS.<see cref="MoPubiOS.LoadNativePluginsForAdUnits(string[])"/>.
    /// </para>
    /// </summary>
    /// <param name="nativeAdUnitIds">The ad units to initialize plugins for</param>
    public static void LoadNativePluginsForAdUnits(string[] nativeAdUnitIds) { }
#endif


    /// <summary>
    /// Flag indicating if the SDK has been initialized.
    /// <para>
    /// For platform-specific implementations, see
    /// MoPubAndroid.<see cref="MoPubAndroid.IsSdkInitialized"/> and
    /// MoPubiOS.<see cref="MoPubiOS.IsSdkInitialized"/>.
    /// </para>
    /// </summary>
    /// <returns>true if a call to initialize the SDK has been made; false otherwise.</returns>
    public static bool IsSdkInitialized {
        get { return _isInitialized; }
    }


    /// <summary>
    /// Flag indicating advanced bidding has been enabled.
    /// <para>
    /// For platform-specific implementations, see
    /// MoPubAndroid.<see cref="MoPubAndroid.AdvancedBiddingEnabled"/> and
    /// MoPubiOS.<see cref="MoPubiOS.AdvancedBiddingEnabled"/>.
    /// </para>
    /// </summary>
    /// <returns>true if the SDK was initialized with advanced bidders; false otherwise.</returns>
    public static bool AdvancedBiddingEnabled { get; /* Testing: */ set; }


    /// <summary>
    /// Enables or disables location support for banners and interstitials.
    /// <para>
    /// For platform-specific implementations, see
    /// MoPubAndroid.<see cref="MoPubAndroid.EnableLocationSupport(bool)"/> and
    /// MoPubiOS.<see cref="MoPubiOS.EnableLocationSupport(bool)"/>.
    /// </para>
    /// </summary>
    /// <param name="shouldUseLocation">Whether location should be enabled or not.</param>
    public static void EnableLocationSupport(bool shouldUseLocation) { }


    /// <summary>
    /// Reports an app download to MoPub.
    /// <para>
    /// For platform-specific implementations, see
    /// MoPubAndroid.<see cref="MoPubAndroid.ReportApplicationOpen(string)"/> and
    /// MoPubiOS.<see cref="MoPubiOS.ReportApplicationOpen(string)"/>.
    /// </para>
    /// </summary>
    /// <param name="iTunesAppId">The app id on the App Store (only applicable to iOS).</param>
    public static void ReportApplicationOpen(string iTunesAppId = null) { }


    /// <summary>
    /// Returns a human-readable string of the MoPub SDK being used.
    /// <para>
    /// For platform-specific implementations, see
    /// MoPubAndroid.<see cref="MoPubAndroid.GetSdkName"/> and
    /// MoPubiOS.<see cref="MoPubiOS.GetSdkName"/>.
    /// </para>
    /// </summary>
    /// <returns>A string with the MoPub SDK platform and version.</returns>
    protected static string GetSdkName()
    {
        return "no SDK loaded (not on a mobile device)";
    }


    #endregion SdkSetup


    #region Banners


    /// <summary>
    /// Requests a banner ad and immediately shows it once loaded.
    /// <para>
    /// For platform-specific implementations, see
    /// MoPubAndroid.<see cref="MoPubAndroid.CreateBanner(string,MoPubBase.AdPosition)"/>
    /// and MoPubiOS.<see cref="MoPubiOS.CreateBanner(string, MoPubBase.AdPosition,MoPubBase.BannerType)"/>.
    /// </para>
    /// </summary>
    /// <param name="adUnitId">A string with the ad unit id.</param>
    /// <param name="position">Where in the screen to position the loaded ad. See <see cref="MoPubBase.AdPosition"/>.
    /// </param>
    /// <param name="bannerType">The size of the banner to load (only applicable to iOS).
    /// See <see cref="MoPubBase.BannerType"/>.</param>
    public static void CreateBanner(string adUnitId, AdPosition position, BannerType bannerType = BannerType.Size320x50)
    {
        RequestAdUnit(adUnitId);
        ForceRefresh(adUnitId);
    }


    /// <summary>
    /// Shows or hides an already-loaded banner ad.
    /// <para>
    /// For platform-specific implementations, see
    /// MoPubAndroid.<see cref="MoPubAndroid.ShowBanner(string,bool)"/> and
    /// MoPubiOS.<see cref="MoPubiOS.ShowBanner(string,bool)"/>.
    /// </para>
    /// </summary>
    /// <param name="adUnitId">A string with the ad unit id.</param>
    /// <param name="shouldShow">A bool with `true` to show the ad, or `false` to hide it.</param>
    /// <remarks>Banners are automatically shown after first loading.</remarks>
    public static void ShowBanner(string adUnitId, bool shouldShow)
    {
        CheckAdUnitRequested(adUnitId);
    }


    /// <summary>
    /// Sets the desired keywords and reloads the banner ad.
    /// <para>
    /// For platform-specific implementations, see
    /// MoPubAndroid.<see cref="MoPubAndroid.RefreshBanner(string,string,string)"/> and
    /// MoPubiOS.<see cref="MoPubiOS.RefreshBanner(string,string,string)"/>.
    /// </para>
    /// </summary>
    /// <param name="adUnitId">A string with the ad unit id.</param>
    /// <param name="keywords">A comma-separated string with the desired keywords for this ad.</param>
    /// <param name="userDataKeywords">An optional comma-separated string with user data for this ad.</param>
    /// <remarks>If a user is in a General Data Protection Regulation (GDPR) region and MoPub doesn't obtain consent
    /// from the user, "keywords" will be sent to the server but "userDataKeywords" will be excluded.
    /// (See <see cref="CanCollectPersonalInfo"/>).</remarks>
    public static void RefreshBanner(string adUnitId, string keywords, string userDataKeywords = "")
    {
        ForceRefresh(adUnitId);
    }


    /// <summary>
    /// Enables or disables banners automatically refreshing every 30 seconds.
    /// <para>
    /// For platform-specific implementations, see
    /// MoPubAndroid.<see cref="MoPubAndroid.SetAutorefresh(string,bool)"/> and
    /// MoPubiOS.<see cref="MoPubiOS.SetAutorefresh(string,bool)"/>.
    /// </para>
    /// </summary>
    /// <param name="adUnitId">A string with the ad unit id.</param>
    /// <param name="enabled">Whether to enable or disable autorefresh.</param>
    public static void SetAutorefresh(string adUnitId, bool enabled) { }


    /// <summary>
    /// Refreshes the banner ad regardless of whether autorefresh is enabled or not.
    /// <para>
    /// For platform-specific implementations, see
    /// MoPubAndroid.<see cref="MoPubAndroid.ForceRefresh(string)"/> and
    /// MoPubiOS.<see cref="MoPubiOS.ForceRefresh(string)"/>.
    /// </para>
    /// </summary>
    /// <param name="adUnitId">A string with the ad unit id.</param>
    public static void ForceRefresh(string adUnitId)
    {
        CheckAdUnitRequested(adUnitId);
        WaitOneFrame(() => { MoPubManager.Instance.EmitAdLoadedEvent(ArgsToJson(adUnitId, "50")); });
    }


    /// <summary>
    /// Destroys the banner ad and removes it from the view.
    /// <para>
    /// For platform-specific implementations, see
    /// MoPubAndroid.<see cref="MoPubAndroid.DestroyBanner(string)"/> and
    /// MoPubiOS.<see cref="MoPubiOS.DestroyBanner(string)"/>.
    /// </para>
    /// </summary>
    /// <param name="adUnitId">A string with the ad unit id.</param>
    public static void DestroyBanner(string adUnitId)
    {
        CheckAdUnitRequested(adUnitId);
    }

    #endregion Banners


    #region Interstitials


    /// <summary>
    /// Requests an interstitial ad with the given (optional) keywords to be loaded. The two possible resulting events
    /// are <see cref="MoPubManager.OnInterstitialLoadedEvent"/> and
    /// <see cref="MoPubManager.OnInterstitialFailedEvent"/>.
    /// <para>
    /// For platform-specific implementations, see
    /// MoPubAndroid.<see cref="MoPubAndroid.RequestInterstitialAd(string,string,string)"/> and
    /// MoPubiOS.<see cref="MoPubiOS.RequestInterstitialAd(string,string,string)"/>.
    /// </para>
    /// </summary>
    /// <param name="adUnitId">A string with the ad unit id.</param>
    /// <param name="keywords">An optional comma-separated string with the desired keywords for this ad.</param>
    /// <param name="userDataKeywords">An optional comma-separated string with user data for this ad.</param>
    /// <remarks>If a user is in a General Data Protection Regulation (GDPR) region and MoPub doesn't obtain consent
    /// from the user, "keywords" will be sent to the server but "userDataKeywords" will be excluded.
    /// (See <see cref="CanCollectPersonalInfo"/>).</remarks>
    public static void RequestInterstitialAd(string adUnitId, string keywords = "", string userDataKeywords = "")
    {
        RequestAdUnit(adUnitId);
        WaitOneFrame(() => { MoPubManager.Instance.EmitInterstitialLoadedEvent(ArgsToJson(adUnitId)); });
    }


    /// <summary>
    /// If the interstitial ad has loaded, this will take over the screen and show the ad.
    /// <para>
    /// For platform-specific implementations, see
    /// MoPubAndroid.<see cref="MoPubAndroid.ShowInterstitialAd(string)"/> and
    /// MoPubiOS.<see cref="MoPubiOS.ShowInterstitialAd(string)"/>.
    /// </para>
    /// </summary>
    /// <param name="adUnitId">A string with the ad unit id.</param>
    /// <remarks><see cref="MoPubManager.OnInterstitialLoadedEvent"/> must have been triggered already.</remarks>
    public static void ShowInterstitialAd(string adUnitId)
    {
        if (CheckAdUnitRequested(adUnitId))
            WaitOneFrame(() => { MoPubManager.Instance.EmitInterstitialShownEvent(ArgsToJson(adUnitId)); });
    }


    /// <summary>
    /// Whether the interstitial ad is ready to be shown or not.
    /// <para>
    /// For platform-specific implementations, see
    /// MoPubAndroid.<see cref="MoPubAndroid.IsInterstialReady(string)"/> and
    /// MoPubiOS.<see cref="MoPubiOS.IsInterstialReady(string)"/>.
    /// </para>
    /// </summary>
    /// <param name="adUnitId">A string with the ad unit id.</param>
    public static bool IsInterstialReady(string adUnitId)
    {
        CheckAdUnitRequested(adUnitId);
        return _requestedAdUnits.Contains(adUnitId);
    }


    /// <summary>
    /// Destroys an already-loaded interstitial ad.
    /// <para>
    /// For platform-specific implementations, see
    /// MoPubAndroid.<see cref="MoPubAndroid.DestroyInterstitialAd(string)"/> and
    /// MoPubiOS.<see cref="MoPubiOS.DestroyInterstitialAd(string)"/>.
    /// </para>
    /// </summary>
    /// <param name="adUnitId">A string with the ad unit id.</param>
    public static void DestroyInterstitialAd(string adUnitId)
    {
        CheckAdUnitRequested(adUnitId);
    }



    #endregion Interstitials


    #region RewardedVideos


    /// <summary>
    /// Requests an rewarded video ad with the given (optional) configuration to be loaded. The two possible resulting
    /// events are <see cref="MoPubManager.OnRewardedVideoLoadedEvent"/> and
    /// <see cref="MoPubManager.OnRewardedVideoFailedEvent"/>.
    /// <para>
    /// For platform-specific implementations, see
    /// MoPubAndroid.<see cref="MoPubAndroid.RequestRewardedVideo(string,System.Collections.Generic.List{MoPubBase.MediationSetting},string,string,double,double,string)"/> and
    /// MoPubiOS.<see cref="MoPubiOS.RequestRewardedVideo(string,System.Collections.Generic.List{MoPubBase.MediationSetting},string,string,double,double,string)"/>.
    /// </para>
    /// </summary>
    /// <param name="adUnitId">A string with the ad unit id.</param>
    /// <param name="mediationSettings">See <see cref="MoPubBase.SdkConfiguration.MediationSettings"/>.</param>
    /// <param name="keywords">An optional comma-separated string with the desired keywords for this ad.</param>
    /// <param name="userDataKeywords">An optional comma-separated string with user data for this ad.</param>
    /// <param name="latitude">An optional location latitude to be used for this ad.</param>
    /// <param name="longitude">An optional location longitude to be used for this ad.</param>
    /// <param name="customerId">An optional string to indentify this user within this app. </param>
    /// <remarks>If a user is in a General Data Protection Regulation (GDPR) region and MoPub doesn't obtain consent
    /// from the user, "keywords" will be sent to the server but "userDataKeywords" will be excluded.
    /// (See <see cref="CanCollectPersonalInfo"/>).</remarks>
    public static void RequestRewardedVideo(string adUnitId, List<MediationSetting> mediationSettings = null,
                                            string keywords = null, string userDataKeywords = null,
                                            double latitude = LatLongSentinel, double longitude = LatLongSentinel,
                                            string customerId = null)
    {
        RequestAdUnit(adUnitId);
        WaitOneFrame(() => { MoPubManager.Instance.EmitRewardedVideoLoadedEvent(ArgsToJson(adUnitId)); });
    }


    /// <summary>
    /// If the rewarded video ad has loaded, this will take over the screen and show the ad.
    /// <para>
    /// For platform-specific implementations, see
    /// MoPubAndroid.<see cref="MoPubAndroid.ShowRewardedVideo(string,string)"/> and
    /// MoPubiOS.<see cref="MoPubiOS.ShowRewardedVideo(string,string)"/>.
    /// </para>
    /// </summary>
    /// <param name="adUnitId">A string with the ad unit id.</param>
    /// <param name="customData">An optional string with custom data for the ad.</param>
    /// <remarks><see cref="MoPubManager.OnRewardedVideoLoadedEvent"/> must have been triggered already.</remarks>
    public static void ShowRewardedVideo(string adUnitId, string customData = null)
    {
        if (CheckAdUnitRequested(adUnitId))
            WaitOneFrame(() => { MoPubManager.Instance.EmitRewardedVideoShownEvent(ArgsToJson(adUnitId)); });
    }


    /// <summary>
    /// Whether a rewarded video is ready to play for this ad unit.
    /// <para>
    /// For platform-specific implementations, see
    /// MoPubAndroid.<see cref="MoPubAndroid.HasRewardedVideo(string)"/> and
    /// MoPubiOS.<see cref="MoPubiOS.HasRewardedVideo(string)"/>.
    /// </para>
    /// </summary>
    /// <param name="adUnitId">A string with the ad unit id.</param>
    /// <returns>`true` if a rewarded ad for the given ad unit id is loaded and ready to be shown; false othewise
    /// </returns>
    public static bool HasRewardedVideo(string adUnitId)
    {
        return _requestedAdUnits.Contains(adUnitId);
    }


    /// <summary>
    /// Retrieves a list of available rewards for the given ad unit id.
    /// <para>
    /// For platform-specific implementations, see
    /// MoPubAndroid.<see cref="MoPubAndroid.GetAvailableRewards(string)"/> and
    /// MoPubiOS.<see cref="MoPubiOS.GetAvailableRewards(string)"/>.
    /// </para>
    /// </summary>
    /// <param name="adUnitId">A string with the ad unit id.</param>
    /// <returns>A list of <see cref="MoPubBase.Reward"/>s for the given ad unit id.</returns>
    public static List<Reward> GetAvailableRewards(string adUnitId)
    {
        return new List<Reward>();
    }


    /// <summary>
    /// Selects the reward to give the user when the ad has finished playing.
    /// <para>
    /// For platform-specific implementations, see
    /// MoPubAndroid.<see cref="MoPubAndroid.SelectReward(string,MoPubBase.Reward)"/> and
    /// MoPubiOS.<see cref="MoPubiOS.SelectReward(string,MoPubBase.Reward)"/>.
    /// </para>
    /// </summary>
    /// <param name="adUnitId">A string with the ad unit id.</param>
    /// <param name="selectedReward">See <see cref="MoPubBase.Reward"/>.</param>
    public static void SelectReward(string adUnitId, Reward selectedReward) { }


    #endregion RewardedVideos


#if mopub_native_beta
    #region NativeAds


    public static void RequestNativeAd(string adUnitId) {
        RequestAdUnit(adUnitId);
        WaitOneFrame(() => {
            if (!"1".Equals(adUnitId))  {
                Debug.Log("Native ad unit was requested: " + adUnitId);
                return;
            }
            MoPubManager.Instance.EmitNativeLoadEvent(adUnitId, new AbstractNativeAd.Data {
                MainImageUrl =
                    new Uri("https://d30x8mtr3hjnzo.cloudfront.net/creatives/8d0a2ba02b2b485f97e1867366762951"),
                IconImageUrl =
                    new Uri("https://d30x8mtr3hjnzo.cloudfront.net/creatives/6591163c525f4720b99abf831ca247f6"),
                ClickDestinationUrl = new Uri("https://www.mopub.com/click-test"),
                CallToAction = "Go",
                Title = "MoPub",
                Text = "Success! Your integration is ready to go. Tap to test this ad.",
                PrivacyInformationIconClickThroughUrl = new Uri("https://www.mopub.com/optout/")
            });
        });
    }


    #endregion NativeAds
#endif


    #region UserConsent

    /// <summary>
    /// Whether or not this app is allowed to collect Personally Identifiable Information (PII) from the user.
    /// <para>
    /// For platform-specific implementations, see
    /// MoPubAndroid.<see cref="MoPubAndroid.CanCollectPersonalInfo"/> and
    /// MoPubiOS.<see cref="MoPubiOS.CanCollectPersonalInfo"/>.
    /// </para>
    /// </summary>
    public static bool CanCollectPersonalInfo { get; /* Testing: */ set; }


    private static Consent.Status _currentConsentStatus = Consent.Status.Unknown;

    /// <summary>
    /// The user's current consent state for the app to collect Personally Identifiable Information (PII).
    /// <see cref="MoPubBase.Consent.Status"> for the values and their meanings.
    /// <para>
    /// For platform-specific implementations, see
    /// MoPubAndroid.<see cref="MoPubAndroid.CurrentConsentStatus"/> and
    /// MoPubiOS.<see cref="MoPubiOS.CurrentConsentStatus"/>.
    /// </para>
    /// </summary>
    public static Consent.Status CurrentConsentStatus {
        get { return _currentConsentStatus; }
        /* Testing: */
        set {
            if (value == _currentConsentStatus)
                return;
            WaitOneFrame(() => {
                var old = _currentConsentStatus;
                _currentConsentStatus = value;
                MoPubManager.Instance.EmitConsentStatusChangedEvent(
                    ArgsToJson(old.ToString(), value.ToString(), CanCollectPersonalInfo ? "true" : "false"));
            });
        }
    }


    /// <summary>
    /// Checks to see if a publisher should load and then show a consent dialog.
    /// <para>
    /// For platform-specific implementations, see
    /// MoPubAndroid.<see cref="MoPubAndroid.ShouldShowConsentDialog"/> and
    /// MoPubiOS.<see cref="MoPubiOS.ShouldShowConsentDialog"/>.
    /// </para>
    /// </summary>
    public static bool ShouldShowConsentDialog {
        // Note: the logic below is for testing purposes.  It does not reflect the runtime production logic
        // (in particular, the call to a PartnerApi method.)
        get {
            _checkInitialized();
            return (IsGdprApplicable ?? false) && _currentConsentStatus == Consent.Status.Unknown;
        }
        // HACK for testing purposes only.  Use it to reinitialize an unknown consent state, or to skip the consent dialog.
        set {
            IsGdprApplicable = value;
            _currentConsentStatus = value ? Consent.Status.Unknown : Consent.Status.Consented;
        }
    }


    /// <summary>
    /// Sends off an asynchronous network request to load the MoPub consent dialog. The two possible resulting events
    /// are <see cref="MoPubManager.OnConsentDialogLoadedEvent"/> and
    /// <see cref="MoPubManager.OnConsentDialogFailedEvent"/>.
    /// <para>
    /// For platform-specific implementations, see
    /// MoPubAndroid.<see cref="MoPubAndroid.LoadConsentDialog()"/> and
    /// MoPubiOS.<see cref="MoPubiOS.LoadConsentDialog()"/>.
    /// </para>
    /// </summary>
    public static void LoadConsentDialog()
    {
        _checkInitialized();
        WaitOneFrame(() => {
            IsConsentDialogReady = true;
            MoPubManager.Instance.EmitConsentDialogLoadedEvent();
        });
    }


    /// <summary>
    /// Flag indicating whether the MoPub consent dialog is currently loaded and showable.
    /// <para>
    /// For platform-specific implementations, see
    /// MoPubAndroid.<see cref="MoPubAndroid.IsConsentDialogReady"/> and
    /// MoPubiOS.<see cref="MoPubiOS.IsConsentDialogReady"/>.
    /// </para>
    /// </summary>
    public static bool IsConsentDialogReady { get; /* Testing: */ set; }


    [Obsolete("Use the property name IsConsentDialogReady instead.")]
    public static bool IsConsentDialogLoaded {
        get { return IsConsentDialogReady; }
        set { IsConsentDialogReady = value; }
    }


    /// <summary>
    /// If the MoPub consent dialog is loaded, this will take over the screen and show it.
    /// <para>
    /// For platform-specific implementations, see
    /// MoPubAndroid.<see cref="MoPubAndroid.ShowConsentDialog()"/> and
    /// MoPubiOS.<see cref="MoPubiOS.ShowConsentDialog()"/>.
    /// </para>
    /// </summary>
    public static void ShowConsentDialog()
    {
        if (!IsConsentDialogReady) {
            Debug.LogError("Called ShowConsentDialog before consent dialog loaded!");
            return;
        }
        WaitOneFrame(() => {
            Debug.Log("When running on a mobile device, the consent dialog would appear now.");
            MoPubManager.Instance.EmitConsentDialogShownEvent();
        });
    }


    /// <summary>
    /// Flag indicating whether data collection is subject to the General Data Protection Regulation (GDPR).
    /// <para>
    /// For platform-specific implementations, see
    /// MoPubAndroid.<see cref="MoPubAndroid.IsGdprApplicable"/> and
    /// MoPubiOS.<see cref="MoPubiOS.IsGdprApplicable"/>.
    /// </para>
    /// </summary>
    /// <returns>
    /// True for Yes, False for No, Null for Unknown (from startup until server responds during SDK initialization).
    /// </returns>
    public static bool? IsGdprApplicable { get; /* Testing: */ set; }


    /// <summary>
    /// Forces the SDK to treat this app as in a GDPR region. Setting this will permanently force GDPR rules for this
    /// user unless this app is uninstalled or the data for this app is cleared.
    /// <para>
    /// For platform-specific implementations, see
    /// MoPubAndroid.<see cref="MoPubAndroid.ForceGdprApplicable"/> and
    /// MoPubiOS.<see cref="MoPubiOS.ForceGdprApplicable"/>.
    /// </para>
    /// </summary>
    public static void ForceGdprApplicable() { IsGdprApplicable = true; }


    /// <summary>
    /// API calls to be used by whitelisted publishers who are implementing their own consent dialog.
    /// <para>
    /// For platform-specific implementations, see
    /// MoPubAndroid.<see cref="MoPubAndroid.PartnerApi"/> and
    /// MoPubiOS.<see cref="MoPubiOS.PartnerApi"/>.
    /// </para>
    /// </summary>
    public static class PartnerApi
    {
        /// <summary>
        /// Notifies the MoPub SDK that this user has granted consent to this app.
        /// <para>
        /// For platform-specific implementations, see
        /// MoPubAndroid.PartnerApi.<see cref="MoPubAndroid.PartnerApi.GrantConsent()"/> and
        /// MoPubiOS.PartnerApi.<see cref="MoPubiOS.PartnerApi.GrantConsent()"/>.
        /// </para>
        /// </summary>
        public static void GrantConsent()
        {
            _checkInitialized();
            CurrentConsentStatus = Consent.Status.Consented;
            CanCollectPersonalInfo = true;
        }


        /// <summary>
        /// Notifies the MoPub SDK that this user has denied consent to this app.
        /// <para>
        /// For platform-specific implementations, see
        /// MoPubAndroid.PartnerApi.<see cref="MoPubAndroid.PartnerApi.RevokeConsent()"/> and
        /// MoPubiOS.PartnerApi.<see cref="MoPubiOS.PartnerApi.RevokeConsent()"/>.
        /// </para>
        /// </summary>
        public static void RevokeConsent()
        {
            _checkInitialized();
            CurrentConsentStatus = Consent.Status.Denied;
            CanCollectPersonalInfo = false;
        }


        /// <summary>
        /// The URL for the privacy policy this user has consented to.
        /// <para>
        /// For platform-specific implementations, see
        /// MoPubAndroid.PartnerApi.<see cref="MoPubAndroid.PartnerApi.CurrentConsentPrivacyPolicyUrl"/> and
        /// MoPubiOS.PartnerApi.<see cref="MoPubiOS.PartnerApi.CurrentConsentPrivacyPolicyUrl"/>.
        /// </para>
        /// </summary>
        public static Uri CurrentConsentPrivacyPolicyUrl { get; /* Testing: */ set; }


        /// <summary>
        /// The URL for the list of vendors this user has consented to.
        /// <para>
        /// For platform-specific implementations, see
        /// MoPubAndroid.PartnerApi.<see cref="MoPubAndroid.PartnerApi.CurrentVendorListUrl"/> and
        /// MoPubiOS.PartnerApi.<see cref="MoPubiOS.PartnerApi.CurrentVendorListUrl"/>.
        /// </para>
        /// </summary>
        public static Uri CurrentVendorListUrl { get; /* Testing: */ set; }


        /// <summary>
        /// The list of vendors this user has consented to in IAB format.
        /// <para>
        /// For platform-specific implementations, see
        /// MoPubAndroid.PartnerApi.<see cref="MoPubAndroid.PartnerApi.CurrentConsentIabVendorListFormat"/> and
        /// MoPubiOS.PartnerApi.<see cref="MoPubiOS.PartnerApi.CurrentConsentIabVendorListFormat"/>.
        /// </para>
        /// </summary>
        public static string CurrentConsentIabVendorListFormat { get; /* Testing: */ set; }


        /// <summary>
        /// The version for the privacy policy this user has consented to.
        /// <para>
        /// For platform-specific implementations, see
        /// MoPubAndroid.PartnerApi.<see cref="MoPubAndroid.PartnerApi.CurrentConsentPrivacyPolicyVersion"/> and
        /// MoPubiOS.PartnerApi.<see cref="MoPubiOS.PartnerApi.CurrentConsentPrivacyPolicyVersion"/>.
        /// </para>
        /// </summary>
        public static string CurrentConsentPrivacyPolicyVersion { get; /* Testing: */ set; }


        /// <summary>
        /// The version for the list of vendors this user has consented to.
        /// <para>
        /// For platform-specific implementations, see
        /// MoPubAndroid.PartnerApi.<see cref="MoPubAndroid.PartnerApi.CurrentConsentVendorListVersion"/> and
        /// MoPubiOS.PartnerApi.<see cref="MoPubiOS.PartnerApi.CurrentConsentVendorListVersion"/>.
        /// </para>
        /// </summary>
        public static string CurrentConsentVendorListVersion { get; /* Testing: */ set; }


        /// <summary>
        /// The list of vendors this user has previously consented to in IAB format.
        /// <para>
        /// For platform-specific implementations, see
        /// MoPubAndroid.PartnerApi.<see cref="MoPubAndroid.PartnerApi.PreviouslyConsentedIabVendorListFormat"/> and
        /// MoPubiOS.PartnerApi.<see cref="MoPubiOS.PartnerApi.PreviouslyConsentedIabVendorListFormat"/>.
        /// </para>
        /// </summary>
        public static string PreviouslyConsentedIabVendorListFormat { get; /* Testing: */ set; }


        /// <summary>
        /// The version for the privacy policy this user has previously consented to.
        /// <para>
        /// For platform-specific implementations, see
        /// MoPubAndroid.PartnerApi.<see cref="MoPubAndroid.PartnerApi.PreviouslyConsentedPrivacyPolicyVersion"/> and
        /// MoPubiOS.PartnerApi.<see cref="MoPubiOS.PartnerApi.PreviouslyConsentedPrivacyPolicyVersion"/>.
        /// </para>
        /// </summary>
        public static string PreviouslyConsentedPrivacyPolicyVersion { get; /* Testing: */ set; }


        /// <summary>
        /// The version for the vendor list this user has previously consented to.
        /// <para>
        /// For platform-specific implementations, see
        /// MoPubAndroid.PartnerApi.<see cref="MoPubAndroid.PartnerApi.PreviouslyConsentedVendorListVersion"/> and
        /// MoPubiOS.PartnerApi.<see cref="MoPubiOS.PartnerApi.PreviouslyConsentedVendorListVersion"/>.
        /// </para>
        /// </summary>
        public static string PreviouslyConsentedVendorListVersion { get; /* Testing: */ set; }
    }

    #endregion UserConsent


    #region MockEditor

    // Track whether the SDK has been initialized before use.
    private static bool _isInitialized;


    private static void _checkInitialized()
    {
        if (_isInitialized) return;
        Debug.LogWarning(
            "Make sure you initialize the MoPub SDK before loading an ad. For now, the SDK "
            + "will be automatically initialized on your behalf. Starting from release "
            + "5.2.0, initialization will be a strict requirement, and ad requests "
            + "made with an uninitialized SDK will begin to fail.");
        _isInitialized = true; // Stop the above warning from appearing more than once in the console.
    }


    // Track ad units that have been requested.
    private static readonly HashSet<string> _requestedAdUnits = new HashSet<string>();


    private static void RequestAdUnit(string adUnitId)
    {
        _checkInitialized();
        _requestedAdUnits.Add(adUnitId);
    }


    private static bool CheckAdUnitRequested(string adUnitId)
    {
        if (!_requestedAdUnits.Contains(adUnitId)) {
            Debug.LogError("Ad unit id has not been loaded: " + adUnitId);
            return false;
        }
        return true;
    }


    // Emulate the one-frame delay inherent in calling UnitySendMessage from Java/Objective-C
    // code in our native SDKs.
    private static IEnumerator WaitOneFrameCoroutine(Action action)
    {
        yield return null;
        action();
    }


    private static void WaitOneFrame(Action action)
    {
        MoPubManager.Instance.StartCoroutine(WaitOneFrameCoroutine(action));
    }


    private static string ArgsToJson(params string[] args)
    {
        return Json.Serialize(args);
    }


    #endregion MockEditor
}
#endif
