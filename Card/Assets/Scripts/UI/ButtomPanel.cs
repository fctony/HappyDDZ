using Protocol.Code;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtomPanel : UIBase
{
    private void Awake()
    {
        Bind(UIEvent.CHANGE_MUTIPLE);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.CHANGE_MUTIPLE:
                changeMutiple((int)message);
                break;
            default:
                break;
        }
    }

    private Text txtBeen;
    private Text txtMutiple;
    private Button btnChat;
    private Image imgChoose;
    private Button[] btns;

    private SocketMsg socketMsg;

    private void Start()
    {
        initPanel();
        socketMsg = new SocketMsg();

        //默认
        imgChoose.gameObject.SetActive(false);

        refreshPanel(Models.GameModel.UserDto.Been);
    }

    private void initPanel()
    {
        txtBeen = transform.Find("txtBeen").GetComponent<Text>();
        txtMutiple = transform.Find("txtMutiple").GetComponent<Text>();
        btnChat = transform.Find("btnChat").GetComponent<Button>();
        btns = new Button[7];
        imgChoose = transform.Find("imgChoose").GetComponent<Image>();
        for (int i = 0; i < 7; i++)
        {
            btns[i] = imgChoose.transform.GetChild(i).GetComponent<Button>();
        }
        btns[0].onClick.AddListener(chatClick1);
        btns[1].onClick.AddListener(chatClick2);
        btns[2].onClick.AddListener(chatClick3);
        btns[3].onClick.AddListener(chatClick4);
        btns[4].onClick.AddListener(chatClick5);
        btns[5].onClick.AddListener(chatClick6);
        btns[6].onClick.AddListener(chatClick7);

        btnChat.onClick.AddListener(setChooseActive);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        btnChat.onClick.RemoveListener(setChooseActive);
        btns[0].onClick.RemoveListener(chatClick1);
        btns[1].onClick.RemoveListener(chatClick2);
        btns[2].onClick.RemoveListener(chatClick3);
        btns[3].onClick.RemoveListener(chatClick4);
        btns[4].onClick.RemoveListener(chatClick5);
        btns[5].onClick.RemoveListener(chatClick6);
        btns[6].onClick.RemoveListener(chatClick7);
    }

    /// <summary>
    /// 刷新自身面板的信息
    /// </summary>
    private void refreshPanel(int beenCount)
    {
        this.txtBeen.text = "× " + beenCount;
    }

    /// <summary>
    /// 改变牌局倍数
    /// </summary>
    /// <param name="muti"></param>
    private void changeMutiple(int mutiple)
    {
        txtMutiple.text = "倍数 × " + mutiple;
    }

    /// <summary>
    /// 设置选择对话内容的面板显示
    /// </summary>
    private void setChooseActive()
    {
        bool active = imgChoose.gameObject.activeInHierarchy;
        imgChoose.gameObject.SetActive(!active);
    }

    /// <summary>
    /// 点击某一句聊天内容时候调用
    /// </summary>
    /// <param name="chatType"></param>
    private void chatClick1()
    {
        socketMsg.Change(OpCode.CHAT, ChatCode.CREQ, 1);
        Dispatch(AreaCode.NET, 0, socketMsg);
    }
    private void chatClick2()
    {
        socketMsg.Change(OpCode.CHAT, ChatCode.CREQ, 2);
        Dispatch(AreaCode.NET, 0, socketMsg);
    }
    private void chatClick3()
    {
        socketMsg.Change(OpCode.CHAT, ChatCode.CREQ, 3);
        Dispatch(AreaCode.NET, 0, socketMsg);
    }
    private void chatClick4()
    {
        socketMsg.Change(OpCode.CHAT, ChatCode.CREQ, 4);
        Dispatch(AreaCode.NET, 0, socketMsg);
    }
    private void chatClick5()
    {
        socketMsg.Change(OpCode.CHAT, ChatCode.CREQ, 5);
        Dispatch(AreaCode.NET, 0, socketMsg);
    }
    private void chatClick6()
    {
        socketMsg.Change(OpCode.CHAT, ChatCode.CREQ, 6);
        Dispatch(AreaCode.NET, 0, socketMsg);
    }
    private void chatClick7()
    {
        socketMsg.Change(OpCode.CHAT, ChatCode.CREQ, 7);
        Dispatch(AreaCode.NET, 0, socketMsg);
    }

}
