//  Blog:        http://blog.csdn.net/snowcoldgame
//  Author:      SnowCold
//  E-mail:      snowcold.ouyang@gmail.com
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Facebook.Unity;

namespace Qarth
{
	public class FBPhotoRes : AbstractRes
	{
		public const string PREFIX_KEY = "FBPhoto:";
		private string      m_Url;
		private string      m_HashCode;

		public static string CreatePhotoURL(string userID, int width, int height)
		{
			return string.Format("{0}?{1}&{2}", userID, width, height);
		}

		public static string URL2FBGetParam(string url)
		{
			int paramIndex = url.IndexOf('?');
			if (paramIndex < 0)
			{
				return string.Format("/{0}/picture", url);
			}

			string userID = url.Substring(0, paramIndex);
			string sizeParam = url.Substring(paramIndex + 1, url.Length - paramIndex - 1);
			int[] size = Helper.String2IntArray(sizeParam, "&");

			return string.Format("/{0}/picture?width={1}&height={2}", userID, size[0], size[1]);
		}

		public static FBPhotoRes Allocate(string name)
		{
			FBPhotoRes res = ObjectPool<FBPhotoRes>.S.Allocate();
			if (res != null)
			{
				res.name = name;
				res.SetUrl(name.Substring(8));
			}
			return res;
		}

		public string localResPath
		{
			get
			{
                return string.Format("{0}{1}", FilePath.persistentDataPath4Photo, m_HashCode);
            }
        }

		public bool needDownload
		{
			get
			{
				return refCount > 0;
			}
		}

		public string url
		{
			get
			{
				return m_Url;
			}
		}

		public int fileSize
		{
			get
			{
				return 1;
			}
		}

		public void SetUrl(string url)
		{
            if (string.IsNullOrEmpty(url))
			{
				return;
			}

			m_Url = url;
			m_HashCode = string.Format("FBPhoto_{0}", m_Url.GetHashCode());
		}

		public override bool UnloadImage(bool flag)
		{
			return false;
		}

		public override bool LoadSync()
		{
			return false;
		}

		public override void LoadAsync()
		{
			if (!CheckLoadAble())
			{
				return;
			}

			if (string.IsNullOrEmpty(m_Name))
			{
				return;
			}

			DoLoadWork();
		}

		protected override void OnReleaseRes()
		{
			if (m_Asset != null)
			{
				GameObject.Destroy(m_Asset);
				m_Asset = null;
			}
		}

		public override void Recycle2Cache()
		{
			ObjectPool<FBPhotoRes>.S.Recycle(this);
		}

		public override void OnCacheReset()
		{

		}

		public void DeleteOldResFile()
		{
			//throw new NotImplementedException();
		}

        private void DoLoadWork()
        {
            resState = eResState.kLoading;

            if (File.Exists(localResPath))
            {
                ResMgr.S.PostIEnumeratorTask(this);
            }
            else
            {
                FB.API(URL2FBGetParam(m_Url), HttpMethod.GET, OnProfilePhotoCallback);
            }
        }

		private void OnProfilePhotoCallback(IGraphResult result)
        {
            if (string.IsNullOrEmpty(result.Error) && result.Texture != null)
            {
                m_Asset = result.Texture;

                try
                {
					if (File.Exists(localResPath))
					{
						File.Delete(localResPath);
					}
                    byte[] bytes = result.Texture.EncodeToJPG();
                    File.WriteAllBytes(localResPath, bytes);
                }
                catch (Exception e)
                {
                    Log.e(e);
                    //如果保存出错，删除出错文件
                    if (File.Exists(localResPath))
                    {
                        File.Delete(localResPath);
                    }
                }

                resState = eResState.kReady;
            }
			else
			{
				OnResLoadFaild();
			}
            
        }

		protected override float CalculateProgress()
		{
			return 0;
		}

        public override IEnumerator StartIEnumeratorTask(Action finishCallback)
        {
            if (refCount <= 0)
            {
                OnResLoadFaild();
                finishCallback();
                yield break;
            }
#if UNITY_EDITOR
            WWW www = new WWW("file:///" + localResPath);
#else
            WWW www = new WWW("file://" + localResPath);
#endif
            yield return www;
            if (www.error != null)
            {
                Log.e("WWW Error:" + www.error);
                OnResLoadFaild();
                finishCallback();
                yield break;
            }

            if (!www.isDone)
            {
                Log.e("FBPhoto WWW Not Done! Url:" + m_Url);
                OnResLoadFaild();
                finishCallback();

                www.Dispose();
                www = null;

                yield break;
            }

            if (refCount <= 0)
            {
                OnResLoadFaild();
                finishCallback();

                www.Dispose();
                www = null;
                yield break;
            }

            Texture2D tex = www.texture;
            tex.Compress(true);

            m_Asset = tex;
            www.Dispose();
            www = null;

            resState = eResState.kReady;

            finishCallback();
        }
    }
}

