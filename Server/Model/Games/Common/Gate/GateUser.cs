using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
namespace ETModel
{
    /// <summary>
    /// 网关简易用户对象:只存储userid, actorId等必要数据
    /// </summary>
    [BsonIgnoreExtraElements]
    public class GateUser : Entity
    {
        public int UserId { get; set; }
        public long ActorId { get; set; }
        public Session Session { get; set; }

    }
}
