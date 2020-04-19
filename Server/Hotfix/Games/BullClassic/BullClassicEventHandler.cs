using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
namespace ETHotfix
{
    /// <summary>
    /// 创建经典牛牛游戏房间
    /// 1是经典牛牛游戏id
    /// </summary>
    [Event(EventType.GameRoomCreate+"1")]
    public class BullClassicCreateHandler : AEvent<int>
    {
        public override void Run(int roomId)
        {
            var roomMgr = Game.Scene.GetComponent<GameRoomComponent>();
            var room =  roomMgr.CreateRoom<BullClassicRoom>(roomId);
            roomMgr.AddRoom(roomId, room);
            GameHelper.SynRoomData(roomId, 0, room.InstanceId);
        }
    }

    [Event(EventType.GameRoomEnter + "1")]
    public class BullClassicJoinHandler : AEvent<int,GamePlayerData>
    {
        public override void Run(int roomId, GamePlayerData playerData )
        {
            var roomMgr = Game.Scene.GetComponent<GameRoomComponent>();
            var player = BullClassicFactory.CreatePlayer(playerData);
            var room = roomMgr.GetRoom<BullClassicRoom>(roomId);
            room.EnterRoom(player);
        }
    }

    //[Event(EventType.GameRoomLeave+ "1")]
    //public class BullClassicLeaveHandler : AEvent<int,int>
    //{
    //    public override void Run(int roomId,int userId)
    //    {
    //        var roomMgr = Game.Scene.GetComponent<GameRoomComponent>();
    //        var room = roomMgr.GetRoom<BullClassicRoom>(roomId);
    //        room.LeaveRoom(userId);
    //    }
    //}
}
