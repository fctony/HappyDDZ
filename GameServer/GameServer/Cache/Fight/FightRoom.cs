using AhpilyServer;
using Protocol.Constant;
using Protocol.Dto.Fight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Cache.Fight
{
    /// <summary>
    /// 战斗房间
    /// </summary>
    public class FightRoom
    {
        /// <summary>
        /// 房间唯一标识码
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// 存储所有玩家
        /// </summary>
        public List<PlayerDto> PlayerList { get; set; }
        /// <summary>
        /// 中途退出的玩家id列表
        /// </summary>
        public List<int> LeaveUIdList { get; set; }

        /// <summary>
        /// 牌库
        /// </summary>
        public LibraryModel libraryModel { get; set; }
        /// <summary>
        /// 底牌
        /// </summary>
        public List<CardDto> TableCardList { get; set; }
        /// <summary>
        /// 倍数
        /// </summary>
        public int Multiple { get; set; }

        /// <summary>
        /// 回合管理类
        /// </summary>
        public RoundModel roundModel { get; set; }

        /// <summary>
        /// 构造方法 做初始化
        /// </summary>
        /// <param name="id"></param>
        public FightRoom(int id, List<int> uidList)
        {
            this.Id = id;
            this.PlayerList = new List<PlayerDto>();
            foreach (int uid in uidList)
            {
                PlayerDto player = new PlayerDto(uid);
                this.PlayerList.Add(player);
            }
            this.LeaveUIdList = new List<int>();
            this.libraryModel = new LibraryModel();
            this.TableCardList = new List<CardDto>();
            this.Multiple = 1;
            this.roundModel = new RoundModel();
        }

        public void Init(List<int> uidList)
        {
            foreach (int uid in uidList)
            {
                PlayerDto player = new PlayerDto(uid);
                this.PlayerList.Add(player);
            }
        }

        public bool IsOffline(int uid)
        {
            return LeaveUIdList.Contains(uid);
        }

        /// <summary>
        /// 转换出牌
        /// </summary>
        public int Turn()
        {
            int currUId = roundModel.CurrentUId;
            int nextUId = GetNextUId(currUId);
            //更改回合信息
            roundModel.CurrentUId = nextUId;
            return nextUId;
        }

        /// <summary>
        /// 如何计算下一个出牌者
        /// </summary>
        /// <param name="currUId">当前出牌者</param>
        /// <returns></returns>
        public int GetNextUId(int currUId)
        {
            for (int i = 0; i < PlayerList.Count; i++)
            {
                if (PlayerList[i].UserId == currUId)
                {
                    // 1 2 3
                    if (i == 2)
                        return PlayerList[0].UserId;
                    else
                        return PlayerList[i + 1].UserId;
                }
            }
            //如果这里还没获取到 说明传来的出牌者 是错误的
            throw new Exception("并没有这个出牌者！");
            //return -1;
        }

        /// <summary>
        /// 出牌（判断能不能压上一回合的牌）
        /// </summary>
        /// <returns></returns>
        public bool DeadCard(int type, int weight, int length, int userId, List<CardDto> cardList)
        {
            bool canDeal = false;

            //用什么牌管什么牌 大的才能管小的
            if (type == roundModel.LastCardType && weight > roundModel.LastWeight)
            {
                //特殊的类型 ：顺子  还要进行长度的限制
                if (type == CardType.STRAIGHT || type == CardType.DOUBLE_STRAIGHT || type == CardType.TRIPLE_STRAIGHT)
                {
                    if (length == roundModel.LastLength)
                    {
                        //满足出牌条件
                        canDeal = true;
                    }
                }
                else
                {
                    //满足出牌条件
                    canDeal = true;
                }
            }
            //普通的炸弹 可以管 任何不是炸弹的牌 
            else if (type == CardType.BOOM && roundModel.LastCardType != CardType.BOOM)
            {
                //满足出牌条件
                canDeal = true;
            }
            //王炸可以管任何牌
            else if (type == CardType.JOKER_BOOM)
            {
                //满足出牌条件
                canDeal = true;
            }
            //第一次出牌 或者 当前自己是最大出牌者
            else if (userId == roundModel.BiggestUId)
            {
                canDeal = true;
            }

            //出牌 
            if (canDeal)
            {
                //移除玩家的手牌
                removeCards(userId, cardList);
                //可能会翻倍
                if (type == CardType.BOOM)
                {
                    this.Multiple *= 4;
                }
                else if (type == CardType.JOKER_BOOM)
                {
                    this.Multiple *= 8;
                }
                //保存回合信息
                roundModel.Change(length, type, weight, userId);
            }

            return canDeal;
        }

        /// <summary>
        /// 移除玩家手牌
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cardList"></param>
        private void removeCards(int userId, List<CardDto> cardList)
        {
            ////获取玩家现有的手牌
            //List<CardDto> currList = GetUserCards(userId);
            //List<int> index = new List<int>();
            //foreach (CardDto temp in cardList)
            //{
            //for (int i = currList.Count - 1; i >= 0; i--)
            //{
            //    if (currList[i].Name == temp.Name)
            //    {
            //    //currList.RemoveAt(i);
            //    currList.Remove(currList[i]);
            //    }
            //}
            //}

            List<CardDto> currList = GetUserCards(userId);
            List<CardDto> list = new List<CardDto>();
            foreach (var select in cardList)
            {
                for (int i = currList.Count - 1; i >= 0; i--)
                {
                    if (currList[i].Name == select.Name)
                    {
                        list.Add(currList[i]);
                        break;
                    }
                }
            }
            foreach (var card in list)
                currList.Remove(card);
        }


        /// <summary>
        /// 获取玩家的现有手牌
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<CardDto> GetUserCards(int userId)
        {
            foreach (PlayerDto player in PlayerList)
            {
                if (player.UserId == userId)
                    return player.CardList;
            }
            throw new Exception("不存在这个玩家！");
        }

        /// <summary>
        /// 发牌 (初始化角色手牌)
        /// </summary>
        public void InitPlayerCards()
        {
            //54 
            //一个人 17 张
            //17*3=51  
            //剩3个 当底牌
            //谁是地主 就 给谁
            for (int i = 0; i < 17; i++)
            {
                CardDto card = libraryModel.Deal();
                PlayerList[0].Add(card);
            }
            for (int i = 0; i < 17; i++)
            {
                CardDto card = libraryModel.Deal();
                PlayerList[1].Add(card);
            }
            for (int i = 0; i < 17; i++)
            {
                CardDto card = libraryModel.Deal();
                PlayerList[2].Add(card);
            }
            //发底牌
            for (int i = 0; i < 3; i++)
            {
                CardDto card = libraryModel.Deal();
                TableCardList.Add(card);
            }
        }

        /// <summary>
        /// 设置地主身份
        /// </summary>
        public void SetLandlord(int userId)
        {
            foreach (PlayerDto player in PlayerList)
            {
                if (player.UserId == userId)
                {
                    //找对人了
                    player.Identity = Identity.LANDLORD;
                    //给地主发底牌
                    for (int i = 0; i < TableCardList.Count; i++)
                    {
                        player.Add(TableCardList[i]);
                    }
                    //重新排序
                    this.Sort();
                    //开始回合
                    roundModel.Start(userId);
                }
            }
        }

        /// <summary>
        /// 获取玩家的数据模型
        /// </summary>
        /// <returns></returns>
        public PlayerDto GetPlayerModel(int userId)
        {
            foreach (PlayerDto player in PlayerList)
            {
                if (player.UserId == userId)
                {
                    return player;
                }
            }
            throw new Exception("没有这个玩家！获取不到数据");
            //return null;
        }

        /// <summary>
        /// 获取用户的身份
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int GetPlayerIdentity(int userId)
        {
            return GetPlayerModel(userId).Identity;
            throw new Exception("没有这个玩家！获取不到数据");
        }

        /// <summary>
        /// 获取相同身份的用户id
        /// </summary>
        /// <returns></returns>
        public List<int> GetSameIdentityUIds(int identity)
        {
            List<int> uids = new List<int>();
            foreach (PlayerDto player in PlayerList)
            {
                if (player.Identity == identity)
                {
                    uids.Add(player.UserId);
                }
            }
            return uids;
        }

        /// <summary>
        /// 获取不同身份的用户id
        /// </summary>
        /// <returns></returns>
        public List<int> GetDifferentIdentityUIds(int identity)
        {
            List<int> uids = new List<int>();
            foreach (PlayerDto player in PlayerList)
            {
                if (player.Identity != identity)
                {
                    uids.Add(player.UserId);
                }
            }
            return uids;
        }

        /// <summary>
        /// 获取房间内第一个玩家的id
        /// </summary>
        /// <returns></returns>
        public int GetFirstUId()
        {
            return PlayerList[0].UserId;
        }

        /// <summary>
        /// 排序手牌
        /// </summary>
        /// <param name="cardList"></param>
        /// <param name="asc"></param>
        private void sortCard(List<CardDto> cardList, bool asc = true)//asc des
        {
            cardList.Sort(
                delegate (CardDto a, CardDto b)
                {
                    if (asc)
                        return a.Weight.CompareTo(b.Weight);
                    else
                        return a.Weight.CompareTo(b.Weight) * -1;
                });
        }

        /// <summary>
        /// 排序 默认升序
        /// </summary>
        public void Sort(bool asc = true)
        {
            sortCard(PlayerList[0].CardList, asc);
            sortCard(PlayerList[1].CardList, asc);
            sortCard(PlayerList[2].CardList, asc);
            sortCard(TableCardList, asc);
        }

    }
}
