using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
    public partial class BullFightRoomData : Component
    {
        public override void Dispose()
        {
            if (this.IsDisposed) return;
            base.Dispose();
            PlayerList.Clear();
            Data?.Dispose();
        }
    }
}
