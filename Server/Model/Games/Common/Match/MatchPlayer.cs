using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETModel
{
    public partial class MatchPlayer : Entity {
        public UserInfo UserInfo { get; set; }
        public int RoomId { get; set; }
        public int HallId { get; set; }
        public long GateSessionId { get; set; }
        public bool IsRobot
        {
            get
            {
                return UserInfo.IsRobot;
            }
        }
        public int UserId
        {
            get
            {
                return UserInfo.UserId;
            }
        }
    }

}
