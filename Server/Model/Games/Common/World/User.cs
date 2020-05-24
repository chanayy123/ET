using MongoDB.Bson.Serialization.Attributes;
using ETModel;
namespace ETModel
{
    [BsonIgnoreExtraElements]
    public partial class User : Entity
    {
        public long GateSessionId { get; set; }
        public bool Online { get; set; }
        public long ActorId { get; set; }
        public int GameId { get; set; }
        public int RoomId { get; set; }
        public UserInfo UserInfo { get; set; }

        public void Awake(UserInfo info)
        {
            UserInfo = info;
            GateSessionId = 0;
            Online = false;
            ActorId = 0;
            GameId = 0;
            RoomId = 0;
        }
    }
}
