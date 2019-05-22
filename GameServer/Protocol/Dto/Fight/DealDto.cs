using Protocol.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Dto.Fight
{
    [Serializable]
    public class DealDto
    {
        /// <summary>
        /// 选中要出的牌
        /// </summary>
        public List<CardDto> SelectCardList;
        /// <summary>
        /// 长度
        /// </summary>
        public int Length;
        /// <summary>
        /// 权值
        /// </summary>
        public int Weight;
        /// <summary>
        /// 类型
        /// </summary>
        public int Type;

        /// <summary>
        /// 谁出的牌
        /// </summary>
        public int UserId;
        /// <summary>
        /// 牌是否合法
        /// </summary>
        public bool IsRegular;
        /// <summary>
        /// 剩余的手牌
        /// </summary>
        public List<CardDto> RemainCardList;

        public DealDto()
        {

        }

        public DealDto(List<CardDto> cardList, int uid)
        {
            this.SelectCardList = cardList;
            this.Length = cardList.Count;
            this.Type = CardType.GetCardType(cardList);
                //是不是单牌？
                //是不是对儿
                //是不是顺子
                //是不是。。。
            this.Weight = CardWeight.GetWeight(cardList, this.Type);
            this.UserId = uid;
            this.IsRegular = (this.Type != CardType.NONE);
            this.RemainCardList = new List<CardDto>();
        }
    }
}
