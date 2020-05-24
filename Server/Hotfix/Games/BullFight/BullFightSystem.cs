using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
namespace ETHotfix
{

    [ObjectSystem]
    public class BullFightPlayerAwakeSystem : AwakeSystem<BullFightPlayer, BullFightPlayerData>
    {
        public override void Awake(BullFightPlayer self, BullFightPlayerData data)
        {
            self.Awake(data);
        }
    }

    [ObjectSystem]
    public class BullFightRoomAwakeSystem2 : AwakeSystem<BullFightRoom, int,RoomConfig>
    {
        public override void Awake(BullFightRoom self, int roomId, RoomConfig cfg)
        {
            self.Awake(roomId,cfg);
        }
    }





}
