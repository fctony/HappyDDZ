using Protocol.Dto.Fight;
using System;
using System.Collections.Generic;

namespace Protocol.Constant
{
    public static class Constant
    {
        /// <summary>
        /// 存储的是聊天类型 对应的  文字
        /// </summary>
        private static Dictionary<int, string> typeTextDict = new Dictionary<int, string>();

        static Constant()
        {
            typeTextDict = new Dictionary<int, string>();

            typeTextDict.Add(1, "大家好，很高兴见到各位~");
            typeTextDict.Add(2, "和你合作真是太愉快啦！");
            typeTextDict.Add(3, "快点吧，我等到花都谢了。");
            typeTextDict.Add(4, "你的牌打得太好了！");
            typeTextDict.Add(5, "不要吵了，有什么好吵的，专心玩游戏吧！");
            typeTextDict.Add(6, "不要走，决战到天亮！");
            typeTextDict.Add(7, "再见了，我会想念大家的~");
        }

        public static string GetChatText(int chatType)
        {
            return typeTextDict[chatType];
        }

        #region 都行
        //public static string GetCardName(int color, int weight)
        //{
        //    string cardName = string.Empty;
        //    switch (weight)
        //    {
        //        case 3:
        //            cardName+= "Three";
        //            break;
        //        case 4:
        //            cardName += "Four";
        //            break;
        //        case 5:
        //            cardName += "Five";
        //            break;
        //        case 6:
        //            cardName += "Six";
        //            break;
        //        case 7:
        //            cardName += "Seven";
        //            break;
        //        case 8:
        //            cardName += "Eight";
        //            break;
        //        case 9:
        //            cardName += "Nine";
        //            break;
        //        case 10:
        //            cardName += "Ten";
        //            break;
        //        case 11:
        //            cardName += "Jack";
        //            break;
        //        case 12:
        //            cardName += "Queen";
        //            break;
        //        case 13:
        //            cardName += "King";
        //            break;
        //        case 14:
        //            cardName += "One";
        //            break;
        //        case 15:
        //            cardName += "Two";
        //            break;
        //        case 16:
        //            cardName += "SJoker";
        //            break;
        //        case 17:
        //            cardName += "LJoker";
        //            break;
        //        default:
        //            throw new Exception("不存在这样的权值");
        //    }
        //    switch (color)
        //    {
        //        case CardColor. CLUE:
        //            cardName+= "Clue";
        //            break;
        //        case CardColor.HEART:
        //            cardName += "Heart";
        //            break;
        //        case CardColor.SPADE:
        //            cardName += "Spade";
        //            break;
        //        case CardColor.SQUARE:
        //            cardName += "Square";
        //            break;
        //        default:
        //            throw new Exception("不存在这样的花色");
        //    }
        //    return cardName;
        //} 

        #endregion

    }
}
