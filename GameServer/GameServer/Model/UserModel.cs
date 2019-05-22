using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model
{
    /// <summary>
    /// 角色数据模型
    /// </summary>
    public class UserModel
    {
        public int Id;//唯一id
        public string Name;//角色名字
        public int Been;//豆子的数量

        public int WinCount;//胜场
        public int LoseCount;//负场
        public int RunCount;//逃跑场

        public int Lv;//等级
        public int Exp;//经验

        //金币 游戏币...

        public int AccountId;//外键：与这个角色所关联的账号Id


        public UserModel(int id, string name, int accountId)
        {
            this.Id = id;
            this.Name = name;
            this.Been = 10000;
            this.WinCount = 0;
            this.LoseCount = 0;
            this.RunCount = 0;
            this.Lv = 1;
            this.Exp = 0;
            this.AccountId = accountId;
        }

    }
}
