using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
namespace ETHotfix
{

    [ObjectSystem]
    public class BullClassicPlayerAwakeSystem : AwakeSystem<BullClassicPlayer, GamePlayerData>
    {
        public override void Awake(BullClassicPlayer self, GamePlayerData data)
        {
            self.Awake(data);
        }
    }

    [ObjectSystem]
    public class BullClassicRoomAwakeSystem : AwakeSystem<BullClassicRoom, GameRoomData>
    {
        public override void Awake(BullClassicRoom self, GameRoomData data)
        {
            self.Awake(data);
        }
    }
    [ObjectSystem]
    public class BullClassicRoomAwakeSystem2 : AwakeSystem<BullClassicRoom, int>
    {
        public override void Awake(BullClassicRoom self, int roomId)
        {
            self.Awake(roomId);
        }
    }





}
