using UnityEngine;
using System.Collections;
using Qarth;
using UnityEngine.SceneManagement;

namespace GameWish.Game
{
    public class InputModule : AbstractModule
    {
        private IInputter m_KeyboardInputter;
        private KeyCodeTracker m_KeyCodeTracker;

        public override void OnComLateUpdate(float dt)
        {
            m_KeyboardInputter.LateUpdate();
            m_KeyCodeTracker.LateUpdate();
        }

        protected override void OnComAwake()
        {
            m_KeyCodeTracker = new KeyCodeTracker();
            m_KeyCodeTracker.SetDefaultProcessListener(ShowBackKeydownTips);

            m_KeyboardInputter = new KeyboardInputter();
            m_KeyboardInputter.RegisterKeyCodeMonitor(KeyCode.F1, null, OnClickF1, null);
            m_KeyboardInputter.RegisterKeyCodeMonitor(KeyCode.F2, null, OnClickF2, null);
            m_KeyboardInputter.RegisterKeyCodeMonitor(KeyCode.F3, null, OnClickF3, null);
            m_KeyboardInputter.RegisterKeyCodeMonitor(KeyCode.F4, null, OnClickF4, null);
        }

        private void ShowBackKeydownTips()
        {
            FloatMessage.S.ShowMsg(TDLanguageTable.Get("Press Again to Quit"));
        }

        private void OnClickF1()
        {
            MeasureUnitHelper.AddCount(0,0,-50,-500);
            Log.i(MeasureUnitHelper.GetTotalCount());
        }
        
        private void OnClickF2()
        {

        }

        private void OnClickF3()
        {

        }

        private void OnClickF4()
        {
            KeyCodeEventInfo info = new KeyCodeEventInfo();
            EventSystem.S.Send(EngineEventID.BackKeyDown, info);
        }

        private void OnSceneLoadResult(string sceneName, bool result)
        {
            Log.i("SceneLoad:" + sceneName + " " + result);
        }
    }
}
