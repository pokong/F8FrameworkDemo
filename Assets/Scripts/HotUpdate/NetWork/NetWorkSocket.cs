using System;
using System.Collections;
using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityWebSocket;

public class NetWorkSocket
{
    // Start is called before the first frame update
    private string wsUrl = "ws://localhost:10100/websocket"; //io game
    private static NetWorkSocket _instance = null;
    public WebSocket socket;

    public static NetWorkSocket Instance()
    {
        if (_instance == null)
        {
            _instance = new NetWorkSocket();
            _instance.init();
        }

        return _instance;
    }

    /// <summary>
    /// 关闭链接
    /// </summary>
    public void Close()
    {
        socket.CloseAsync();
    }

    public void init()
    {
        if(socket == null)
        {
            socket = new WebSocket(wsUrl);
            // 注册回调
            socket.OnOpen += OnOpen;
            socket.OnClose += OnClose;
            socket.OnMessage += OnMessage;
            socket.OnError += OnError;
        }
        
        // HandleMgr.addHandler(1, 0, HandleMgr.Hello);
        HandleMgr.addHandler(3, 1, HandleMgr.Hello);
        
    }

    public void Connect()
    {
        socket.ConnectAsync();
    }

    public void OnOpen(object o, OpenEventArgs args)
    {
        var loginVerify = new ReqLoginVerify
        {
            Age = 273676,
            Jwt = "luoyi",
            LoginBizCode = 1
        };
        var myExternalMessage = new MyExternalMessage
        {
            CmdMerge = CmdMgr.getMergeCmd(3, 1),
            DataContent = loginVerify.ToByteString(),
            ProtocolSwitch = 0,
            CmdCode = 1
        };
        Debug.Log("发送消息："+CmdMgr.getMergeCmd(3, 1));
        socket.SendAsync(myExternalMessage.ToByteArray());
        Debug.Log("打开链接回调");
    }


    public void OnClose(object o, CloseEventArgs args)
    {
        Debug.Log("关闭链接");
    }

    public void OnMessage(object o, MessageEventArgs args)
    {
        //将字节数组转换为
        IMessage message = new MyExternalMessage();
        var mySelf = (MyExternalMessage)message.Descriptor.Parser.ParseFrom(args.RawData);
        
        Debug.Log("OnMessage getCmd="+CmdMgr.getCmd(mySelf.CmdMerge));
        
        Debug.Log("OnMessage：CmdCode=" + mySelf.CmdCode + " CmdMerge=" + mySelf.CmdMerge + " ResponseStatus=" +
                  mySelf.ResponseStatus + " ValidMsg=" + mySelf.ValidMsg);
        HandleMgr.packageHandler(mySelf.CmdMerge, mySelf.DataContent);
    }

    public void OnError(object o, ErrorEventArgs args)
    {
        Debug.Log("异常出现: " + args.Message);
    }
}