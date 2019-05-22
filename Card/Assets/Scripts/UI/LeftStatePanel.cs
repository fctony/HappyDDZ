using Protocol.Dto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeftStatePanel : StatePanel
{
    //fix bug
    protected override void Awake()
    {
        base.Awake();

        Bind(UIEvent.SET_LEFT_PLAYER_DATA);
    }

    public override void Execute(int eventCode, object message)
    {
        base.Execute(eventCode, message);

        switch (eventCode)
        {
            case UIEvent.SET_LEFT_PLAYER_DATA:
                this.userDto = message as UserDto;
                break;
            default:
                break;
        }
    }


    protected override void Start()
    {
        base.Start();

        //fix bug
        //如果！= -1 就代表 有角色
        MatchRoomDto room = Models.GameModel.MatchRoomDto;
        int leftId = room.LeftId;
        if (leftId != -1)
        {
            this.userDto = room.UIdUserDict[leftId];
            //fixbug923
            if (room.ReadyUIdList.Contains(leftId))
            {
                readyState();
            }
            else
            {
                //nothing
            }
        }
        else
        {
            setPanelActive(false);
        }
    }

}
