using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Qarth;

public class UIAnimExtern
{

    public static Sequence PlayPingpong(Transform target,float delay, float duration, params float[] allScale)
    {
        Vector3 scale = target.localScale;
        var sequence = DOTween.Sequence();
        if (delay > 0)
        {
            sequence.AppendInterval(delay);
        }
        for (int i = 0; i < allScale.Length; i++)
        {
            sequence = sequence.Append(target.DOScale(scale*allScale[i], duration).SetEase(Ease.Linear));
        }
        return sequence;
    }
}


