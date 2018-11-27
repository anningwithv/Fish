using System.Collections;
using System.Collections.Generic;
using Qarth;
using UnityEngine;

namespace Qarth
{
	public class OutAdsAdapter : AbstractAdsAdapter 
	{
	    public override string adPlatform
	    {
	        get { return "outAds"; }
	    }

	    protected override bool DoAdapterInit(SDKConfig config, SDKAdapterConfig adapterConfig)
	    {
         //   AndroidJavaClass jc = new AndroidJavaClass("com.kids.adsdk.AdSdk");

	        //AndroidJavaObject context = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");

         //   AndroidJavaObject jo1 =  jc.CallStatic<AndroidJavaObject>("get",context);
         //   jo1.Call("init");

         //   jc = new AndroidJavaClass("com.kids.bcsdk.BcSdk");

	        //jc.CallStatic("init", context);

            return true;
	    }
	}
}

