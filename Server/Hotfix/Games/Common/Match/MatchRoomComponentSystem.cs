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
            self.playerList = self.playerList ?? new List<MatchPlayer>();
        }
    }

    public static class MatchRoomComponentExtensions
    {
        public static MatchRoom CreateRoom(this MatchRoomComponent self, int roomId,RoomConfig cfg)
        {
            var room = ComponentFactory.Create<MatchRoom,int,RoomConfig>(roomId,cfg);
            return room;
        }

        public static void Add(this MatchRoomComponent self, long templateId, MatchRoom room)
        {
            if (!self.roomTemplatesDic.TryGetValue(templateId, out List<MatchRoom> list))
            {
                list = new List<MatchRoom>();
                self.roomTemplatesDic.Add(templateId, list);
            }
            list.Add(room);
            self.roomsDic.Add(room.RoomId, room);
        }

        public static void EnterRoom(this MatchRoomComponent self,MatchPlayer player)
        {
            var room = self.GetByRoomId(player.RoomId);
            self.userDic.Add(player.UserId, player);
            room.playerList.Add(player);
            room.Count = room.playerList.Count;
        }

        public static void LeaveRoom(this MatchRoomComponent self, int userId)
        {
            var room = self.GetByUserId(userId);
            self.userDic.Remove(userId, out MatchPlayer player);
            room.playerList.Remove(player);
            room.Count = room.playerList.Count;
        }

        public static OpRetCode CanEnterRoom(this MatchRoomComponent self, int roomId, User user)
        {
            if(self.userDic.ContainsKey(user.UserId))
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
            if(user.Coin < room.Config.MinLimitCoin )
            {
                return OpRetCode.RoomNotEnoughCoin;
            }
            if(room.Config.MaxLimitCoin !=-1 && user.Coin > room.Config.MaxLimitCoin)
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

        public static MatchRoom[] GetAll(this MatchRoomComponent self, long templateId)
        {
            if (self.roomTemplatesDic.TryGetValue(templateId, out List<MatchRoom> list))
            {
                return list.ToArray();        
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
    public class MatchRoomComponentStartSystem : StartSystem<MatchRoomComponent>
    {
        public const int DEFAULT_ROOM_COUNT = 50;
        public override void Start(MatchRoomComponent self)
        {
            IConfig[] list = Game.Scene.GetComponent<ConfigComponent>().GetAll(typeof(RoomConfig));
            for (int i = 0; i < list.Length; ++i)
            {
                var cfg = list[i] as RoomConfig;
                for (var j = 0; j < DEFAULT_ROOM_COUNT; ++j)
                {
                    var room = self.CreateRoom((int)cfg.Id + j, cfg);
                    self.Add(cfg.Id, room);
                }
            }
        }
    }


}
