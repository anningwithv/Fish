using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Qarth;


//namespace GameWish.Game
//{
//    public partial class GameStateMgr
//    {
//        public enum GameplayStateID
//        {
//            NONE,
//            LevelInit,
//            LevelStart,
//            LevelPlaying,
//            LevelDying,//死亡中，可能会复活
//            LevelRevive,//复活状态
//            LevelOver,
//            LevelClean,//清理战场
//        }

//        public class GameplayState : FSMState<GameplayMgr>
//        {
//            public GameplayStateID stateID
//            {
//                get;
//                set;
//            }

//            public override void Enter(GameplayMgr mgr)
//            {
//            }

//            public override void Exit(GameplayMgr mgr)
//            {
//            }

//            public override void Execute(GameplayMgr mgr, float dt)
//            {
//            }
//        }

//        public class GameplayState_LevelInit : GameplayState
//        {
//            public override void Enter(GameplayMgr mgr)
//            {
//                LevelStateMgr.S.InitLevel();
//                //UIMgr.S.OpenPanel(UIID.HomePanel);
//                EventSystem.S.Send(EventID.OnLevelInit);
//            }
//        }

//        public class GameplayState_LevelStart : GameplayState
//        {
//            public override void Enter(GameplayMgr mgr)
//            {
//                LevelStateMgr.S.StartLevel();
                
//                EventSystem.S.Send(EventID.OnLevelStart);

//                mgr.stateMachine.SetCurrentStateByID(GameplayStateID.LevelPlaying);

//                //AdsMgr.S.HideBannerAd(Define.AD_MAIN_BANNER);
//            }
//        }

//        public class GameplayState_LevelPlaying : GameplayState
//        {
//            public override void Enter(GameplayMgr mgr)
//            {
//                LevelStateMgr.S.PlayingLevel();
//            }

//            public override void Execute(GameplayMgr mgr, float dt)
//            {

//            }
//        }

//        public class GameplayState_LevelDying : GameplayState
//        {
//            public override void Enter(GameplayMgr mgr)
//            {
//                //AdsMgr.S.ShowBannerAd(Define.AD_MAIN_BANNER, AdSize.SmartBanner, AdPosition.Bottom);
//                LevelStateMgr.S.DyingLevel();
//                if (LevelStateMgr.S.score >= 5)
//                {
//                    //UIMgr.S.OpenPanel(UIID.DyingPanel);
//                }
//                else
//                {
//                    //UIMgr.S.OpenPanel(UIID.GameOverPanel);
//                    mgr.stateMachine.SetCurrentStateByID(GameplayStateID.LevelOver);
//                }
//                //TEST
//                //mgr.stateMachine.SetCurrentStateByID(GameplayStateID.LevelOver);
//            }
//        }

//        public class GameplayState_LevelRevive : GameplayState
//        {
//            public override void Enter(GameplayMgr mgr)
//            {
//                //AdsMgr.S.HideBannerAd(Define.AD_MAIN_BANNER);
//                LevelStateMgr.S.ReceiveLevel();

//                //mgr.stateMachine.SetCurrentStateByID(GameplayStateID.LevelPlaying);

//                EventSystem.S.Send(EventID.OnLevelRevive);
//            }
//        }

//        public class GameplayState_LevelOver : GameplayState
//        {
//            public override void Enter(GameplayMgr mgr)
//            {
//                //AdsMgr.S.ShowBannerAd(Define.AD_MAIN_BANNER, AdSize.SmartBanner, AdPosition.Bottom);
//                LevelStateMgr.S.OverLevel();
//                EventSystem.S.Send(EventID.OnLevelOver);
//                //UIMgr.S.OpenPanel(UIID.GameOverPanel);
//                //mgr.stateMachine.SetCurrentStateByID(GameplayStateID.LevelClean);
//            }
//        }

//        public class GameplayState_LevelClean : GameplayState
//        {
//            //private float m_DelayTime = 0;

//            public override void Enter(GameplayMgr mgr)
//            {
//                LevelStateMgr.S.CleanLevel();

//                EventSystem.S.Send(EventID.OnLevelClean);
//               // mgr.stateMachine.SetCurrentStateByID(GameplayStateID.LevelInit);
//            }

//        }
//    }
//}
