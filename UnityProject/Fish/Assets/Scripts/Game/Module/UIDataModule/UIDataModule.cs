using UnityEngine;
using System.Collections;
using Qarth;

namespace GameWish.Game
{
    public class UIDataModule : AbstractModule
    {
        public static void RegisterStaticPanel()
        {
            InitUIPath();
            UIDataTable.SetABMode(false);

            UIDataTable.AddPanelData(UIID.LogoPanel, null, "LogoPanel/LogoPanel");
        }

        protected override void OnComAwake()
        {
            InitUIPath();
            RegisterAllPanel();
        }

        private static void InitUIPath()
        {
            PanelData.PREFIX_PATH = "Resources/UI/Panels/{0}";
            PageData.PREFIX_PATH = "Resources/UI/Panels/{0}";
        }

        private void RegisterAllPanel()
        {
            UIDataTable.SetABMode(true);

            UIDataTable.AddPanelData(EngineUI.FloatMessagePanel, null, "Common/FloatMessagePanel", true, 1);
            UIDataTable.AddPanelData(EngineUI.MsgBoxPanel, null, "Common/MsgBoxPanel", true, 1);
            UIDataTable.AddPanelData(EngineUI.HighlightMaskPanel, null, "Guide/HighlightMaskPanel", true, 0);
			UIDataTable.AddPanelData(EngineUI.GuideHandPanel, null, "Guide/GuideHandPanel", true, 0);
            //UIDataTable.AddPanelData(EngineUI.MaskPanel, null, "Common/MaskPanel", true, 1);
            UIDataTable.AddPanelData(EngineUI.ColorFadeTransition, null, "Common/ColorFadeTransition", true, 1);
            UIDataTable.AddPanelData(SDKUI.AdDisplayer, null, "Common/AdDisplayer", false, 1);
            UIDataTable.AddPanelData(SDKUI.OfficialVersionAdPanel, null, "OfficialVersionAdPanel");

            //在开发阶段使用该模式方便调试
            UIDataTable.SetABMode(false);
            UIDataTable.AddPanelData(UIID.WaitForStartPanel, null, "GamePanel/WaitForStartPanel");
            UIDataTable.AddPanelData(UIID.GameOverPanel, null, "GamePanel/GameOverPanel");
            UIDataTable.AddPanelData(UIID.MainGamePanel, null, "GamePanel/MainGamePanel");
        }
    }
}
