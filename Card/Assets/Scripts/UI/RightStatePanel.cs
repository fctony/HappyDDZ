using Protocol.Dto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RightStatePanel : StatePanel
{
    //void Awake()
    //fix bug
    protected override void Awake()
    {
        base.Awake();

        Bind(UIEvent.SET_RIGHT_PLAYER_DATA);
    }

    public override void Execute(int eventCode, object message)
    {
        base.Execute(eventCode, message);

        switch (eventCode)
        {
            case UIEvent.SET_RIGHT_PLAYER_DATA:
                this.userDto = message as UserDto;
                break;
            default:
                break;
        }
    }


    protected override void Start()
    {
        base.Start();

        GameModel gm = Models.GameModel;
        if (gm.GetRightUserId() != -1)
        {
            this.userDto = gm.GetUserDto(gm.GetRightUserId());
            MatchRoomDto room = gm.MatchRoomDto;
            //fixbug923
            if (room.ReadyUIdList.Contains(gm.GetRightUserId()))
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
