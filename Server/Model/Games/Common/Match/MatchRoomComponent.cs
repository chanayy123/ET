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
        /// 房间配置缓存
        /// </summary>
        public readonly Dictionary<long, RoomConfig> roomConfigDic = new Dictionary<long, RoomConfig>();
        /// <summary>
        /// 房间大厅id:列表模式房间列表
        /// </summary>
        public readonly Dictionary<long, List<MatchRoom>> hallListModeDic = new Dictionary<long, List<MatchRoom>>();
        /// <summary>
        /// 房间id : 房间对象,包括所有模式房间
        /// </summary>
        public readonly Dictionary<int, MatchRoom> roomsDic = new Dictionary<int, MatchRoom>();
        /// <summary>
        /// userId : 进入房间玩家对象
        /// </summary>
        public readonly Dictionary<int, MatchPlayer> userRoomDic = new Dictionary<int, MatchPlayer>();
        /// <summary>
        /// userId : 进入匹配队列 玩家对象
        /// </summary>
        public readonly Dictionary<int, MatchPlayer> userMatchDic = new Dictionary<int, MatchPlayer>();
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
        /// <summary>
        /// 大厅id:匹配模式房间列表
        /// </summary>
        public readonly Dictionary<long, List<MatchRoom>> hallMatchModeDic = new Dictionary<long, List<MatchRoom>>();
        /// <summary>
        /// 房间大厅id:匹配玩家列表
        /// </summary>
        public readonly Dictionary<long, List<MatchPlayer>> matchQueueDic = new Dictionary<long, List<MatchPlayer>>();
        /// <summary>
        /// 玩家userid: 创建的房卡房间列表
        /// </summary>
        public readonly Dictionary<int, MatchRoom> hallCardModeDic = new Dictionary<int, MatchRoom>();
    }
}
