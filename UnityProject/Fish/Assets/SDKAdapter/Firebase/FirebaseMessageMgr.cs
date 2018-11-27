using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Messaging;
using UnityEngine;
using System.Threading.Tasks;

namespace Qarth
{

	public class FirebaseMessageMgr : TSingleton<FirebaseMessageMgr>
	{
		private bool m_IsInit;

		public void Init()
		{
            if (!SDKConfig.S.firebaseMessageConfig.isEnable)
            {
                return;
            }

			SDKMgr.S.RegisterFilebaseDepInitCB(()=>
			{
				InitializeFirebase();
				
				m_IsInit = true;
				Log.i("FirebaseMessageConfig init success");
			});
		}

		public void SubscribeTopic(string topic, Action<Task> task = null)
		{
			if(!m_IsInit)
			{
				return;
			}
			FirebaseMessaging.SubscribeAsync(topic).ContinueWith(task);
			Log.i("Register topic Success");
		}

		public void UnSubcribeTopic(string topic, Action<Task> task = null)
		{
			if(!m_IsInit)
			{
				return;
			}
			Firebase.Messaging.FirebaseMessaging.UnsubscribeAsync(topic).ContinueWith(task);
			Log.i("UnSubcribeTopic topic Success");
		}

		private void InitializeFirebase()
		{

			Firebase.Messaging.FirebaseMessaging.TokenRegistrationOnInitEnabled = false;
    		Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageReceived;    		
    		Firebase.Messaging.FirebaseMessaging.RequestPermissionAsync().ContinueWith(task => {
      			
			});
			
			RegisterDailyTopic();
		}

        private void OnTokenReceived(object sender, TokenReceivedEventArgs e)
        {
            
        }

        private void OnMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            
    		var notification = e.Message.Notification;
    		if (notification != null)
			{
				Log.e("Notification is null");
   	 		}
    		if (e.Message.From.Length > 0)
			{
				Log.i("Notification from "+ e.Message.From);
			}
     		 
    		if (e.Message.Link != null) {
        		
    		}
    		if (e.Message.Data.Count > 0)
			{     			
      			foreach (System.Collections.Generic.KeyValuePair<string, string> iter in e.Message.Data)
			    {
        			//DebugLog("  " + iter.Key + ": " + iter.Value);
      			}
    		}
        }

		private void RegisterDailyTopic()
		{
			string lastTopic = PlayerPrefs.GetString("DailyTopicKey","");
						
			Log.i("Already Register Topic" + lastTopic);
			if(!string.IsNullOrEmpty(lastTopic))
			{
				DateTime dt = Convert.ToDateTime(lastTopic);

				if(dt.Day == DateTime.Now.Day)
				{
					return;
				}
				else
				{
					UnSubcribeTopic(lastTopic);
				}				
			}
			string topic = DateTime.Today.Date.Year + "-" + DateTime.Today.Date.Month + "-" + DateTime.Today.Date.Day;
			SubscribeTopic(topic);
			PlayerPrefs.SetString("DailyTopicKey",topic);
			Log.i("register success");
		}

    }
}