using System;
using System.Collections.Generic;
using AhpilyServer;
using Protocol.Dto;
using GameServer.Cache;
using GameServer.Cache.Match;
using Protocol.Code;
using Protocol.Constant;

namespace GameServer.Logic
{
    public class ChatHandler : IHandler
    {
        private UserCache userCache = Caches.User;
        private MatchCache matchCache = Caches.Match;

        public void OnDisconnect(ClientPeer client)
        {

        }

        public void OnReceive(ClientPeer client, int subCode, object value)
        {
            switch (subCode)
            {
                case ChatCode.CREQ:
                    chatRequest(client, (int)value);
                    break;
                default:
                    break;
            }
        }

        private void chatRequest(ClientPeer client, int chatType)
        {
            //接收到的是 聊天类型
            //返回的是什么？
            if (userCache.IsOnline(client) == false)
                return;
            int userId = userCache.GetId(client);
            //谁？ 发送者的id  userID
            //发了什么？  聊天的类型  chatType
            ChatDto dto = new ChatDto(userId, chatType);
            //给谁？  房间内的每一个玩家
            if (matchCache.IsMatching(userId))
            {
                MatchRoom mRoom = matchCache.GetRoom(userId);
                mRoom.Brocast(OpCode.CHAT, ChatCode.SRES, dto);
            }
            else if (false)
            {
                //在这里检测战斗房间
                //TODO
            }
        }
    }
}
