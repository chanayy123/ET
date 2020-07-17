using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ETModel;
namespace ETHotfix
{

    public static class MatchPlayerExtensions
    {
        public static void Awake(this MatchPlayer self, UserInfo  userInfo, int roomId, long sessionId)
        {
            self.UserInfo = userInfo;
            self.RoomId = roomId;
            self.GateSessionId = sessionId;
            self.HallId = 0;
        }
    }

    public static class MatchRoomExtensions
    {
        public static void Awake(this MatchRoom self, int roomId, RoomConfig cfg)
        {
            self.RoomId = roomId;
            self.Config = cfg;
            self.State = 0;
            self.Count = 0;
            self.RoomActorId = 0;
            self.PlayerList = self.PlayerList ?? new List<MatchPlayer>();
        }
    }

    public static class MatchRoomComponentExtensions
    {

        public static void AddListModeRoom(this MatchRoomComponent self, long hallId, MatchRoom room)
        {
            if (!self.hallListModeDic.TryGetValue(hallId, out List<MatchRoom> list))
            {
                list = new List<MatchRoom>();
                self.hallListModeDic.Add(hallId, list);
            }
            list.Add(room);
            self.AddMatchRoom(room.RoomId, room);
        }

        public static void EnterRoom(this MatchRoomComponent self, MatchPlayer player)
        {
            var room = self.GetByRoomId(player.RoomId);
            self.userRoomDic.Add(player.UserId, player);
            room.PlayerList.Add(player);
            room.Count = room.PlayerList.Count;
            self.SetDirty(room);
        }

        public static void EnterRoom(this MatchRoomComponent self, List<MatchPlayer> playerList)
        {
            if (playerList.Count == 0) return;
            var room = self.GetByRoomId(playerList[0].RoomId);
            foreach (var player in playerList)
            {
                self.userRoomDic.Add(player.UserId, player);
                room.PlayerList.Add(player);
                room.Count = room.PlayerList.Count;
            }
            self.SetDirty(room);
        }

        public static void LeaveRoom(this MatchRoomComponent self, int userId)
        {
            var room = self.GetByUserId(userId);
            self.userRoomDic.Remove(userId, out MatchPlayer player);
            room.PlayerList.Remove(player);
            room.Count = room.PlayerList.Count;
            player?.Dispose();
            self.SetDirty(room);
        }

        public static void LeaveRoom(this MatchRoomComponent self, List<int> userList)
        {
            if (userList.Count == 0) return;
            var room = self.GetByUserId(userList[0]);
            foreach (var userId in userList)
            {
                self.userRoomDic.Remove(userId, out MatchPlayer player);
                room.PlayerList.Remove(player);
                room.Count = room.PlayerList.Count;
                player?.Dispose();
            }
            self.SetDirty(room);
        }

        public static void UpdateRoom(this MatchRoomComponent self, int roomId, int state, long roomActorId)
        {
            var room = self.GetByRoomId(roomId);
            room.State = state;
            room.RoomActorId = roomActorId;
            self.SetDirty(room);
        }

        public static void SetDirty(this MatchRoomComponent self, MatchRoom room)
        {
            var hallId = room.Config.Id;
            if (!self.roomDirtyDic.TryGetValue(hallId, out List<MatchRoom> list)) {
                list = new List<MatchRoom>();
                self.roomDirtyDic.Add(hallId, list);
            }
            if (!list.Contains(room))
            {
                list.Add(room);
            }
        }
        /// <summary>
        /// 广播所有大厅玩家:状态变化的列表模式房间(在游戏房间的不用广播)
        /// </summary>
        /// <param name="self"></param>
        public static void BroadcastRoomChanged(this MatchRoomComponent self)
        {
            var msg = SimplePool.Instance.Fetch<SC_RoomListChanged>();
            foreach (var item in self.roomDirtyDic)
            {
                if (item.Value.Count == 0) continue;
                if (self.hallUserListDic.TryGetValue(item.Key, out List<HallPlayer> list))
                {
                    foreach (var player in list)
                    {
                        if (self.IsInMatchOrRoom(player.UserId)) continue;
                        msg.ActorId = player.GateSessionId;
                        msg.List.Clear();
                        msg.List.AddRange(item.Value);
                        NetInnerHelper.SendActorMsg(msg);
                    }
                }
            }
            SimplePool.Instance.Recycle(msg);
            //广播完毕后,重置dirty房间字典
            foreach (var item in self.roomDirtyDic)
            {
                item.Value.Clear();
            }
        }

