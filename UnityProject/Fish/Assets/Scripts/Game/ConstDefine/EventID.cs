using UnityEngine;
using System.Collections;
using Qarth;

namespace GameWish.Game
{
    public enum EventID
    {
        OnLanguageTableSwitchFinish,

        OnPlayerSpawned, //当玩家出生时发送

        OnPlayerEnergyChanged,
        OnGetEnergy,
        //// Data related msg
        OnAddGoldCount, //当金币数发生改变时发送这个消息, long goldCount
        OnAddDiamondCount, //当钻石数发生改变时发送这个消息, int diamondCount
     
        OnWatchingAD, //看广告开始和结束时发送， bool isWatching

        //OnBoostTimeZero,
        //OnBuyIncomeTimeUpdate
    }

}
