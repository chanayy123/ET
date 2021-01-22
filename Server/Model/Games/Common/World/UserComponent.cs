using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ETHotfix;

namespace ETModel
{

    /// <summary>
    /// 用户服管理类:管理所有在线玩家用户数据
    /// </summary>
    public class UserComponent : Singleton<UserComponent>
    {
        public readonly Dictionary<int, User> userDic = new Dictionary<int, User>();
        public readonly Dictionary<long, Session> cacheSessionDic = new Dictionary<long, Session>();
        public DBProxyComponent DBProxy { get; private set; }
        public int MaxUserId { get; set; }
        /// <summary>
        /// 是否锁定注册
        /// </summary>
        public bool IsLocking { get; set; }
        public void Awake()
        {
            DBProxy = Game.Scene.GetComponent<DBProxyComponent>();
        }
    }
}
