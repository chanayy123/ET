using MongoDB.Bson.Serialization.Attributes;
using ETModel;
namespace ETModel
{

    [BsonIgnoreExtraElements]
    public partial class UserInfo : Entity
    {
        public const string Property_Coin = "Coin";
        public bool IsRobot { get; set; }
    }
}
