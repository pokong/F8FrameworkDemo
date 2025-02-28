using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using F8Framework.Core;
using HotUpdate;
using RenderHeads.Media.AVProVideo;
using RenderHeads.Media.AVProVideo.Demos;

public class UIVideoPlay : BaseView
{
    public MediaPlayerUI _MediaPlayerUICOm;
    public MediaPlayer _MediaPlayer;

    public int _VidoId = -1;
    public int _VidoIdLast = -1;

    // Awake
    protected override void OnAwake()
    {
        Debug.Log("UIVideoPlay OnAwake");
        _MediaPlayerUICOm = go_MediaPlayerUI_go.GetComponent<MediaPlayerUI>();
        _MediaPlayer = _MediaPlayerUICOm.GetmediaPlayer();

        _MediaPlayer.Events.AddListener(VideoEventAction);
    }

    // 参数传入，每次打开UI都会执行
    protected override void OnAdded(int uiId, object[] args = null)
    {
        Debug.Log("UIVideoPlay OnAdded");
        // u_DataVideoid.AddValueChangeAction(DataShowviewAction);
        int index = int.Parse(args[0].ToString());
        ShowPlayeVideo(index);
    }

    // Start
    protected override void OnStart()
    {
        Debug.Log("UIVideoPlay OnStart");
        
        AddEventListener(HotMessageEvent.VideoPlayId, OnPlayerEvent);
    }

    protected override void OnViewTweenInit()
    {
        Debug.Log("UIVideoPlay OnViewTweenInit");
        //transform.localScale = Vector3.one * 0.7f;
    }

    // 自定义打开界面动画
    protected override void OnPlayViewTween()
    {
        Debug.Log("UIVideoPlay OnPlayViewTween");
        //transform.ScaleTween(Vector3.one, 0.1f).SetEase(Ease.Linear).SetOnComplete(OnViewOpen);
    }

    // 打开界面动画完成后
    protected override void OnViewOpen()
    {
        Debug.Log("UIVideoPlay OnViewOpen");
    }

    // 删除之前，每次UI关闭前调用
    protected override void OnBeforeRemove()
    {
        RemoveEventListener(HotMessageEvent.VideoPlayId, OnPlayerEvent);
        Debug.Log("UIVideoPlay OnBeforeRemove");
    }

    // 删除，UI关闭后调用
    protected override void OnRemoved()
    {
        Debug.Log("UIVideoPlay OnRemoved");
    }


    private void VideoEventAction(MediaPlayer mp, MediaPlayerEvent.EventType et, ErrorCode errorCode)
    {
        if (mp != _MediaPlayer) return;

        switch (et)
        {
            case MediaPlayerEvent.EventType.MetaDataReady:
                Debug.Log("视频数据准备完成。当元数据（宽度，持续时间等）可用时触发");
                break;
            case MediaPlayerEvent.EventType.ReadyToPlay:
                Debug.Log("加载视频并准备播放时触发");
                break;
            case MediaPlayerEvent.EventType.Started:
                Debug.Log("播放开始时触发");
                break;
            case MediaPlayerEvent.EventType.FirstFrameReady:
                Debug.Log("渲染第一帧时触发");
                break;
            case MediaPlayerEvent.EventType.FinishedPlaying:
                Debug.Log("当非循环视频播放完毕时触发");
                Debug.Log(_VidoIdLast + " = videoid = " + _VidoId);
                switch (_VidoId)
                {
                    // case 1 :
                    //     m_PanelMgr.OpenPanel<TestPanel,ETestPanelViewEnum>(ETestPanelViewEnum.YIUI2View);
                    //     break;
                    // case 2 :
                    //     m_PanelMgr.OpenPanel<TestPanel,ETestPanelViewEnum>(ETestPanelViewEnum.YIUI3View);
                    //     break;
                    // case 3 :
                    //     m_PanelMgr.OpenPanel<TestPanel,ETestPanelViewEnum>(ETestPanelViewEnum.YIUI4View);
                    //     break;
                    default:
                        //打开END UI
                        if (FF8.UI.Has(UIConfigData.UIID.UIVideoPlayEnd))
                        {
                            FF8.Message.DispatchEvent(HotMessageEvent.VideoPlayEnd);
                        }
                        else
                        {
                            FF8.UI.Open(UIConfigData.UIID.UIVideoPlayEnd);
                        }
                        break;
                }

                break;
            case MediaPlayerEvent.EventType.Closing:
                Debug.Log("媒体关闭时触发");
                break;
            case MediaPlayerEvent.EventType.Error:
                Debug.Log("发生错误时触发");
                break;
            case MediaPlayerEvent.EventType.SubtitleChange:
                Debug.Log("字幕更改时触发");
                break;
            case MediaPlayerEvent.EventType.Stalled:
                Debug.Log("媒体停顿/暂停？时触发（例如，当媒体流失去连接时）-当前仅在Windows平台上受支持");
                break;
            case MediaPlayerEvent.EventType.Unstalled:
                Debug.Log("当介质从停止状态恢复时触发（例如，重新建立丢失的连接时）");
                break;
            case MediaPlayerEvent.EventType.ResolutionChanged:
                Debug.Log("当视频的分辨率改变（包括负载）时触发");
                break;
            case MediaPlayerEvent.EventType.StartedSeeking:
                Debug.Log("寻找开始时触发");
                break;
            case MediaPlayerEvent.EventType.FinishedSeeking:
                Debug.Log("搜索完成时触发");
                break;
            case MediaPlayerEvent.EventType.StartedBuffering:
                Debug.Log("缓冲开始时触发");
                break;
            case MediaPlayerEvent.EventType.FinishedBuffering:
                Debug.Log("缓冲完成后触发");
                break;
            case MediaPlayerEvent.EventType.PropertiesChanged:
                Debug.Log("当任何属性（例如，立体声包装改变）时触发-必须手动触发");
                break;
            case MediaPlayerEvent.EventType.PlaylistItemChanged:
                Debug.Log("在播放列表中播放新项目时触发");
                break;
            case MediaPlayerEvent.EventType.PlaylistFinished:
                Debug.Log("播放列表结束时触发");
                break;
        }
    }

