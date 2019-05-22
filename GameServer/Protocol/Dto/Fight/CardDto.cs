using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Dto.Fight
{
    /// <summary>
    /// 表示卡牌
    /// </summary>
    [Serializable]
    public class CardDto
    {
        public string Name;
        public int Color;//红桃
        public int Weight;//9

        public CardDto()
        {

        }

        public CardDto(string name, int color, int weight)
        {
            this.Name = name;
            this.Color = color;
            this.Weight = weight;
        }

    }
}
