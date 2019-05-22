using Protocol.Code;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreatePanel : UIBase
{
    private void Awake()
    {
        Bind(UIEvent.CREATE_PANEL_ACTIVE);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.CREATE_PANEL_ACTIVE:
                setPanelActive((bool)message);
                break;
            default:
                break;
        }
    }

    private InputField inputName;
    private Button btnCreate;

    private PromptMsg promptMsg;
    private SocketMsg socketMsg;

    void Start()
    {
        inputName = transform.Find("inputName").GetComponent<InputField>();
        btnCreate = transform.Find("btnCreate").GetComponent<Button>();

        btnCreate.onClick.AddListener(createClick);

        promptMsg = new PromptMsg();
        socketMsg = new SocketMsg();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        btnCreate.onClick.RemoveListener(createClick);
    }

    private void createClick()
    {
        if (string.IsNullOrEmpty(inputName.text))
        {
            //非法输入
            promptMsg.Change("请正确输入您的名称", Color.red);
            Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
            return;
        }

        //自身处理：进行一些其他的判断 比如 长度 符号。。。

        //向服务器发送一个创建的请求
        socketMsg.Change(OpCode.USER, UserCode.CREATE_CREQ, inputName.text);
        Dispatch(AreaCode.NET, 0, socketMsg);
    }

}
