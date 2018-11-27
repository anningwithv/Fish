using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Qarth;

namespace GameWish.Game
{
    public class MusicMgr : TSingleton<MusicMgr>
    {
        private bool m_IsBgMusicPlaying = false;

        private int m_PlayWaveSoundInterval = 10;
        private int m_PlaySeagullSoundInterval = 12;
        private int m_PlaySeagullSoundRandomTime = 0;
        private int m_PlaySeagullSoundRandomRange = 8;

        private int m_WaveSoundCount = 0;
        private int m_SeagullSoundCount = 0;

        public void StartPlayWaveSound()
        {
            Timer.S.Post2Really(OnTimeReach, 1, -1);
        }

        private void OnTimeReach(int count)
        {
            if (m_IsBgMusicPlaying == false)
                return;

            //Play wave sound
            m_WaveSoundCount++;
            if (m_WaveSoundCount > m_PlayWaveSoundInterval)
            {
                m_WaveSoundCount = 0;
                m_PlayWaveSoundInterval = UnityEngine.Random.Range(8, 15);

                PlayWaveSound();
            }

            //Play seagull sound
            m_SeagullSoundCount++;
            if (m_SeagullSoundCount > m_PlaySeagullSoundInterval + m_PlaySeagullSoundRandomTime)
            {
                m_SeagullSoundCount = 0;
                m_PlaySeagullSoundRandomTime = UnityEngine.Random.Range(-1 * m_PlaySeagullSoundRandomRange, m_PlaySeagullSoundRandomRange);

                PlaySeagullSound();
            }
        }

        private void PlayWaveSound()
        {
            AudioMgr.S.PlaySound(TDConstTable.QueryString(ConstType.SOUND_SEAATMOSPHERE));
        }

        private void PlaySeagullSound()
        {
            int playTime = UnityEngine.Random.Range(2, 4);

            GameplayMgr.S.GetComponent<MonoBehaviour>().StartCoroutine(PlaySeagullSoundCor(playTime));
        }

        private IEnumerator PlaySeagullSoundCor(int time)
        {
            for (int i = 0; i < time; i++)
            {
                float delay = UnityEngine.Random.Range(0.1f, 0.5f);
                int randomIndex = UnityEngine.Random.Range(0, 2);
                if (randomIndex == 0)
                {
                    AudioMgr.S.PlaySound(TDConstTable.QueryString(ConstType.SOUND_SEAGULL1));
                }
                else
                {
                    AudioMgr.S.PlaySound(TDConstTable.QueryString(ConstType.SOUND_SEAGULL2));
                }
                yield return new WaitForSeconds(delay);
            }
        }

        public void PlayBgMusic()
        {
            m_IsBgMusicPlaying = true;
            AudioUnitID.MUSIC_BGID = AudioMgr.S.PlayBg(TDConstTable.QueryString(ConstType.MUSIC_BG));
            AudioMgr.S.SetVolume(AudioUnitID.MUSIC_BGID, 0.8f);
        }

        public void StopBgMusic()
        {
            m_IsBgMusicPlaying = false;
            AudioMgr.S.Stop(AudioUnitID.MUSIC_BGID);
        }

        public void PauseBgMusic()
        {
            m_IsBgMusicPlaying = false;
            AudioMgr.S.Pause(AudioUnitID.MUSIC_BGID);
        }

        public void ResumeBgMusic()
        {
            m_IsBgMusicPlaying = true;
            AudioMgr.S.Resume(AudioUnitID.MUSIC_BGID);
        }
    }
}
