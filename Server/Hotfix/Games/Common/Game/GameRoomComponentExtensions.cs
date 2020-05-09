using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{

    public static class GameRoomComponentExtensions
    {

        public static AGameRoom CreateRoom<T>(this GameRoomComponent self, int roomId,RoomConfig cfg) where T:AGameRoom
        {
            if (self.roomsDic.ContainsKey(roomId))
            {
                Log.Warning($"创建房间: {roomId}已存在!");
                return null;
            }
            var room = ComponentFactory.Create<T, int,RoomConfig>(roomId,cfg);
            self.roomsDic.Add(roomId, room);
            return room;
        }


        public static bool AddRoom(this GameRoomComponent self, int roomId, AGameRoom room)
        {
            bool flag =self.roomsDic.TryAdd(roomId, room);
            return flag;
        }

        public static T GetRoom<T>(this GameRoomComponent self, int roomId) where T:AGameRoom
        {
            self.roomsDic.TryGetValue(roomId, out AGameRoom room);
            return (T)room;
        }

        public static AGameRoom GetRoom(this GameRoomComponent self, int roomId) 
        {
            self.roomsDic.TryGetValue(roomId, out AGameRoom room);
            return room;
        }

        public static void RemoveRoom(this GameRoomComponent self, int roomId)
        {
            self.roomsDic.Remove(roomId, out AGameRoom room);
            if (room != null)
            {
                room.Dispose();
            }
        }

    }

}
