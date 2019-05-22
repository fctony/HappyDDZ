using Protocol.Dto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 游戏数据的存储类
/// </summary>
public class GameModel
{
    //登录用户的数据
    public UserDto UserDto { get; set; }

    public int Id { get { return UserDto.Id; } }

    //匹配房间的数据
    public MatchRoomDto MatchRoomDto { get; set; }

    public UserDto GetUserDto(int userId)
    {
        return MatchRoomDto.UIdUserDict[userId];
    }
    
    public int GetRightUserId()
    {
        return MatchRoomDto.RightId;
    }

}
