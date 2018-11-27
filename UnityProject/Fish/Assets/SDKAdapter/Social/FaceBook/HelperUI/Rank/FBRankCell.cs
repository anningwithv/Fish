using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Qarth
{
    public class FBRankCell : UListItemView
    {
        [SerializeField]
        private Text m_RankNum;
        [SerializeField]
        private Text m_NameLabel;
        [SerializeField]
        private Text m_MaxScore;
        [SerializeField]
        private FBPhotoView m_Icon;

        //隔行颜色控制
        [SerializeField]
        private Image m_SelfBackGround;
        [SerializeField]
        private Color m_FirstColor;
        [SerializeField]
        private Color m_ScenedColor;
        //前三名icon
        [SerializeField]
        private Image m_RankIcon;
        [SerializeField]
        private Image[] m_CrownImage;

        public void SetData(FacebookUserInfo data, int index)
        {
            m_SelfBackGround.color = index % 2 == 0 ? m_FirstColor : m_ScenedColor;
            if (index<3)
            {
                m_RankIcon.sprite = m_CrownImage[index].sprite;
                m_NameLabel.gameObject.SetActive(false);
                m_RankIcon.gameObject.SetActive(true);
            }
            else
            {
                m_RankIcon.gameObject.SetActive(false);
                m_NameLabel.gameObject.SetActive(true);
            }
            m_RankNum.text = "No." + (index + 1).ToString();
            m_NameLabel.text = data.userName;
            m_Icon.userBase = data;
            m_MaxScore.text = data.gameSocre.ToString();
        }
    }
}
