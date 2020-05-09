using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETModel
{
    public partial class BullPlayerData : Component
    {
        public override void Dispose()
        {
            if (this.IsDisposed) return;
            base.Dispose();
            Data?.Dispose();
        }
    }
}
