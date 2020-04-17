using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ETHotfix;

namespace ETModel
{
    /// <summary>
    /// 网关用户管理类
    /// </summary>
   public class GateUserComponent : Component
    {
        public readonly Dictionary<int, GateUser> userDic = new Dictionary<int, GateUser>();
        public static GateUserComponent Instance { get; private set; }

        public void Awake()
        {
            Instance = this;
        }
    }
}
