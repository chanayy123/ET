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
        public static void Awake(this MatchPlayer self, int userId, int roomId, long sessionId)
        {
            self.UserId = userId;
            self.RoomId = roomId;
            self.GateSessionId = sessionId;
        }
    }

    public static class MatchRoomExtensions
    {
        public static void Awake(this MatchRoom self, int roomId,RoomConfig cfg)
        {
            self.RoomId = roomId;
            self.Config = cfg;
            self.PlayerList = self.PlayerList ?? new List<MatchPlayer>();
        }
    }

    public static class MatchRoomComponentExtensions
    {
        public static MatchRoom CreateRoom(this MatchRoomComponent self, int roomId,RoomConfig cfg)
        {
            var room = ComponentFactory.Create<MatchRoom,int,RoomConfig>(roomId,cfg);
            return room;
        }

        public static void Add(this MatchRoomComponent self, long hallId, MatchRoom room)
        {
            if (!self.roomTemplatesDic.TryGetValue(hallId, out List<MatchRoom> list))
            {
                list = new List<MatchRoom>();
                self.roomTemplatesDic.Add(hallId, list);
            }
            list.Add(room);
            self.roomsDic.Add(room.RoomId, room);
        }

        public static void EnterRoom(this MatchRoomComponent self,MatchPlayer player)
        {
            var room = self.GetByRoomId(player.RoomId);
            self.userDic.Add(player.UserId, player);
            room.PlayerList.Add(player);
            room.Count = room.PlayerList.Count;
            self.SetDirty(room);
        }

        public static void LeaveRoom(this MatchRoomComponent self, int userId)
        {
            var room = self.GetByUserId(userId);
            self.userDic.Remove(userId, out MatchPlayer player);
            room.PlayerList.Remove(player);
            room.Count = room.PlayerList.Count;
            player?.Dispose();
            self.SetDirty(room);
        }

        public static void UpdateRoom(this MatchRoomComponent self, int roomId,int state,long roomActorId)
        {
            var room = self.GetByRoomId(roomId);
            room.State = state;
            room.RoomActorId = roomActorId;
            self.SetDirty(room);
        }

        public static void SetDirty(this MatchRoomComponent self, MatchRoom room)
        {
            var hallId = room.Config.Id;
            if(!self.roomDirtyDic.TryGetValue(hallId,out List<MatchRoom> list)){
                list = new List<MatchRoom>();
                self.roomDirtyDic.Add(hallId, list);
            }
            if(!list.Contains(room))
            {
                list.Add(room);
            }
        }
        /// <summary>
        /// 广播所有大厅玩家:状态变化的房间
        /// </summary>
        /// <param name="self"></param>
        public static void BroadcastRoomChanged(this MatchRoomComponent self)
        {
            var msg = SimplePool.Instance.Fetch<SC_RoomListChanged>();
            foreach (var item in self.roomDirtyDic)
            {
                if (item.Value.Count == 0) continue;
                if (self.hallUserListDic.TryGetValue(item.Key,out List<HallPlayer> list))
                {
                    foreach (var player in list)
                    {
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

        public static OpRetCode CanEnterRoom(this MatchRoomComponent self, int roomId, User user)
        {
            if(self.userDic.ContainsKey(user.UserInfo.UserId))
            {
                return OpRetCode.RoomAlreadyIn;
            }
            var room = self.GetByRoomId(roomId);
            if (room == null)
            {
                return OpRetCode.RoomNotExist;
            }
            if (room.State == (int)RoomState.GAMING)
            {
                return OpRetCode.RoomAlreadyGaming;
            }
                if (room.Count >= room.Config.MaxPlayers)
            {
                return OpRetCode.RoomAlreadyFull;
            }
            if(user.UserInfo.Coin < room.Config.MinLimitCoin )
            {
                return OpRetCode.RoomNotEnoughCoin;
            }
            if(room.Config.MaxLimitCoin !=-1 && user.UserInfo.Coin > room.Config.MaxLimitCoin)
            {
                return OpRetCode.RoomTooMuchCoin;
            }
            return OpRetCode.Success;
        }

        public static OpRetCode CanLeaveRoom(this MatchRoomComponent self, int userId)
        {
            if (!self.userDic.ContainsKey(userId))
            {
                return OpRetCode.RoomAlreadyOut;
            }
            var room = self.GetByRoomId(self.userDic[userId].RoomId);
            if(room.State == (int)RoomState.GAMING)
            {
                return OpRetCode.RoomAlreadyGaming;
            }
            return OpRetCode.Success;
        }

        public static MatchPlayer GetMatchPlayer(this MatchRoomComponent self,int userId)
        {
            self.userDic.TryGetValue(userId, out MatchPlayer player);
            return player;
        }

        public static MatchRoom GetByRoomId(this MatchRoomComponent self, int roomId)
        {
            self.roomsDic.TryGetValue(roomId, out MatchRoom room);
            return room;
        }

        public static MatchRoom GetByUserId(this MatchRoomComponent self, int userId)
        {
            self.userDic.TryGetValue(userId, out MatchPlayer player);
            if(player != null)
            {
                return self.GetByRoomId(player.RoomId);
            }
            return null;
        }

        public static MatchRoom[] GetAll(this MatchRoomComponent self, long hallId)
        {
            if (self.roomTemplatesDic.TryGetValue(hallId, out List<MatchRoom> list))
            {
                return list.ToArray();        
            }
            return null;
        }

        public static void EnterHall(this MatchRoomComponent self, long hallId,HallPlayer player)
        {
            if(player.HallId == 0)
            {
                if (!self.hallUserListDic.TryGetValue(hallId, out List<HallPlayer> list))
                {
                    list = new List<HallPlayer>();
                    self.hallUserListDic.Add(hallId, list);
                }
                player.HallId = hallId;
                list.Add(player);
            }
            else if(player.HallId != hallId) //进入不同大厅,先离开之前大厅
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
        }

        public static void LeaveHall(this MatchRoomComponent self,HallPlayer player)
        {
            if(self.hallUserListDic.TryGetValue(player.HallId, out List<HallPlayer> list))
            {
                list.Remove(player);
                player.Dispose();
            }
        }

        public static HallPlayer GetHallPlayer(this MatchRoomComponent self, int userId,long sessionId=0,bool isNewAdd=true)
        {
            if(self.hallUserDic.TryGetValue(userId, out HallPlayer player))
            {
                return player;
            }
            else if(isNewAdd)
            {
                var p = MatchFactory.CreateHallPlayer(userId, sessionId);
                self.hallUserDic.Add(userId, p);
                return p;
            }
            return null;
        }

    }

    [ObjectSystem]
    public class MatchPlayerAwakeSystem : AwakeSystem<MatchPlayer, int, int,long>
    {
        public override void Awake(MatchPlayer self, int userId, int roomId,long sessionId)
        {
            self.Awake(userId, roomId, sessionId);
        }
    }

    [ObjectSystem]
    public class MatchRoomAwakeSystem : AwakeSystem<MatchRoom, int, RoomConfig>
    {
        public override void Awake(MatchRoom self, int roomId, RoomConfig cfg)
        {
            self.Awake(roomId, cfg);
        }
    }


    [ObjectSystem]
    public class MatchRoomComponentAwakeSystem : AwakeSystem<MatchRoomComponent>
    {
        public const int DEFAULT_ROOM_COUNT = 50;
        public override void Awake(MatchRoomComponent self)
        {
            //房间列表模式:预生成
            IConfig[] list = Game.Scene.GetComponent<ConfigComponent>().GetAll(typeof(RoomConfig));
            for (int i = 0; i < list.Length; ++i)
            {
                var cfg = list[i] as RoomConfig;
                for (var j = 0; j < DEFAULT_ROOM_COUNT; ++j)
                {
                    var room = self.CreateRoom((int)cfg.Id + j, cfg);
                    room.RoomType = RoomType.List;
                    self.Add(cfg.Id, room);
                }
            }
        }
    }

    [ObjectSystem]
    public class MatchRoomComponentStartSystem : StartSystem<MatchRoomComponent>
    {
        public const int REFRESH_ROOM_INTERVAL = 1000; //毫秒
        public override async void Start(MatchRoomComponent self)
        {
            while (true)
            {
                await TimerComponent.Instance.WaitAsync(REFRESH_ROOM_INTERVAL);
                self.BroadcastRoomChanged();
            }
        }
    }


}
