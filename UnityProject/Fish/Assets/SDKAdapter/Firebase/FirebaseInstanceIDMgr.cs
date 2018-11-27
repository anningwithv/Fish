using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Qarth
{
	public class FirebaseInstanceIDMgr : TSingleton<FirebaseInstanceIDMgr> 
	{
		public void Init()
		{
			Log.i("FirebaseInstanceID Start");
			SDKMgr.S.RegisterFilebaseDepInitCB(()=>
			{				
				GetInstanceID();	
				Log.i("FirebaseInstanceID Init Success");			
			});
			Log.i("FirebaseInstanceID end");
		}

        private void GetInstanceID()
        {
            Firebase.InstanceId.FirebaseInstanceId.DefaultInstance.GetTokenAsync().ContinueWith(task => 
			{
    			if (!(task.IsCanceled || task.IsFaulted) && task.IsCompleted) 
				{
      				Log.i(System.String.Format("Instance ID Token {0}", task.Result));
    			}
  			});
        }
    }
}