        public static OpRetCode CanEnterRoom(this MatchRoomComponent self, int roomId, UserInfo userInfo)
        {
            if (self.IsInMatchOrRoom(userInfo.UserId))
            {
                return OpRetCode.RoomAlreadyIn;
            }
            var room = self.GetByRoomId(roomId);
            if (room == null)
            {
                return OpRetCode.RoomNotExist;
            }
            if (!self.IsHallOpen(room.Config.Id))
            {
                return OpRetCode.RoomIsClosed;
            }
            if (room.State == (int)RoomState.GAMING)
            {
                return OpRetCode.RoomAlreadyGaming;
            }
            if (room.Count >= room.Config.MaxPlayers)
            {
                return OpRetCode.RoomAlreadyFull;
            }
            if (userInfo.Coin < room.Config.MinLimitCoin)
            {
                return OpRetCode.RoomNotEnoughCoin;
            }
            if (room.Config.MaxLimitCoin != -1 && userInfo.Coin > room.Config.MaxLimitCoin)
            {
                return OpRetCode.RoomTooMuchCoin;
            }
            return OpRetCode.Success;
        }

        public static OpRetCode CanLeaveRoom(this MatchRoomComponent self, int userId)
        {
            if (!self.IsInMatchOrRoom(userId))
            {
                return OpRetCode.RoomAlreadyOut;
            }
            var flag = self.userRoomDic.TryGetValue(userId, out MatchPlayer player);
            if (!flag)
            {
                return OpRetCode.MatchPlayerInvalid;
            }
            var room = self.GetByRoomId(player.RoomId);
            if (room.State == (int)RoomState.GAMING)
            {
                return OpRetCode.RoomAlreadyGaming;
            }
            return OpRetCode.Success;
        }


        public static OpRetCode CanEnterMatchQueue(this MatchRoomComponent self, int hallId, UserInfo userInfo)
        {
            if (self.IsInMatchOrRoom(userInfo.UserId)) {
                return OpRetCode.MatchAlreadyIn;
            }
            if (!self.IsHallOpen(hallId))
            {
                return OpRetCode.MatchIsClosed;
            }
            var cfg = RoomConfigComponent.Instance.Get(hallId);
            if(cfg == null)
            {
                return OpRetCode.RoomConfigError;
            }
            if (userInfo.Coin < cfg.MinLimitCoin)
            {
                return OpRetCode.RoomNotEnoughCoin;
            }
            if (cfg.MaxLimitCoin != -1 && userInfo.Coin > cfg.MaxLimitCoin)
            {
                return OpRetCode.RoomTooMuchCoin;
            }
            return OpRetCode.Success;
        }

        public static OpRetCode CanLeaveMatchQueue(this MatchRoomComponent self, int userId)
        {
            if (!self.IsInMatchOrRoom(userId)) {
                return OpRetCode.MatchAlreadyOut;
            }
            return OpRetCode.Success;
        }
        public static void EnterMatchQueue(this MatchRoomComponent self, MatchPlayer player)
        {
            if (!self.matchQueueDic.TryGetValue(player.HallId, out List<MatchPlayer> list))
            {
                list = new List<MatchPlayer>();
                self.matchQueueDic.Add(player.HallId, list);
            }
            list.Add(player);
            self.userMatchDic.Add(player.UserId, player);

        }

