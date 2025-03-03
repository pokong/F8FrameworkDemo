using System.Collections;
using System.Collections.Generic;
using F8Framework.Core;
using HotUpdate;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameState : ProcedureNode
{
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
        SceneManager.LoadSceneAsync("MainScenes", LoadSceneMode.Single);
        FF8.UI.Open(InitState.UIID.UIVideoPlay);
    }


    public override void OnExit(ProcedureProcessor processor)
    {
            
    }
    
    public override void OnUpdate(ProcedureProcessor processor)
    {
            
    }
        
    public override void OnDestroy(ProcedureProcessor processor)
    {
            
    }
}