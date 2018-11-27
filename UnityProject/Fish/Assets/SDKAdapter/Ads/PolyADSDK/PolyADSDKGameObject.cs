﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolyXXJSON;
using System;
namespace Polymer {

	public class PolyADSDKGameObject : MonoBehaviour {

		private readonly static string Function_Reward_DidOpen    = "reward_didopen";
		private readonly static string Function_Reward_DidClick   = "reward_didclick";
		private readonly static string Function_Reward_DidClose   = "reward_didclose";
		private readonly static string Function_Reward_DidGivien  = "reward_didgiven";
		private readonly static string Function_Reward_DidAbandon = "reward_didabandon";

		private readonly static string Function_Interstitial_Didshow  = "interstitial_didshow";
		private readonly static string Function_Interstitial_Didclose = "interstitial_didclose";
		private readonly static string Function_Interstitial_Didclick = "interstitial_didclick";

		private readonly static string Function_Banner_DidShow   = "banner_didshow";
		private readonly static string Function_Banner_DidClick  = "banner_didclick";
		private readonly static string Function_Banner_DidRemove = "banner_didremove";

		private readonly static string Function_Reward_DidLoadFail    = "reward_didloadfail";
		private readonly static string Function_Reward_DidLoadSuccess = "reward_didloadsuccess";

		private readonly static string Function_Interstitial_DidLoadFail    = "interstitial_didloadfail";
		private readonly static string Function_Interstitial_DidLoadSuccess = "interstitial_didloadsuccess";

		private readonly static string Function_Access_Privacy_Info_Accepted = "accepte_access_privacy_information";
		private readonly static string Function_Access_Privacy_Info_Defined  = "define_access_privacy_information";
		private readonly static string Function_Access_Privacy_Info_Failed   = "fail_access_privacy_information";

		private readonly static string Function_User_Is_European_User      = "user_is_european_union";
		private readonly static string Function_User_IsNot_European_User   = "user_not_is_european_union";


		#if UNITY_ANDROID && !UNITY_EDITOR
		private readonly static string Function_ExitAd_DidShow      = "exitad_didshow";
		private readonly static string Function_ExitAd_DidClick     = "exitad_didclick";
		private readonly static string Function_ExitAd_DidClickMore = "exitad_didclickmore";
		private readonly static string Function_ExitAd_DidExit      = "exitad_onexit";
		private readonly static string Function_ExitAd_DidCancel    = "exitad_oncancel";
		#endif

		private static PolyADSDKGameObject instance = null;
		public static readonly string GameObject_Callback_Name = "PolyAdSDK_Callback_Object";
		public static readonly string Java_Callback_Function = "onJavaCallback";

		 
		public static PolyADSDKGameObject getInstance()
		{
			if (instance == null) {
				GameObject polyCallback = new GameObject (GameObject_Callback_Name);
				polyCallback.hideFlags = HideFlags.HideAndDontSave;
				DontDestroyOnLoad (polyCallback);

				instance = polyCallback.AddComponent<PolyADSDKGameObject> ();
			}
			return instance;
		}

		//bool isPaused = false;

		Hashtable actionIntsFailMaps;
		Hashtable actionIntsSuccessMaps;
		Action<string, string> rewardVideoFailAction;
		Action<string, string> rewardVideoSuccessAction;
		Action<UPConstant.UPAccessPrivacyInfoStatusEnum, string> accessPrivacyInformationCallback;
		Action<bool, string> checkEuropeanUserCallback;

		List<string> cachedMessages = new List<string> (12);
		bool isAppFocus = false;

		bool enableCallbackAfterAppFocus = false;
		bool canObserverAppFocusCall = false;

		void OnGUI()
		{
			//Debug.Log ("===> Game onGUI Call");
		}

		void OnApplicationFocus(bool hasFocus)
		{
			Debug.Log ("===> OnApplicationFocus() hasFocus:" + hasFocus);
			isAppFocus = hasFocus;

			#if UNITY_ANDROID && !UNITY_EDITOR
			canObserverAppFocusCall = true;
			PolyADSDK.OnApplicationFocus (hasFocus);
			Debug.Log ("===> Game OnApplicationFocus Call, android hasFocus: " + hasFocus);
			#endif



		}

		void OnApplicationPause(bool pauseStatus)
		{
			Debug.Log ("===> OnApplicationPause() pauseStatus:" + pauseStatus);
			isAppFocus = !pauseStatus;
			#if UNITY_ANDROID && !UNITY_EDITOR
			canObserverAppFocusCall = true;
			Debug.Log ("===> Game OnApplicationFocus Call, android pauseStatus: " + pauseStatus);
			#endif

		}

