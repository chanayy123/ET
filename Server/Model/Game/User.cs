using MongoDB.Bson.Serialization.Attributes;
using ETModel;
namespace ETHotfix
{
    [BsonIgnoreExtraElements]
    public partial class User : Entity
    {
    }
}
