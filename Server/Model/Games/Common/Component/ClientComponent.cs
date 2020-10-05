using System;
using System.Collections.Generic;
using System.Text;

namespace ETModel
{
    /// <summary>
    /// 游戏客户端组件:模拟测试
    /// </summary>
    public class ClientComponent :Component
    {
        public const int INIT_COUNT = 100;
        public readonly List<UserInfo> clientList = new List<UserInfo>();
        public readonly Dictionary<int, Session> clientSessionDic = new Dictionary<int, Session>();
        public NetOuterComponent Net { get; set; }

        public void Awake(NetworkProtocol protocol)
        {
            this.Net = Game.Scene.AddComponent<NetOuterComponent,NetworkProtocol>(protocol);
        }

    }
}
