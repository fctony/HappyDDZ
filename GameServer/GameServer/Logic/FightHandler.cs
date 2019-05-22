using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AhpilyServer;
using GameServer.Cache.Fight;
using GameServer.Cache;
using Protocol.Code;
using Protocol.Dto.Fight;
using GameServer.Model;

namespace GameServer.Logic
{
    public class FightHandler : IHandler
    {
        public FightCache fightCache = Caches.Fight;
        public UserCache userCache = Caches.User;

        public void OnDisconnect(ClientPeer client)
        {
            leave(client);
        }

        public void OnReceive(ClientPeer client, int subCode, object value)
        {
            switch (subCode)
            {
                case FightCode.GRAB_LANDLORD_CREQ:
                    //如果是true就是抢地主 如果是false就是不抢
                    bool result = (bool)value;
                    grabLandlord(client, result);
                    break;
                case FightCode.DEAL_CREQ:
                    deal(client, value as DealDto);
                    break;
                case FightCode.PASS_CREQ:
                    pass(client);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 用户离开
        /// </summary>
        /// <param name="client"></param>
        private void leave(ClientPeer client)
        {
            SingleExecute.Instance.Execute(
                () =>
                {
                    if (userCache.IsOnline(client) == false)
                        return;
                    //必须确保在线
                    int userId = userCache.GetId(client);
                    if (fightCache.IsFighting(userId) == false)
                    {
                        return;
                    }
                    FightRoom room = fightCache.GetRoomByUId(userId);

                    //就算中途退出得人
                    room.LeaveUIdList.Add(userId);
                    brocast(room, OpCode.FIGHT, FightCode.LEAVE_BRO, userId);

                    if (room.LeaveUIdList.Count == 3)
                    {
                        //所有人都走了

                        //给逃跑玩家添加逃跑场次
                        for (int i = 0; i < room.LeaveUIdList.Count; i++)
                        {
                            UserModel um = userCache.GetModelById(room.LeaveUIdList[i]);
                            um.RunCount++;
                            um.Been -= (room.Multiple * 1000) * 3;
                            um.Been += 0;
                            userCache.Update(um);
                        }

                        //销毁缓存层的房间数据
                        fightCache.Destroy(room);
                    }
                });
        }

        /// <summary>
        /// 不出的处理
        /// </summary>
        /// <param name="client"></param>
        private void pass(ClientPeer client)
        {
            SingleExecute.Instance.Execute(
                () =>
                {
                    if (userCache.IsOnline(client) == false)
                        return;
                    //必须确保在线
                    int userId = userCache.GetId(client);
                    //if()
                    FightRoom room = fightCache.GetRoomByUId(userId);

                    //分两种情况
                    if (room.roundModel.BiggestUId == userId)
                    {
                        //当前玩家是最大出牌者 没管它 他不能不出
                        client.Send(OpCode.FIGHT, FightCode.PASS_SRES, -1);
                        return;
                    }
                    else
                    {
                        //可以不出
                        client.Send(OpCode.FIGHT, FightCode.PASS_SRES, 0);
                        turn(room);
                    }
                });
        }

        /// <summary>
        /// 出牌的处理
        /// </summary>
        private void deal(ClientPeer client, DealDto dto)
        {
            SingleExecute.Instance.Execute(
                delegate ()
                {
                    if (userCache.IsOnline(client) == false)
                        return;
                    //必须确保在线
                    int userId = userCache.GetId(client);
                    if(userId != dto.UserId)
                    {
                        return;
                    }
                    //if()
                    FightRoom room = fightCache.GetRoomByUId(userId);

                    //玩家出牌  2种
                    //玩家已经中途退出 掉线
                    if (room.LeaveUIdList.Contains(userId))
                    {
                        //直接转换出牌
                        turn(room);
                    }
                    //玩家还在
                    bool canDeal = room.DeadCard(dto.Type, dto.Weight, dto.Length, userId, dto.SelectCardList);
                    if (canDeal == false)
                    {
                        //玩家出的牌管不上上一个玩家出的牌
                        client.Send(OpCode.FIGHT, FightCode.DEAL_SRES, -1);
                        return;
                    }
                    else
                    {
                        //给自身客户端 发送一个出牌成功的消息
                        client.Send(OpCode.FIGHT, FightCode.DEAL_SRES, 0);
                        //广播 给所有的客户端 
                        List<CardDto> remainCardList = room.GetPlayerModel(userId).CardList;
                        dto.RemainCardList = remainCardList;
                        brocast(room, OpCode.FIGHT, FightCode.DEAL_BRO, dto);
                        //检测一下剩余手牌 如果手牌数为0 那就游戏结束了
                        if (remainCardList.Count == 0)
                        {
                            //游戏结束
                            gameOver(userId, room);
                        }
                        else
                        {
                            //直接转换出牌
                            turn(room);
                        }
                    }
                });
        }

        /// <summary>
        /// 游戏结束
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="room"></param>
        private void gameOver(int userId, FightRoom room)
        {
            //获取获胜身份 所有的玩家id
            int winIdentity = room.GetPlayerIdentity(userId);
            int winBeen = room.Multiple * 1000;
            //给胜利的玩家添加胜场
            List<int> winUIds = room.GetSameIdentityUIds(winIdentity);
            for (int i = 0; i < winUIds.Count; i++)
            {
                UserModel um = userCache.GetModelById(winUIds[i]);
                um.WinCount++;
                um.Been += winBeen;
                um.Exp += 100;
                userCache.Update(um);
            }
            //给失败的玩家添加负场
            List<int> loseUIds = room.GetDifferentIdentityUIds(winIdentity);
            for (int i = 0; i < loseUIds.Count; i++)
            {
                UserModel um = userCache.GetModelById(loseUIds[i]);
                um.LoseCount++;
                um.Been -= winBeen;
                um.Exp += 10;
                userCache.Update(um);
            }
            //给逃跑玩家添加逃跑场次
            for (int i = 0; i < room.LeaveUIdList.Count; i++)
            {
                UserModel um = userCache.GetModelById(room.LeaveUIdList[i]);
                um.RunCount++;
                um.Been -= (winBeen) * 3;
                um.Been += 0;
                userCache.Update(um);
            }

            //给客户端发消息  赢的身份是什么？ 谁赢了？ 加多少豆子？
            OverDto dto = new OverDto();
            dto.WinIdentity = winIdentity;
            dto.WinUIdList = winUIds;
            dto.BeenCount = winBeen;
            brocast(room, OpCode.FIGHT, FightCode.OVER_BRO, dto);

            //在缓存层销毁房间数据
            fightCache.Destroy(room);
        }

        /// <summary>
        /// 转换出牌
        /// </summary>
        private void turn(FightRoom room)
        {
            //下一个出牌的id
            int nextUId = room.Turn();
            if (room.IsOffline(nextUId) == true)
            {
                //如果下一个玩家掉线了 递归直到不掉线的玩家出牌为止
                turn(room);
            }
            else
            {
                //玩家不掉线就给他发消息 轮到他出牌了
                //ClientPeer nextClient = userCache.GetClientPeer(nextUId);
                // nextClient.Send(OpCode.FIGHT, FightCode.TURN_DEAL_BRO, nextUId);
                brocast(room, OpCode.FIGHT, FightCode.TURN_DEAL_BRO, nextUId);
            }
        }

        /// <summary>
        /// 抢地主的处理
        /// </summary>
        private void grabLandlord(ClientPeer client, bool result)
        {
            SingleExecute.Instance.Execute(
                delegate ()
                {
                    if (userCache.IsOnline(client) == false)
                        return;
                    //必须确保在线
                    int userId = userCache.GetId(client);
                    //if()
                    FightRoom room = fightCache.GetRoomByUId(userId);

                    if (result == true)
                    {
                        //抢
                        room.SetLandlord(userId);
                        //给每一个客户端发一个消息  谁当了地主 
                        //   第一个：地主id  第二个：三张底牌
                        GrabDto dto = new GrabDto(userId, room.TableCardList, room.GetUserCards(userId));
                        brocast(room, OpCode.FIGHT, FightCode.GRAB_LANDLORD_BRO, dto);
                        //发送一个出牌命令
                        brocast(room, OpCode.FIGHT, FightCode.TURN_DEAL_BRO, userId);
                    }
                    else
                    {
                        //不抢
                        int nextUId = room.GetNextUId(userId);
                        brocast(room, OpCode.FIGHT, FightCode.TURN_GRAB_BRO, nextUId);
                    }

                });
        }

        /// <summary>
        /// 开始战斗
        /// </summary>
        public void StartFight(List<int> uidList)
        {
            SingleExecute.Instance.Execute(
                delegate ()
                {
                    //创建战斗房间
                    FightRoom room = fightCache.Create(uidList);
                    room.InitPlayerCards();
                    room.Sort();
                    //发送给每个客户端 他自身有什么牌
                    foreach (int uid in uidList)
                    {
                        ClientPeer client = userCache.GetClientPeer(uid);
                        List<CardDto> cardList = room.GetUserCards(uid);
                        //int[] cardIds = new int[17];
                        //54 每一张牌 都是一个 id
                        //红桃A 是 0
                        //红桃2 是 1
                        //红桃3 是 2
                        //红桃4 是 3
                        //红桃5 是 4
                        //发送的int数组 是 234
                        client.Send(OpCode.FIGHT, FightCode.GET_CARD_SRES, cardList);
                    }
                    //发送开始抢地主的响应
                    int firstUserId = room.GetFirstUId();

                    brocast(room, OpCode.FIGHT, FightCode.TURN_GRAB_BRO, firstUserId, null);
                });
        }

        /// <summary>
        /// 广播
        /// </summary>
        /// <param name="opCode"></param>
        /// <param name="subCode"></param>
        /// <param name="value"></param>
        /// <param name="exClient"></param>
        private void brocast(FightRoom room, int opCode, int subCode, object value, ClientPeer exClient = null)
        {
            SocketMsg msg = new SocketMsg(opCode, subCode, value);
            byte[] data = EncodeTool.EncodeMsg(msg);
            byte[] packet = EncodeTool.EncodePacket(data);

            foreach (var player in room.PlayerList)
            {
                //fixbug923 
                if (userCache.IsOnline(player.UserId))
                {
                    ClientPeer client = userCache.GetClientPeer(player.UserId);
                    if (client == exClient)
                        continue;
                    client.Send(packet);
                }
            }
        }

    }
}
