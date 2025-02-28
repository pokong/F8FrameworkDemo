using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using F8Framework.Core;
using HotUpdate;

public class UIVideoPlayEnd : BaseView
{
    // Awake
    protected override void OnAwake()
    {
        Button1_Button.onClick.AddListener(()=>
        {
            ButtonClickedEvent(1);
        });
        Button2_Button.onClick.AddListener(()=>
        {
            ButtonClickedEvent(2);
        });
        Button3_Button.onClick.AddListener(()=>
        {
            ButtonClickedEvent(3);
        });
        Button4_Button.onClick.AddListener(()=>
        {
            ButtonClickedEvent(4);
        });
    }

    private void ButtonClickedEvent(int index)
    {
        // FF8.UI.Open(UIConfigData.UIID.UIVideoPlay, new object[] { index });
        FF8.Message.DispatchEvent(HotMessageEvent.VideoPlayId, new object[] { index });
        Close();
    }

    // 参数传入，每次打开UI都会执行
    protected override void OnAdded(int uiId, object[] args = null)
    {
        
    }
    
    // Start
    protected override void OnStart()
    {
        TextTMP_TextTMP_5.text = FF8.Config.GetroleByID(1).name;
    }
    
    protected override void OnViewTweenInit()
    {
        //transform.localScale = Vector3.one * 0.7f;
    }
    
    // 自定义打开界面动画
    protected override void OnPlayViewTween()
    {
        //transform.ScaleTween(Vector3.one, 0.1f).SetEase(Ease.Linear).SetOnComplete(OnViewOpen);
    }
    
    // 打开界面动画完成后
    protected override void OnViewOpen()
    {
        
    }
    
    // 删除之前，每次UI关闭前调用
    protected override void OnBeforeRemove()
    {
        
    }
    
    // 删除，UI关闭后调用
    protected override void OnRemoved()
    {
        
    }
    
    // 自动获取组件（自动生成，不能删除）
    [SerializeField] private Button Button1_Button;
    [SerializeField] private Button Button2_Button;
    [SerializeField] private Button Button3_Button;
    [SerializeField] private Button Button4_Button;
    [SerializeField] private Button Button5_Button;
    [SerializeField] private TMPro.TMP_Text TextTMP_TextTMP;
    [SerializeField] private TMPro.TMP_Text TextTMP_TextTMP_2;
    [SerializeField] private TMPro.TMP_Text TextTMP_TextTMP_3;
    [SerializeField] private TMPro.TMP_Text TextTMP_TextTMP_4;
    [SerializeField] private TMPro.TMP_Text TextTMP_TextTMP_5;

#if UNITY_EDITOR
    protected override void SetComponents()
    {
        Button1_Button = transform.Find("Button1").GetComponent<Button>();
        Button2_Button = transform.Find("Button2").GetComponent<Button>();
        Button3_Button = transform.Find("Button3").GetComponent<Button>();
        Button4_Button = transform.Find("Button4").GetComponent<Button>();
        Button5_Button = transform.Find("Button5").GetComponent<Button>();
        TextTMP_TextTMP = transform.Find("Button1/Text (TMP)").GetComponent<TMPro.TMP_Text>();
        TextTMP_TextTMP_2 = transform.Find("Button2/Text (TMP)").GetComponent<TMPro.TMP_Text>();
        TextTMP_TextTMP_3 = transform.Find("Button3/Text (TMP)").GetComponent<TMPro.TMP_Text>();
        TextTMP_TextTMP_4 = transform.Find("Button4/Text (TMP)").GetComponent<TMPro.TMP_Text>();
        TextTMP_TextTMP_5 = transform.Find("Button5/Text (TMP)").GetComponent<TMPro.TMP_Text>();
    }
#endif
    // 自动获取组件（自动生成，不能删除）
}