        public static void LeaveMatchQueue(this MatchRoomComponent self, int userId)
        {
            if (self.userMatchDic.Remove(userId, out MatchPlayer player))
            {
                if (self.matchQueueDic.TryGetValue(player.HallId, out List<MatchPlayer> list))
                {
                    MatchPlayer _p = list.Find((p) => p.UserId == userId);
                    list.Remove(_p);
                }
            }
        }
        /// <summary>
        /// 获得匹配模式房间: 人数已经凑够,只需返回空房间
        /// </summary>
        /// <param name="self"></param>
        /// <param name="hallId"></param>
        /// <returns></returns>
        public static MatchRoom GetMatchModeRoom(this MatchRoomComponent self, int hallId)
        {
            if (!self.hallMatchModeDic.TryGetValue(hallId, out List<MatchRoom> list))
            {
                list = new List<MatchRoom>();
                self.hallMatchModeDic.Add(hallId, list);
            }
            var cfg = RoomConfigComponent.Instance.Get(hallId);
            var matchRoom = list.Find((room) => room.Count == 0);
            if (matchRoom == null)
            {
                matchRoom = MatchFactory.CreateMatchModeRoom(hallId, cfg);
                list.Add(matchRoom);
                self.AddMatchRoom(matchRoom.RoomId, matchRoom);
            }
            return matchRoom;
        }

        public static MatchRoom GetByRoomId(this MatchRoomComponent self, int roomId)
        {
            self.roomsDic.TryGetValue(roomId, out MatchRoom room);
            return room;
        }

        public static MatchRoom GetByUserId(this MatchRoomComponent self, int userId)
        {
            self.userRoomDic.TryGetValue(userId, out MatchPlayer player);
            if (player != null)
            {
                return self.GetByRoomId(player.RoomId);
            }
            return null;
        }

        public static List<MatchRoom> GetAll(this MatchRoomComponent self, long hallId)
        {
            if (self.hallListModeDic.TryGetValue(hallId, out List<MatchRoom> list))
            {
                return list;
            }
            return null;
        }

        public static void EnterHall(this MatchRoomComponent self, long hallId, HallPlayer player)
        {
            if (player.HallId == 0)
            {
                if (!self.hallUserListDic.TryGetValue(hallId, out List<HallPlayer> list))
                {
                    list = new List<HallPlayer>();
                    self.hallUserListDic.Add(hallId, list);
                }
                player.HallId = hallId;
                list.Add(player);
                self.hallUserDic.Add(player.UserId, player);
            }
            else if (player.HallId != hallId) //进入不同大厅,先离开之前大厅
            {
                self.hallUserListDic.TryGetValue(player.HallId, out List<HallPlayer> list);
                list.Remove(player);
                if (!self.hallUserListDic.TryGetValue(hallId, out List<HallPlayer> list2))
                {
                    list2 = new List<HallPlayer>();
                    self.hallUserListDic.Add(hallId, list2);
                }
                player.HallId = hallId;
                list2.Add(player);
            }
            else
            {
                Log.Debug($"EnterHall:玩家{player.UserId}重复进入大厅{hallId}");
            }
        }

        public static void LeaveHall(this MatchRoomComponent self, HallPlayer player)
        {
            self.hallUserDic.Remove(player.UserId);
            if (self.hallUserListDic.TryGetValue(player.HallId, out List<HallPlayer> list))
            {
                list.Remove(player);
                player.Dispose();
            }
        }

        public static HallPlayer GetHallPlayer(this MatchRoomComponent self, int userId, long sessionId = 0, bool isNewAdd = false)
        {
            if (self.hallUserDic.TryGetValue(userId, out HallPlayer player))
            {
                return player;
            }
            else if (isNewAdd)
            {
                var p = MatchFactory.CreateHallPlayer(userId, sessionId);
                return p;
            }
            return null;
        }

        public static bool IsHallOpen(this MatchRoomComponent self,long hallId)
        {
            return GameConfigCacheComponent.Instance.IsHallOpen(hallId);
        }

    }

}
