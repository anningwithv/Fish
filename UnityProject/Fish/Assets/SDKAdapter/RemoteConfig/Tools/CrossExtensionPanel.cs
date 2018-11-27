using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qarth;
using UnityEngine.UI;

namespace Qarth
{
    public class CrossExtensionPanel : AbstractPanel
    {
        [SerializeField]
        private IUListView m_ListView;

        private List<TDAppConfigTable.ConfigHandler> m_HandlerList;

        protected override void OnUIInit()
        {
            m_ListView.SetCellRenderer(OnCellRenderer);
        }

        protected override void OnPanelOpen(params object[] args)
        {
            m_HandlerList = TDAppConfigTable.GetConfigHandlerList();
            m_ListView.SetDataCount(m_HandlerList.Count);
        }

        protected void OnCellRenderer(Transform root, int index)
        {
            CrossExtensionView view = root.GetComponent<CrossExtensionView>();
            if (view == null)
            {
                return;
            }

            view.SetIdentifyer(m_HandlerList[index]);
        }
    }
}
