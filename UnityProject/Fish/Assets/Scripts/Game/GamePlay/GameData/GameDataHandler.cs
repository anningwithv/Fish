using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameWish.Game
{
    public class GameDataHandler : DataClassHandler<GameData>
    {
        public static DataDirtyRecorder s_DataDirtyRecorder = new DataDirtyRecorder();

        public static string s_path { get { return dataFilePath; } }

        public GameDataHandler()
        {
            Load();
            EnableAutoSave();
        }

        public GameData GetGameData()
        {
            return m_Data;
        }

        public void Save()
        {
            Save(true);
        }


        //public static BigInteger s_PlayerCoins
        //{
        //    get { return m_Data.GetCoins(); }
        //}

        //public static bool AddCoins(BigInteger count)
        //{
        //    if (count < 0)
        //    {
        //        if (s_PlayerCoins + count < 0)//如果消耗数量大于拥有的数量
        //        {
        //            return false;
        //        }
        //        else
        //        {
        //            m_Data.AddGoldCount(count);
        //        }
        //    }
        //    else
        //    {
        //        m_Data.AddGoldCount(count);
        //        //AudioMgr.S.PlaySound(TDConstTable.QueryString(ConstType.SOUND_MONEY));
        //    }
        //    EventSystem.S.Send(EventID.OnCoinsAdd, count > 0);
        //    return true;
        //}
    }
}