		public void RunCallbackAfterAppFocus(bool enable) {
			this.enableCallbackAfterAppFocus = enable;
		}

		private void putLoadCallback(Hashtable map, string cpPlaceId, Action<string, string> call) {

			if (cpPlaceId == null || cpPlaceId.Length == 0) {
				return;
			}
			 
			if (map.ContainsKey (cpPlaceId)) {
				map.Remove (cpPlaceId);
			}

			if (call != null) {
				map.Add (cpPlaceId, call);
				//Debug.Log ("putLoadCallback function cpPlaceId:" + cpPlaceId);
			}
		}

		public void setAccessPrivacyInformationCallback(Action<UPConstant.UPAccessPrivacyInfoStatusEnum, string> callback) {
			accessPrivacyInformationCallback = callback;
		}

		public void setCheckEuropeanUserCallback(Action<bool, string> callback) {
			checkEuropeanUserCallback = callback;
		}

		public void setRewardVideoLoadFailCallback(Action<string, string> call) {
			this.rewardVideoFailAction = call;
		}

		public void setRewardVideoLoadSuccessCallback(Action<string, string> call) {
			this.rewardVideoSuccessAction = call;
		}

		public void addIntsLoadFailCallback(string cpPlaceId, Action<string, string> call) {
			 
			if (actionIntsFailMaps == null) {
				actionIntsFailMaps = new Hashtable ();
			}

			putLoadCallback (actionIntsFailMaps, cpPlaceId, call);
		}

		public void addIntsLoadSuccessCallback(string cpPlaceId, Action<string, string> call) {

			if (actionIntsSuccessMaps == null) {
				actionIntsSuccessMaps = new Hashtable ();
			}

			putLoadCallback (actionIntsSuccessMaps, cpPlaceId, call);
		}

		public void onJavaCallback(string message) {
			// Debug.Log ("===> onJavaCallback enableCallbackAfterAppFocus: " + enableCallbackAfterAppFocus +",canObserverAppFocusCall: " + canObserverAppFocusCall);
			if (enableCallbackAfterAppFocus) {
				if (canObserverAppFocusCall) {
					if (isAppFocus) {
						if (cachedMessages.Count > 0) {
							foreach (string s in cachedMessages) {
								doCallback (s);
							}
							cachedMessages.Clear();
						}
						doCallback (message);
					} else {
						cachedMessages.Add (message);
					}
				} else {
					Hashtable jsonObj = (Hashtable)PolyXXJSON.MiniJSON.jsonDecode (message);
					if (jsonObj.ContainsKey ("function")) {
						string function = (string)jsonObj ["function"];
						if (function.Equals (Function_Reward_DidOpen)
						    || function.Equals (Function_Reward_DidClick)
						    || function.Equals (Function_Reward_DidGivien)
							|| function.Equals (Function_Reward_DidAbandon)
							|| function.Equals (Function_Interstitial_Didshow)
							|| function.Equals (Function_Interstitial_Didclick)) {
							cachedMessages.Add (message);
						} else {
							if (function.Equals (Function_Reward_DidClose)
								|| function.Equals (Function_Interstitial_Didclose)) {
								if (cachedMessages.Count > 0) {
									foreach (string s in cachedMessages) {
										doCallback (s);
									}
									cachedMessages.Clear ();
								}
							}
							doCallback (message);
						}
					}

				}

			} else {
				doCallback (message);
			}

		}

