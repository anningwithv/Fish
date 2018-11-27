using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;
namespace GameWish.Game
{
    public class SpriteLoader : TMonoSingleton<SpriteLoader> 
    {
        private ResLoader m_UILoader;

        public override void OnSingletonInit()
        {
        
        }
      
        public Sprite GetSpriteByName(string _strIconName)
        {
            if (m_UILoader == null)
            {
                m_UILoader = ResLoader.Allocate("UI_UpgradePreviewLoader");
                Debug.Log("loaded init upgrade panel");
            }
            UnityEngine.Object obj = m_UILoader.LoadSync(_strIconName);
            Texture2D text = obj as Texture2D;
            Sprite sprite = Sprite.Create(text, new Rect(0, 0, text.width, text.height), new Vector2(0.5f, 0.5f));
            return sprite;
        }

        private void OnDestroy()
        {
            if (m_UILoader != null)
            {
                m_UILoader.ReleaseAllRes();
                m_UILoader.Recycle2Cache();
                m_UILoader = null;
            }
        }

        
    }
}
