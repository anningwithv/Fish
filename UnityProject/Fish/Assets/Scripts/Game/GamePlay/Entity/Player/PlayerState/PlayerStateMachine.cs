using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public class PlayerStateMachine : FSMStateMachine<PlayerController>
    {
        public PlayerStateMachine(PlayerController mgr) : base(mgr)
        {
            InitStateFactory();
        }

        public PlayerState currentGameplayState
        {
            get
            {
                return base.currentState as PlayerState;
            }
        }

        public PlayerStateID currentStateID
        {
            get
            {
                var state = currentGameplayState;
                if (state == null)
                {
                    return PlayerStateID.None;
                }
                return state.stateID;
            }
        }

        private void InitStateFactory()
        {
            stateFactory = new PlayerStateFactory(false);
        }

        public bool IsJumping()
        {
            return currentGameplayState.stateID == PlayerStateID.Jump;
        }

        public bool IsFalling()
        {
            return currentGameplayState.stateID == PlayerStateID.Fall;
        }

        public bool IsMoving()
        {
            return currentGameplayState.stateID == PlayerStateID.Move;
        }
    }
}
