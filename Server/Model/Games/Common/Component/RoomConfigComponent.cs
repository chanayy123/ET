using System;
using System.Collections.Generic;
using System.Text;

namespace ETModel
{
    public class RoomConfigComponent : Singleton<RoomConfigComponent>
    {
        public readonly Dictionary<long, RoomConfig> roomConfigDic = new Dictionary<long, RoomConfig>();

        public void Awake()
        {
        }

        public RoomConfig Get(long hallId)
        {
            roomConfigDic.TryGetValue(hallId, out RoomConfig cfg);
            return cfg;
        }

        public override void Dispose()
        {
            if (this.IsDisposed) return;
            base.Dispose();
            roomConfigDic.Clear();
        }
    }
}
