using MongoDB.Bson.Serialization.Attributes;
using ETModel;
namespace ETModel
{
    [BsonIgnoreExtraElements]
    public partial class User : Entity
    {
        [BsonIgnore]
        public long gateSessionId { get; set; }
    }
}
