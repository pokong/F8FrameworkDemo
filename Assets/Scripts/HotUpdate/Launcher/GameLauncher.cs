using System.Collections;
using F8Framework.Core;
using F8Framework.F8ExcelDataClass;
using UnityEngine;
using UnityEngine.Audio;


namespace HotUpdate
{
    public class GameLauncher : MonoBehaviour
    {
        IEnumerator Start()
        {
            // 初始化模块中心
            ModuleCenter.Initialize(this);

            // 初始化版本
            FF8.HotUpdate = ModuleCenter.CreateModule<HotUpdateManager>();

            // 按顺序创建模块，可按需添加
            FF8.Message = ModuleCenter.CreateModule<MessageManager>();
            FF8.Input = ModuleCenter.CreateModule<InputManager>(new DefaultInputHelper());
            FF8.Storage = ModuleCenter.CreateModule<StorageManager>();
            FF8.Timer = ModuleCenter.CreateModule<TimerManager>();
            FF8.Procedure = ModuleCenter.CreateModule<ProcedureManager>();
            FF8.Network = ModuleCenter.CreateModule<NetworkManager>();
            FF8.FSM = ModuleCenter.CreateModule<FSMManager>();
            FF8.GameObjectPool = ModuleCenter.CreateModule<GameObjectPool>();
            FF8.Asset = ModuleCenter.CreateModule<AssetManager>();
#if UNITY_WEBGL
            yield return AssetBundleManager.Instance.LoadAssetBundleManifest(); // WebGL专用，如果游戏中没有使用任何AB包加载资源，可以删除此方法的调用！
#endif
            FF8.Config = ModuleCenter.CreateModule<F8DataManager>();
            FF8.Audio = ModuleCenter.CreateModule<AudioManager>();
            FF8.Tween = ModuleCenter.CreateModule<Tween>();
            FF8.UI = ModuleCenter.CreateModule<UIManager>();
#if UNITY_WEBGL
            yield return F8DataManager.Instance.LoadLocalizedStringsIEnumerator(); // WebGL专用
#endif
            FF8.Local = ModuleCenter.CreateModule<Localization>();
            FF8.SDK = ModuleCenter.CreateModule<SDKManager>();
            FF8.Download = ModuleCenter.CreateModule<DownloadManager>();
            FF8.LogWriter = ModuleCenter.CreateModule<F8LogWriter>();

            StartGame();
            yield break;
        }

        // 开始游戏
        public void StartGame()
        {
            FF8.Config.LoadAll();

#if UNITY_EDITOR
            ReadExcel.Instance.LoadAllExcelData();
#endif

            LogF8.Log(FF8.Config.GetroleByID(1).name);


            FF8.Asset.Load("IsometricSpriteAtlas");

            // 加载文件夹内资产
            FF8.Asset.LoadDirAsync("Role_Textures", () =>
            {
                // 初始化
                FF8.UI.Initialize(UIConfigData.ConfigsData);

                FF8.UI.Open(UIConfigData.UIID.UISelectRole);

                StartNet();
            });


            FF8.Audio.SetAudioMixer(FF8.Asset.Load<AudioMixer>("F8AudioMixer"));

            FF8.Audio.PlayMusic("02b Town Theme", null, true);
        }

        public void StartNet()
        {
            
            NetWorkSocket.Instance().Connect();
        }

        void Update()
        {
            // 更新模块
            ModuleCenter.Update();
        }

        void LateUpdate()
        {
            // 更新模块
            ModuleCenter.LateUpdate();
        }

        void FixedUpdate()
        {
            // 更新模块
            ModuleCenter.FixedUpdate();
        }
    }
}