using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameWish.Game
{
    public class ItemBase : EntityController
    {
        protected override void Init()
        {
        }

        protected override void SetInterestEvent()
        {
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            
        }
    }
}