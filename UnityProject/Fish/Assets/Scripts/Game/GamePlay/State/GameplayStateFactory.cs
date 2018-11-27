using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Qarth;

//namespace GameWish.Game
//{
//    public class GameplayStateFactory : FSMStateFactory<GameplayMgr>
//    {
//        public GameplayStateFactory(bool alwaysCreate) : base(alwaysCreate)
//        {
//            InitStateCreator();
//        }

//        private void InitStateCreator()
//        {
//            RegisterGameplayState(GameplayMgr.GameplayStateID.LevelInit, new GameplayMgr.GameplayState_LevelInit());
//            RegisterGameplayState(GameplayMgr.GameplayStateID.LevelStart, new GameplayMgr.GameplayState_LevelStart());
//            RegisterGameplayState(GameplayMgr.GameplayStateID.LevelPlaying, new GameplayMgr.GameplayState_LevelPlaying());
//            RegisterGameplayState(GameplayMgr.GameplayStateID.LevelDying, new GameplayMgr.GameplayState_LevelDying());
//            RegisterGameplayState(GameplayMgr.GameplayStateID.LevelRevive, new GameplayMgr.GameplayState_LevelRevive());
//            RegisterGameplayState(GameplayMgr.GameplayStateID.LevelOver, new GameplayMgr.GameplayState_LevelOver());
//            RegisterGameplayState(GameplayMgr.GameplayStateID.LevelClean, new GameplayMgr.GameplayState_LevelClean());
//        }

//        private void RegisterGameplayState(GameplayMgr.GameplayStateID id, GameplayMgr.GameplayState state)
//        {
//            state.stateID = id;
//            RegisterState(id, state);
//        }
//    }
//}
