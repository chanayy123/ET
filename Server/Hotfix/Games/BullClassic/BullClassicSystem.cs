using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
namespace ETHotfix
{

    [ObjectSystem]
    public class BullClassicPlayerAwakeSystem : AwakeSystem<BullClassicPlayer, BullPlayerData>
    {
        public override void Awake(BullClassicPlayer self, BullPlayerData data)
        {
            self.Awake(data);
        }
    }

    [ObjectSystem]
    public class BullClassicRoomAwakeSystem2 : AwakeSystem<BullClassicRoom, int,RoomConfig>
    {
        public override void Awake(BullClassicRoom self, int roomId, RoomConfig cfg)
        {
            self.Awake(roomId,cfg);
        }
    }





}
