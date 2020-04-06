using System;
using System.Collections.Generic;
using System.Text;
using ETHotfix;
namespace ETModel
{

    public class OnlineUserComponent : Component
    {
        private readonly Dictionary<int,int> onlineList = new Dictionary<int, int>();

        public static OnlineUserComponent Instance { get; private set; }

        public OnlineUserComponent()
        {
            Instance = this;
        }

        public void Add(int userId,int gateId)
        {
            onlineList.Add(userId,gateId);
        }

        public void Remove(int userId)
        {
            onlineList.Remove(userId);
        }

        public int Get(int userId)
        {
            onlineList.TryGetValue(userId, out  int gateId);
            return gateId;
        }

    }
}
