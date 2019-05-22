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
    /// 牌库
    ///   什么？就是54张牌的地方
    /// </summary>
    public class LibraryModel
    {
        /// <summary>
        /// 所有牌的队列
        /// </summary>
        public Queue<CardDto> CardQueue { get; set; }

        public LibraryModel()
        {
            //创建牌
            create();
            //洗牌
            shuffle();
        }

        public void Init()
        {
            //创建牌
            create();
            //洗牌
            shuffle();
        }

        private void create()
        {
            CardQueue = new Queue<CardDto>();
            //创建普通的牌
            for (int color = CardColor.CLUB; color <= CardColor.SQUARE; color++)
            {
                for (int weight = CardWeight.THREE; weight <= CardWeight.TWO; weight++)
                {
                    string cardName = CardColor.GetString(color) + CardWeight.GetString(weight);
                    CardDto card = new CardDto(cardName, color, weight);
                    //添加到 CardQueue  里面
                    CardQueue.Enqueue(card);
                }
            }
            //大王小王
            CardDto sJoker = new CardDto("SJoker", CardColor.NONE, CardWeight.SJOKER);
            CardDto lJoker = new CardDto("LJoker", CardColor.NONE, CardWeight.LJOKER);
            CardQueue.Enqueue(sJoker);
            CardQueue.Enqueue(lJoker);
        }

        /// <summary>
        /// 洗牌
        /// </summary>
        private void shuffle()
        {
            List<CardDto> newList = new List<CardDto>();
            Random r = new Random();
            // 1 2 3 4 5 6 7
            foreach (CardDto card in CardQueue)
            {
                int index = r.Next(0, newList.Count + 1);
                // 6 2 5 4 3 7 1...
                newList.Insert(index, card);
            }
            CardQueue.Clear();
            foreach (CardDto card in newList)
            {
                CardQueue.Enqueue(card);
            }
        }

        /// <summary>
        /// 发牌
        /// </summary>
        /// <returns></returns>
        public CardDto Deal()
        {
            return CardQueue.Dequeue();
        }

    }
}
