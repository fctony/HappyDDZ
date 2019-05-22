using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Dto
{
    /// <summary>
    /// 用户数据的传输模型
    /// </summary>
    [Serializable]
    public class UserDto
    {
        public int Id;//由于游戏满足不了需求 所以定义了这个id
        public string Name;//角色名字
        public int Been;//豆子的数量
        public int WinCount;//胜场
        public int LoseCount;//负场
        public int RunCount;//逃跑场
        public int Lv;//等级
        public int Exp;//经验

        public UserDto()
        {

        }

        public UserDto(int id,string name, int been, int winCount, int loseCount, int runCount, int lv, int exp)
        {
            this.Id = id;
            this.Name = name;
            this.Been = been;
            this.WinCount = winCount;
            this.LoseCount = loseCount;
            this.RunCount = runCount;
            this.Lv = lv;
            this.Exp = exp;
        }
    }
}
