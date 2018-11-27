using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameWish.Game
{
    public class ItemEnergy : ItemBase
    {
        protected override void Init()
        {
        }

        protected override void SetInterestEvent()
        {
        }

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == Define.PLAYER_TAG)
            {
                SendEvent(EventID.OnGetEnergy);
            }
        }
    }
}