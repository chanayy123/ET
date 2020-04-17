using System;
using System.Collections.Generic;
using System.Text;
using ETHotfix;
namespace ETModel
{

    public class RealmOnlineUserComponent : Component
    {
        private readonly Dictionary<int,long> onlineList = new Dictionary<int, long>();

        public static RealmOnlineUserComponent Instance { get; private set; }

        public RealmOnlineUserComponent()
        {
            Instance = this;
        }

        public void Add(int userId,long gateSessionId)
        {
            var flag = onlineList.TryAdd(userId, gateSessionId);
            if (!flag) Log.Warning($"realm服重复添加{userId}");
        }

        public void Remove(int userId)
        {
            onlineList.Remove(userId);
        }

        public long Get(int userId)
        {
            onlineList.TryGetValue(userId, out  long gateId);
            return gateId;
        }

    }
}