    private void OnPlayerEvent(params object[] obj)
    {
        LogF8.Log("OnPlayerEvent");
        ShowPlayeVideo(int.Parse(obj[0].ToString()));
    }

    public async void ShowPlayeVideo(int videoindex)
    {
        //解密后的临时视频路径
        var temppath = "/videotemp/";
        var tempname = "videotemp.mp4";
        var tempnamesrt = "videotempsub.srt";
        var tempUrl = Application.persistentDataPath + temppath;
        // tempUrl = Application.streamingAssetsPath + temppath;
        
        //临时目录空的话就创建
        if (Directory.Exists(tempUrl))
        {
            if(_VidoId != videoindex)
            {
                //删除临时目录下的所有文件
                foreach (var filePath in Directory.GetFiles(tempUrl))
                    File.Delete(filePath);
            }
        }
        else
        {
            //创建目录
            Directory.CreateDirectory(tempUrl);
        }
        
        _VidoIdLast = _VidoId;
        _VidoId = videoindex;

        //SUB
        if (!File.Exists(tempUrl + tempnamesrt))
        {
            AssetManager.AssetInfo infosub = FF8.Asset.GetAssetInfo(_VidoId + "sub");

            if (infosub.AssetBundlePath == null || infosub.AssetBundlePath == String.Empty)
            {
                _MediaPlayer.DisableSubtitles();
            }
            else
            {
                BaseLoader loadsub = FF8.Asset.LoadAsync<TextAsset>(_VidoId + "sub");
                await loadsub;

                //将文件转换成字节数组 取相反的二进制
                byte[] buffersrt_ed = TurnByte(loadsub.GetAssetObject<TextAsset>().bytes);
                //将字节数组写入到临时目录下
                File.WriteAllBytes(tempUrl + tempnamesrt, buffersrt_ed);
            }
        }
        var videosrtath = new MediaPath(tempUrl + tempnamesrt, MediaPathType.RelativeToDataFolder);
        _MediaPlayer.SideloadSubtitles = true;
        _MediaPlayer.EnableSubtitles(videosrtath);
        

        //VIDEO
        //如果加密路径下没有对应名字的加密文件 或者 已经临时目录下已有解密的视频文件 则不执行
        if (!File.Exists(tempUrl + tempname))
        {
            AssetManager.AssetInfo infovideo = FF8.Asset.GetAssetInfo(_VidoId + "video");
            if (infovideo.AssetBundlePath != String.Empty)
            {
                BaseLoader loadvideo = FF8.Asset.LoadAsync<TextAsset>(_VidoId + "video");
                await loadvideo;

                //将文件转换成字节数组
                //取相反的二进制
                var bufferEd = TurnByte(loadvideo.GetAssetObject<TextAsset>().bytes);
                //将字节数组写入到临时目录下
                File.WriteAllBytes(tempUrl + tempname, bufferEd);
            }
        }
        //给VideoPlayer组件赋值并播放
        var videoath = new MediaPath(tempUrl + tempname, MediaPathType.RelativeToDataFolder);
        _MediaPlayer.OpenMedia(videoath, autoPlay: false);
        _MediaPlayer.Play();
    }

