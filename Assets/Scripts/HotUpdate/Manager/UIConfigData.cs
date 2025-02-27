using System.Collections;
using System.Collections.Generic;
using F8Framework.Core;
using UnityEngine;


namespace HotUpdate
{
    public class UIConfigData
    {
        private static UIConfigData _instance = null;

        public enum UIID
        {
            // UI枚举
            Empty = 0,
            UISelectRole = 1, // 选择角色
            UIGameView = 2, // 游戏界面
            UIAward = 3, // 奖励
            UITip = 4, // 提示
        }

        public static Dictionary<UIID, UIConfig> ConfigsData = new Dictionary<UIID, UIConfig>
        {
            { UIID.UISelectRole, new UIConfig(LayerType.UI, "UISelectRole") },
            { UIID.UIGameView, new UIConfig(LayerType.UI, "UIGameView") },
            { UIID.UIAward, new UIConfig(LayerType.Dialog, "UIAward") },
            { UIID.UITip, new UIConfig(LayerType.Notify, "UITip") },
        };

        public static UIConfigData Instance()
        {
            if (_instance == null)
            {
                _instance = new UIConfigData();
                _instance.init();
            }

            return _instance;
        }


        public void init()
        {
        }
    }
}