		public void doCallback(string message) {
			Debug.Log (message);
			Hashtable jsonObj = (Hashtable)PolyXXJSON.MiniJSON.jsonDecode (message);
			if (jsonObj.ContainsKey ("function")) {
				string function = (string)jsonObj["function"];
				string msg = "";
				string placeId = "";
				if (jsonObj.ContainsKey ("function")) {
					msg = (string)jsonObj["message"];
					placeId = (string)jsonObj["cpadsid"];
				}
				Debug.Log ("===> function: " + function +",cpadsid: " + placeId);
				//reward callback
				if (function.Equals (Function_Reward_DidOpen)) {
					if (UPSDK.UPRewardDidOpenCallback != null) {
						Debug.Log ("===> function UPRewardDidOpenCallback(): ");
						UPSDK.UPRewardDidOpenCallback (placeId, msg);
					} else if (PolyADSDK.AvidlyRewardDidOpenCallback != null) {
						Debug.Log ("===> function AvidlyRewardDidOpenCallback(): ");
						PolyADSDK.AvidlyRewardDidOpenCallback (placeId, msg);
					} else {
						Debug.Log ("===> function call fail, no delegate object. ");
					}
				} else if (function.Equals (Function_Reward_DidClick)) {
					if (UPSDK.UPRewardDidClickCallback != null) {
						UPSDK.UPRewardDidClickCallback (placeId, msg);
					}
					else if (PolyADSDK.AvidlyRewardDidClickCallback != null) {
						PolyADSDK.AvidlyRewardDidClickCallback (placeId, msg);
					}
				} else if (function.Equals (Function_Reward_DidClose)) {
					if (UPSDK.UPRewardDidCloseCallback != null) {
						UPSDK.UPRewardDidCloseCallback (placeId, msg);
					}
					else if (PolyADSDK.AvidlyRewardDidCloseCallback != null) {
						PolyADSDK.AvidlyRewardDidCloseCallback (placeId, msg);
					}
				} else if (function.Equals (Function_Reward_DidGivien)) {
					if (UPSDK.UPRewardDidGivenCallback != null) {
						UPSDK.UPRewardDidGivenCallback (placeId, msg);
					}
					else if (PolyADSDK.AvidlyRewardDidGivenCallback != null) {
						PolyADSDK.AvidlyRewardDidGivenCallback (placeId, msg);
					}
				} else if (function.Equals (Function_Reward_DidAbandon)) {
					if (UPSDK.UPRewardDidAbandonCallback != null) {
						UPSDK.UPRewardDidAbandonCallback (placeId, msg);
					}
					else if (PolyADSDK.AvidlyRewardDidAbandonCallback != null) {
						PolyADSDK.AvidlyRewardDidAbandonCallback (placeId, msg);
					}
				}
				//Interstitial callback
				else if (function.Equals (Function_Interstitial_Didshow)) {
					if (UPSDK.UPInterstitialDidShowCallback != null) {
						UPSDK.UPInterstitialDidShowCallback (placeId, msg);
					}
					else if (PolyADSDK.AvidlyInterstitialDidShowCallback != null) {
						PolyADSDK.AvidlyInterstitialDidShowCallback (placeId, msg);
					}
				} else if (function.Equals (Function_Interstitial_Didclose)) {
					if (UPSDK.UPInterstitialDidCloseCallback != null) {
						UPSDK.UPInterstitialDidCloseCallback (placeId, msg);
					}
					else if (PolyADSDK.AvidlyInterstitialDidCloseCallback != null) {
						PolyADSDK.AvidlyInterstitialDidCloseCallback (placeId, msg);
					}
				} else if (function.Equals (Function_Interstitial_Didclick)) {
					if (UPSDK.UPInterstitialDidClickCallback != null) {
						UPSDK.UPInterstitialDidClickCallback (placeId, msg);
					}
					else if (PolyADSDK.AvidlyInterstitialDidClickCallback != null) {
						PolyADSDK.AvidlyInterstitialDidClickCallback (placeId, msg);
					}
				}
				//banner callback
				else if (function.Equals (Function_Banner_DidClick)) {
					if (UPSDK.UPBannerDidClickCallback != null) {
						UPSDK.UPBannerDidClickCallback (placeId, msg);
					}
					else if (PolyADSDK.AvidlyBannerDidClickCallback != null) {
						PolyADSDK.AvidlyBannerDidClickCallback (placeId, msg);
					}
				} else if (function.Equals (Function_Banner_DidShow)) {
					if (UPSDK.UPBannerDidShowCallback != null) {
						UPSDK.UPBannerDidShowCallback (placeId, msg);
					}
					else if (PolyADSDK.AvidlyBannerDidShowCallback != null) {
						PolyADSDK.AvidlyBannerDidShowCallback (placeId, msg);
					}
				} else if (function.Equals (Function_Banner_DidRemove)) {
					if (UPSDK.UPBannerDidRemoveCallback != null) {
						UPSDK.UPBannerDidRemoveCallback (placeId, msg);
					}
					else if (PolyADSDK.AvidlyBannerDidRemoveCallback != null) {
						PolyADSDK.AvidlyBannerDidRemoveCallback (placeId, msg);
					}
				}
				// exitad callback 
				#if UNITY_ANDROID && !UNITY_EDITOR
				else if (function.Equals (Function_ExitAd_DidShow)) {
					if (UPSDK.UPExitAdDidShowCallback != null) {
						UPSDK.UPExitAdDidShowCallback (msg);
					}
					else if (PolyADSDK.AvidlyExitAdDidShowCallback != null) {
						PolyADSDK.AvidlyExitAdDidShowCallback (msg);
					}
				}
				else if (function.Equals (Function_ExitAd_DidCancel)) {
					if (UPSDK.UPExitAdOnCancelCallback != null) {
						UPSDK.UPExitAdOnCancelCallback (msg);
					}
					else if (PolyADSDK.AvidlyExitAdOnCancelCallback!= null) {
						PolyADSDK.AvidlyExitAdOnCancelCallback (msg);
					}
				}
				else if (function.Equals (Function_ExitAd_DidExit)) {
					if (UPSDK.UPExitAdOnExitCallback != null) {
						UPSDK.UPExitAdOnExitCallback (msg);
					}
					else if (PolyADSDK.AvidlyExitAdOnExitCallback!= null) {
						PolyADSDK.AvidlyExitAdOnExitCallback (msg);
					}
				}
				else if (function.Equals (Function_ExitAd_DidClick)) {
					if (UPSDK.UPExitAdDidClickCallback != null) {
						UPSDK.UPExitAdDidClickCallback (msg);
					}
					else if (PolyADSDK.AvidlyExitAdDidClickCallback!= null) {
						PolyADSDK.AvidlyExitAdDidClickCallback (msg);
					}
				}
				else if (function.Equals (Function_ExitAd_DidClickMore)) {
					if (UPSDK.UPExitAdDidClickMoreCallback != null) {
						UPSDK.UPExitAdDidClickMoreCallback (msg);
					}
					else if (PolyADSDK.AvidlyExitAdDidClickMoreCallback!= null) {
						PolyADSDK.AvidlyExitAdDidClickMoreCallback (msg);
					}
				}
				#endif
				// check European User Callback
				else if (function.Equals (Function_User_Is_European_User)) {
					if (checkEuropeanUserCallback != null) {
						checkEuropeanUserCallback (true, msg);
					}
					checkEuropeanUserCallback = null;
				}
				else if (function.Equals (Function_User_IsNot_European_User)) {
					if (checkEuropeanUserCallback != null) {
						checkEuropeanUserCallback (false, msg);
					}
					checkEuropeanUserCallback = null;
				}
				// access privacy information callback
				else if (function.Equals (Function_Access_Privacy_Info_Accepted)) {
					if (accessPrivacyInformationCallback != null) {
						accessPrivacyInformationCallback (UPConstant.UPAccessPrivacyInfoStatusEnum.UPAccessPrivacyInfoStatusAccepted, msg);
					}
					accessPrivacyInformationCallback = null;
				}
				else if (function.Equals (Function_Access_Privacy_Info_Defined)) {
					if (accessPrivacyInformationCallback != null) {
						accessPrivacyInformationCallback (UPConstant.UPAccessPrivacyInfoStatusEnum.UPAccessPrivacyInfoStatusDefined, msg);
					}
					accessPrivacyInformationCallback = null;
				}
				else if (function.Equals (Function_Access_Privacy_Info_Failed)) {
					if (accessPrivacyInformationCallback != null) {
						accessPrivacyInformationCallback (UPConstant.UPAccessPrivacyInfoStatusEnum.UPAccessPrivacyInfoStatusFailed, msg);
					}
					accessPrivacyInformationCallback = null;
				}
				// load callback
				else if (function.Equals (Function_Reward_DidLoadFail)) {
					if (rewardVideoFailAction != null) {
						rewardVideoFailAction (placeId, msg);
					}
				}
				else if (function.Equals (Function_Reward_DidLoadSuccess)) {
					if (rewardVideoSuccessAction != null) {
						rewardVideoSuccessAction (placeId, msg);
					}
				}
				else if (function.Equals (Function_Interstitial_DidLoadFail)) {
					if (actionIntsFailMaps != null && placeId != null && actionIntsFailMaps.ContainsKey (placeId)) {
						Action<string, String> action = (Action<string, String>)actionIntsFailMaps [placeId];
						if (action != null) {
							action (placeId, msg);
						}
					}
				}
				else if (function.Equals (Function_Interstitial_DidLoadSuccess)) {
					if (actionIntsSuccessMaps != null && placeId != null && actionIntsSuccessMaps.ContainsKey (placeId)) {
						Action<string, String> action = (Action<string, String>)actionIntsSuccessMaps [placeId];
						if (action != null) {
							action (placeId, msg);
						}
					}
				}
				//unkown call
				else {
					Debug.Log ("unkown function:" + function);
				}
			}
		}
	}

}

