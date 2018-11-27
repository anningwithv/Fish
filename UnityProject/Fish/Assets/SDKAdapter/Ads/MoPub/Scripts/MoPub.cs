/// <summary>
/// This class is simply a proxy for calls to the platform-specific MoPub APIs.
/// For the full documented API, <see cref="MoPubUnityEditor"/>.
/// </summary>
public class MoPub :
// Choose base class based on target platform...
#if UNITY_EDITOR
    MoPubUnityEditor
#elif UNITY_ANDROID
    MoPubAndroid
#else
    MoPubiOS
#endif
{
    private static string _sdkName;

    public static string SdkName {
        get { return _sdkName ?? (_sdkName = GetSdkName().Replace("+unity", "")); }
    }
}
