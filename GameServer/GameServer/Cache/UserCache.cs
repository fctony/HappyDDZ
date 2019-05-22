using GameServer.Model;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AhpilyServer;
using AhpilyServer.Concurrent;

namespace GameServer.Cache
{
    /// <summary>
    /// 角色数据缓存层
    /// </summary>
    public class UserCache
    {
        /// <summary>
        /// 角色id  对应的  角色数据模型
        /// </summary>
        private Dictionary<int, UserModel> idModelDict = new Dictionary<int, UserModel>();

        /// <summary>
        /// 账号id  对应的 角色id 
        /// </summary>
        private Dictionary<int, int> accIdUIdDict = new Dictionary<int, int>();
        //ConcurrentDictionary

        /// <summary>
        /// 作为角色的id
        /// </summary>
        ConcurrentInt id = new ConcurrentInt(-1);

        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="name">角色名</param>
        /// <param name="accountId">账号id</param>
        public void Create(string name, int accountId)
        {
            UserModel model = new UserModel(id.Add_Get(), name, accountId);
            //保存到字典里
            idModelDict.Add(model.Id, model);
            accIdUIdDict.Add(model.AccountId, model.Id);
        }

        /// <summary>
        /// 判断此账号下是否有角色
        /// </summary>
        public bool IsExist(int accountId)
        {
            return accIdUIdDict.ContainsKey(accountId);
        }

        /// <summary>
        /// 根据账号id获取角色数据模型
        /// </summary>
        public UserModel GetModelByAccountId(int accountId)
        {
            int userId = accIdUIdDict[accountId];
            UserModel model = idModelDict[userId];
            return model;
        }

        /// <summary>
        /// 根据账号id获取角色id
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public int GetId(int accountId)
        {
            return accIdUIdDict[accountId];
        }

        //存储 在线玩家 只有在线玩家 才有 这个（ClientPeer）对象 
        private Dictionary<int, ClientPeer> idClientDict = new Dictionary<int, ClientPeer>();
        private Dictionary<ClientPeer, int> clientIdDict = new Dictionary<ClientPeer, int>();

        /// <summary>
        /// 是否在线
        /// </summary>
        /// <param name="clientPeer">客户端连接对象</param>
        /// <returns></returns>
        public bool IsOnline(ClientPeer client)
        {
            return clientIdDict.ContainsKey(client);
        }

        /// <summary>
        /// 是否在线
        /// </summary>
        public bool IsOnline(int id)
        {
            return idClientDict.ContainsKey(id);
        }

        /// <summary>
        /// 角色上线
        /// </summary>
        /// <param name="client"></param>
        /// <param name="id"></param>
        public void Online(ClientPeer client, int id)
        {
            idClientDict.Add(id, client);
            clientIdDict.Add(client, id);
        }

        /// <summary>
        /// 更新角色数据
        /// </summary>
        /// <param name="model"></param>
        public void Update(UserModel model)
        {
            idModelDict[model.Id] = model;
        }

        /// <summary>
        /// 角色下线
        /// </summary>
        /// <param name="client"></param>
        public void Offline(ClientPeer client)
        {
            int id = clientIdDict[client];
            clientIdDict.Remove(client);
            idClientDict.Remove(id);
        }

        /// <summary>
        /// 根据连接对象获取角色模型
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public UserModel GetModelByClientPeer(ClientPeer client)
        {
            int id = clientIdDict[client];
            UserModel model = idModelDict[id];
            return model;
        }

        /// <summary>
        /// 根据用户id获取数据模型
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public UserModel GetModelById(int userId)
        {
            UserModel user = idModelDict[userId];
            return user;
        }

        /// <summary>
        /// 根据角色id获取连接对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ClientPeer GetClientPeer(int id)
        {
            return idClientDict[id];
        }

        /// <summary>
        /// 根据在线玩家的连接对象 获取 角色id
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public int GetId(ClientPeer client)
        {
            if (!clientIdDict.ContainsKey(client))
            {
                throw new IndexOutOfRangeException("这个玩家不在在线的字典里面存储！");
            }
            return clientIdDict[client];
        }
    }
}
