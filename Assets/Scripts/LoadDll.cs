using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using F8Framework.Core;
using HybridCLR;
using UnityEngine;

public class LoadDll : MonoBehaviour
{
    private HotUpdateManager HotUpdateManager;
    private DownloadManager DownloadManager;
    private AssetManager AssetManager;

    public void Awake()
    {
        gameObject.DontDestroy();
    }

    public IEnumerator Start()
    {
        ModuleCenter.Initialize(this);
        // 在这里可以启动热更新
        HotUpdateManager = ModuleCenter.CreateModule<HotUpdateManager>();
        DownloadManager = ModuleCenter.CreateModule<DownloadManager>();
        AssetManager = ModuleCenter.CreateModule<AssetManager>();

        // 初始化本地版本
        HotUpdateManager.InitLocalVersion();

        // 初始化远程版本
        yield return HotUpdateManager.InitRemoteVersion();

        // 初始化资源版本
        yield return HotUpdateManager.InitAssetVersion();
        
        // 检查未加载的分包
        List<string> subPackage = HotUpdateManager.CheckPackageUpdate(GameConfig.LocalGameVersion.SubPackage);

        // 分包加载
        HotUpdateManager.StartPackageUpdate(subPackage, () =>
            {
                LogF8.Log("分包完成");
                startHotUpdate();
            }, () =>
            {
                LogF8.Log("分包失败"); 
                
            },
            progress =>
            {
                // LogF8.Log("分包进度：" + progress); 
                
            });
    }
    
    public void startHotUpdate()
    {
        // 检查需要热更的资源，总大小
        Tuple<Dictionary<string, string>, long> result = HotUpdateManager.CheckHotUpdate();
        var hotUpdateAssetUrl = result.Item1;
        var allSize = result.Item2;
        LogF8.Log("热更总大小：" + allSize.ToString());

        // 资源热更新
        HotUpdateManager.StartHotUpdate(hotUpdateAssetUrl, () =>
        {
            LogF8.Log("完成");
            
            // Editor环境下，HotUpdate.dll.bytes已经被自动加载，不需要加载，重复加载反而会出问题。
#if !UNITY_EDITOR
            
            // 先补充元数据
            LoadMetadataForAOTAssemblies();
            
            TextAsset asset1 = AssetManager.Instance.Load<TextAsset>("F8Framework.F8ExcelDataClass");
            Assembly hotUpdateAss1 = Assembly.Load(asset1.bytes);
            TextAsset asset2 = AssetManager.Instance.Load<TextAsset>("HotUpdate");
            Assembly hotUpdateAss2 = Assembly.Load(asset2.bytes);
#else
            // Editor下无需加载，直接查找获得HotUpdate程序集
            Assembly hotUpdateAss1 = System.AppDomain.CurrentDomain.GetAssemblies()
                .First(a => a.GetName().Name == "F8Framework.F8ExcelDataClass");
            Assembly hotUpdateAss2 = System.AppDomain.CurrentDomain.GetAssemblies()
                .First(a => a.GetName().Name == "HotUpdate");
#endif
            Type type = hotUpdateAss2.GetType("HotUpdate.GameLauncher");
            
            // 添加组件
            gameObject.AddComponent(type);
            
        }, () =>
        {
            LogF8.Log("失败");
 
        }, progress =>
        {
            // LogF8.Log("进度：" + progress); 

        });
        
    }
    
    private static void LoadMetadataForAOTAssemblies()
    {
        List<string> aotDllList = new List<string>
        {
            "mscorlib.dll",
            "System.dll",
            "System.Core.dll", // 如果使用了Linq，需要这个
            // "Newtonsoft.Json.dll", 
            // "protobuf-net.dll",
        };

        foreach (var aotDllName in aotDllList)
        {
            TextAsset asset = AssetManager.Instance.Load<TextAsset>(aotDllName);
            LoadImageErrorCode err = HybridCLR.RuntimeApi.LoadMetadataForAOTAssembly(asset.bytes, HomologousImageMode.SuperSet);
            Debug.Log($"LoadMetadataForAOTAssembly:{aotDllName}. ret:{err}");
        }
    }
}