using MongoDB.Bson.Serialization.Attributes;
using ETModel;
namespace ETModel
{
    [BsonIgnoreExtraElements]
    public partial class UserInfo : Entity
    {
    }
}
