using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;

namespace GameWish.Game
{
    public class GameData : IDataClass
    {
        static public string LastPlayTimeString
        {
            get
            {
                return PlayerPrefs.GetString(Define.LAST_PLAY_TIME, "");
            }
            set
            {
                if (!string.IsNullOrEmpty(PlayerPrefs.GetString(Define.LAST_PLAY_TIME, "")))
                {
                    long token = 0;
                    long.TryParse(PlayerPrefs.GetString(Define.LAST_PLAY_TIME), out token);
                    long newToken = 0;
                    long.TryParse(value, out newToken);
                    if (newToken > token)
                    {
                        PlayerPrefs.SetString(Define.LAST_PLAY_TIME, value);
                    }

                }
                else
                    PlayerPrefs.SetString(Define.LAST_PLAY_TIME, value);
            }
        }

        public GameData()
        {
            SetDirtyRecorder(GameDataHandler.s_DataDirtyRecorder);
        }

        public override void InitWithEmptyData()
        {
        }

        public override void OnDataLoadFinish()
        {
        }
    }
}
