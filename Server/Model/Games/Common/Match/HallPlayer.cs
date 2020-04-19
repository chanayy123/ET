using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETModel
{
    /// <summary>
    /// 大厅玩家对象:在一个大厅的玩家会收到当前大厅房间列表数据变更
    /// </summary>
    public partial class HallPlayer : Entity {
        public int UserId { get; set; }
        public long HallId { get; set; }
        public long GateSessionId { get; set; }
    }

}
