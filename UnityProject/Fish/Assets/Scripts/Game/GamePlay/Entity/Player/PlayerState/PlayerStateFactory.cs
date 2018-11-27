using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public class PlayerStateFactory : FSMStateFactory<PlayerController>
    {
        public PlayerStateFactory(bool alwaysCreate) : base(alwaysCreate)
        {
            InitStateCreator();
        }

        private void InitStateCreator()
        {
            RegisterPlayerState(PlayerStateID.Idle, new PlayerStateIdle(PlayerStateID.Idle));
            RegisterPlayerState(PlayerStateID.Jump, new PlayerStateJump(PlayerStateID.Jump));
            RegisterPlayerState(PlayerStateID.Move, new PlayerStateMove(PlayerStateID.Move));
            RegisterPlayerState(PlayerStateID.Fall, new PlayerStateFall(PlayerStateID.Fall));
            RegisterPlayerState(PlayerStateID.Dead, new PlayerStateDead(PlayerStateID.Dead));
        }

        private void RegisterPlayerState(PlayerStateID id, PlayerState state)
        {
            state.stateID = id;
            RegisterState(id, state);
        }
    }
}
