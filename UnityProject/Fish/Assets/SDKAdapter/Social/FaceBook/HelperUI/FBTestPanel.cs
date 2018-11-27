using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

namespace Qarth
{
    public class FBTestPanel : AbstractPanel
    {
        [SerializeField]
        private Button m_LoginButton;
        [SerializeField]
        private Button m_OpenInvitePanelButton;
        [SerializeField]
        private Button m_OpenSharePanelButton;
        [SerializeField]
        private Button m_OpenRankPanelButton;
        [SerializeField]
        private Button m_OpenScoreTestButton;
        [SerializeField]
        private Button m_LikeButton;

        protected override void OnUIInit()
        {
            m_LoginButton.onClick.AddListener(OnClickLoginButton);
            m_OpenSharePanelButton.onClick.AddListener(OnClickShareButton);
            m_OpenRankPanelButton.onClick.AddListener(OnClickOpenRankButton);
            m_OpenScoreTestButton.onClick.AddListener(OnClickOpenScoreTestButton);
            m_LikeButton.onClick.AddListener(OnClickLikeButton);
        }

        private void OnClickOpenRankButton()
        {
            //UIMgr.S.OpenPanel(EngineUI.FBRankTestPanel);
        }

        private void OnClickOpenScoreTestButton()
        {

        }

        private void OnClickLikeButton()
        {

        }

        private void OnClickShareButton()
        {
            Log.i("This is the share Button Click");
            //FacebookSocialAdapter.S.ShareContext(new Uri("http://forum.china.unity3d.com/thread-23091-1-1.html"), "Unity新手至高手都在关注的267篇Unity中文教程合辑","Some some some some some",new Uri("https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1515508336876&di=cc5953cc91a60de5199911d6288465b1&imgtype=0&src=http%3A%2F%2Fwww.taopic.com%2Fuploads%2Fallimg%2F140421%2F318743-140421213T910.jpg"),OnShareCallBack);
        }

        private void OnClickLoginButton()
        {
            
        }

        private void OnLogin(bool isLogin)
        {
            //CloseSelfPanel();
            if (isLogin)
            {
                Log.i("init Icon");
                //transform.Find("UserIcon").GetComponent<RawImage>().texture = FacebookSocialAdapter.S.userTexture;
                transform.Find("UserIcon").GetComponent<NetImageView>().prefixKey = FBPhotoRes.PREFIX_KEY;
                transform.Find("UserIcon").GetComponent<NetImageView>().imageUrl = FacebookSocialAdapter.S.selfUserID;
            }
        }
    }
}