    public static byte[] TurnByte(byte[] input)
    {
        for (var i = 0; i < input.Length; i++)
        {
            var currentByte = input[i];

            var shiftedValue = (byte)~currentByte;
            // 将移位后的字节添加到加密byte数组中
            input[i] = shiftedValue;
        }

        return input;
    }


    // 自动获取组件（自动生成，不能删除）
    [SerializeField] private GameObject go_MediaPlayer_go;
    [SerializeField] private GameObject go_MediaPlayerUI_go;
    [SerializeField] private TMPro.TMP_Text tmpSub_tmp;
    [SerializeField] private Button ButtonPlayPause_Button;
    [SerializeField] private Button ButtonNavBack_Button;
    [SerializeField] private Button ButtonNavForward_Button;
    [SerializeField] private Button ButtonVolume_Button;
    [SerializeField] private Button ButtonSubtitles_Button;
    [SerializeField] private Button ButtonOptions_Button;
    [SerializeField] private Slider SliderVolume_Slider;
    [SerializeField] private Image ImageLive_Image;
    [SerializeField] private Button Button2_Button;
    [SerializeField] private Button Button3_Button;
    [SerializeField] private Button Button4_Button;
    [SerializeField] private Button Button5_Button;

#if UNITY_EDITOR
    protected override void SetComponents()
    {
        go_MediaPlayer_go = transform.Find("go_MediaPlayer").gameObject;
        go_MediaPlayerUI_go = transform.Find("go_MediaPlayerUI").gameObject;
        tmpSub_tmp = transform.Find("go_MediaPlayerUI/Subtitles/tmpSub").GetComponent<TMPro.TMP_Text>();
        ButtonPlayPause_Button =
            transform.Find("go_MediaPlayerUI/Controls/BottomRow/ButtonPlayPause").GetComponent<Button>();
        ButtonNavBack_Button =
            transform.Find("go_MediaPlayerUI/Controls/BottomRow/ButtonNavBack").GetComponent<Button>();
        ButtonNavForward_Button = transform.Find("go_MediaPlayerUI/Controls/BottomRow/ButtonNavForward")
            .GetComponent<Button>();
        ButtonVolume_Button = transform.Find("go_MediaPlayerUI/Controls/BottomRow/ButtonVolume").GetComponent<Button>();
        ButtonSubtitles_Button =
            transform.Find("go_MediaPlayerUI/Controls/BottomRow/ButtonSubtitles").GetComponent<Button>();
        ButtonOptions_Button =
            transform.Find("go_MediaPlayerUI/Controls/BottomRow/ButtonOptions").GetComponent<Button>();
        SliderVolume_Slider = transform.Find("go_MediaPlayerUI/Controls/BottomRow/VolumeMask/SliderVolume")
            .GetComponent<Slider>();
        ImageLive_Image = transform.Find("go_MediaPlayerUI/Controls/BottomRow/TextLive/ImageLive")
            .GetComponent<Image>();
        Button2_Button = transform.Find("go_MediaPlayerUI/Controls/OptionsMenu/SubtitlesMenu/Button (2)")
            .GetComponent<Button>();
        Button3_Button = transform.Find("go_MediaPlayerUI/Controls/OptionsMenu/SubtitlesMenu/Button (3)")
            .GetComponent<Button>();
        Button4_Button = transform.Find("go_MediaPlayerUI/Controls/OptionsMenu/SubtitlesMenu/Button (4)")
            .GetComponent<Button>();
        Button5_Button = transform.Find("go_MediaPlayerUI/Controls/OptionsMenu/SubtitlesMenu/Button (5)")
            .GetComponent<Button>();
    }
#endif
    // 自动获取组件（自动生成，不能删除）
}