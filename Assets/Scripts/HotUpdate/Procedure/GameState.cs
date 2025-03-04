using System.Collections;
using System.Collections.Generic;
using F8Framework.Core;
using HotUpdate;
// using Unity.Plastic.Newtonsoft.Json.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameState : ProcedureNode
{
    private AsyncOperation loadSceneAsync = null;
    private bool isloadScenes = false;

    public override void OnInit(ProcedureProcessor processor)
    {
    }

    public override void OnEnter(ProcedureProcessor processor)
    {
        OnEnterScenes();
    }

    private async void OnEnterScenes()
    {
        BaseLoader _load = AssetManager.Instance.LoadAsync("MainScenes");
        await _load;
        isloadScenes = true;
        loadSceneAsync = SceneManager.LoadSceneAsync("MainScenes", LoadSceneMode.Single);
        // SceneManager.LoadScene("MainScenes", LoadSceneMode.Single);
    }


    public override void OnExit(ProcedureProcessor processor)
    {
    }

    public override void OnUpdate(ProcedureProcessor processor)
    {
        if (isloadScenes && loadSceneAsync != null)
        {
            // Debug.Log("loadSceneAsync progress=" + loadSceneAsync.progress);
            if (loadSceneAsync.isDone)
            {
                isloadScenes = false;
                FF8.UI.Open(InitState.UIID.UIVideoPlay, new object[] { 1 });
            }
        }
    }

    public override void OnDestroy(ProcedureProcessor processor)
    {
    }
}