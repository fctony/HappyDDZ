using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Protocol.Code;
using Protocol.Dto;

/// <summary>
/// 角色的网络消息处理类
/// </summary>
public class UserHandler : HandlerBase
{
    public override void OnReceive(int subCode, object value)
    {
        switch (subCode)
        {
            case UserCode.CREATE_SRES:
                createResponse((int)value);
                break;
            case UserCode.GET_INFO_SRES:
                getInfoResponse(value as UserDto);
                break;
            case UserCode.ONLINE_SRES:
                onlineResponse((int)value);
                break;
            default:
                break;
        }
    }

    private SocketMsg socketMsg = new SocketMsg();

    /// <summary>
    /// 获取信息的回应
    /// </summary>
    private void getInfoResponse(UserDto user)
    {
        if (user == null)
        {
            //没有角色
            Debug.Log("没有角色 创建");
            //显示创建面板
            Dispatch(AreaCode.UI, UIEvent.CREATE_PANEL_ACTIVE, true);
        }
        else
        {
            //有角色
            //隐藏创建面板
            Dispatch(AreaCode.UI, UIEvent.CREATE_PANEL_ACTIVE, false);

            ////角色上线
            //socketMsg.Change(OpCode.USER, UserCode.ONLINE_CREQ, null);
            //Dispatch(AreaCode.NET, 0, socketMsg);

            //保存服务器发来的角色数据
            //GameModel model = new GameModel();
            Models.GameModel.UserDto = user;

            //更新一下本地的显示
            Dispatch(AreaCode.UI, UIEvent.REFRESH_INFO_PANEL, user);
        }
    }

    /// <summary>
    /// 上线的响应
    /// </summary>
    private void onlineResponse(int result)
    {
        if(result == 0)
        {
            //上线成功
            Debug.Log("上线成功");
        }
        else if(result == -1)
        {
            //客户端非法登录
            Debug.LogError("客户端非法登录");
        }
        else if(result == -2)
        {
            //没有角色 不能创建
            Debug.LogError("非法操作 角色就上线了");
        }
    }

    /// <summary>
    /// 创建角色的响应
    /// </summary>
    private void createResponse(int result)
    {
        if(result == -1)
        {
            Debug.LogError("客户端非法登录");
        }
        else if(result == -2)
        {
            Debug.LogError("已经有角色 重复创建");
        }
        else if(result == 0)
        {
            //创建成功
            //隐藏创建面板
            Dispatch(AreaCode.UI, UIEvent.CREATE_PANEL_ACTIVE, false);
            //获取角色信息
            socketMsg.Change(OpCode.USER, UserCode.GET_INFO_CREQ, null);
            Dispatch(AreaCode.NET, 0, socketMsg);
        }
    }

}
