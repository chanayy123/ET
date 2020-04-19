using System;
using System.Collections.Generic;
using System.Text;
using ETHotfix;
namespace ETModel
{
    /// <summary>
    /// 匹配服房间管理组件
    /// </summary>
    public class MatchRoomComponent : Component
    {
        /// <summary>
        /// 房间大厅id:对应生成房间列表
        /// </summary>
        public readonly Dictionary<long, List<MatchRoom>> roomTemplatesDic = new Dictionary<long, List<MatchRoom>>();
        /// <summary>
        /// 房间id : 房间对象
        /// </summary>
        public readonly Dictionary<int, MatchRoom> roomsDic = new Dictionary<int, MatchRoom>();
        /// <summary>
        /// userId : 匹配玩家对象
        /// </summary>
        public readonly Dictionary<int, MatchPlayer> userDic = new Dictionary<int, MatchPlayer>();
        /// <summary>
        /// 大厅id : 大厅玩家列表
        /// </summary>
        public readonly Dictionary<long, List<HallPlayer>> hallUserListDic = new Dictionary<long, List<HallPlayer>>();
        /// <summary>
        /// userid: 大厅玩家
        /// </summary>
        public readonly Dictionary<int, HallPlayer> hallUserDic = new Dictionary<int, HallPlayer>();
        /// <summary>
        /// 房间大厅id:状态改变的房间列表
        /// </summary>
        public readonly Dictionary<long, List<MatchRoom>> roomDirtyDic = new Dictionary<long, List<MatchRoom>>();
    }
}
