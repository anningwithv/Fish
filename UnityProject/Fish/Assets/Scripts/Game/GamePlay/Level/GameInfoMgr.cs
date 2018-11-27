using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;

public class GameInfoMgr : TSingleton<GameInfoMgr>
{
    private int m_DiedTime = 0;

    private int m_DeadCount;

    private int m_ScoreCount;
    private int m_StageScore;
    private int m_GoldCount;

    private int m_ScoreCountOffset = 1;

    private bool m_StopScoreCount;

    public int score
    {
        get { return m_ScoreCount; }
    }

    /// <summary>
    /// 用于结算时计数使用
    /// </summary>
    public int goldCount
    {
        get { return m_GoldCount; }
    }

    public void Init()
    {
        m_ScoreCount = 0;
        m_DeadCount = 0;
        m_StageScore = 0;
        m_GoldCount = 0;
        SwitchStageID(-1);

        m_DiedTime = 0;
    }

    private void OnScoreAdd(int key, params object[] param)
    {
        ++m_ScoreCount;
        ++m_GoldCount;
        m_ScoreCount += m_ScoreCountOffset;
        ++m_StageScore;
    }


    protected void SwitchStageID(int id = -1)
    {
        if (id > 1)
        {
            DataAnalysisMgr.S.CustomEvent(DataAnalysisDefine.CHALLENGE_LEVENT, id.ToString());
        }

        if (id < 0)
        {

        }

    }
}
