using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
namespace ETModel
{
    [BsonIgnoreExtraElements]
    public class RobotUser : Entity
    {
        public bool IsAvailable { get; set; }
        public UserInfo UserInfo { get; set; }
        public int RoomId { get; set; }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();
            IsAvailable = false;
            UserInfo = null;
        }
    }
}
