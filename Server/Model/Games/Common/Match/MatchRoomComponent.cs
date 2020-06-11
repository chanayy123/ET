﻿using System;
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
        /// <summary>
        /// 玩家创建房间配置列表
        /// </summary>
        public readonly List<UserRoomCfg> userRoomCfgList = new List<UserRoomCfg>();


        public void AddMatchRoom(int roomId, MatchRoom room)
        {
            bool flag =roomsDic.TryAdd(roomId, room);
            if (!flag) Log.Warning($"加入匹配房间失败: {roomId}");
        }

        public MatchRoom GetMatchRoom(int roomId)
        {
            roomsDic.TryGetValue(roomId, out MatchRoom room);
            return room;
        }
        /// <summary>
        /// 是否在匹配队列或者在房间
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool IsInMatchOrRoom(int userId)
        {
            return userRoomDic.ContainsKey(userId) || userMatchDic.ContainsKey(userId);
        }

        public void AddUserRoomCfg(UserRoomCfg room)
        {
            userRoomCfgList.Add(room);
        }
        public void RemoveUserRoomCfg(UserRoomCfg room)
        {
            userRoomCfgList.Remove(room);
        }

        public bool IsRoomExist(int roomId)
        {
            return roomsDic.ContainsKey(roomId);
        }

    }
}
