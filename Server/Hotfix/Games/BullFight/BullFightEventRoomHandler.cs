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
    [Event(EventType.GameRoomCreate+BullEventType.Bull_Game)]
    public class BullFightCreateHandler : AEvent<int,RoomConfig>
    {
        public override void Run(int roomId,RoomConfig cfg)
        {
            var roomMgr = Game.Scene.GetComponent<GameRoomComponent>();
            var room =  roomMgr.CreateRoom<BullFightRoom>(roomId,cfg);
            roomMgr.AddRoom(roomId, room);
            GameHelper.SynRoomData(roomId, (int)RoomState.IDLE, room.InstanceId);
        }
    }

    [Event(EventType.GameRoomEnter + BullEventType.Bull_Game)]
    public class BullFightJoinHandler : AEvent<int,GamePlayerData>
    {
        public override void Run(int roomId, GamePlayerData playerData )
        {
            var roomMgr = Game.Scene.GetComponent<GameRoomComponent>();
            var player = BullFightFactory.CreatePlayer(playerData);
            var room = roomMgr.GetRoom<BullFightRoom>(roomId);
            room.EnterRoom(player);
        }
    }

    [Event(EventType.GameRoomMatch + BullEventType.Bull_Game)]
    public class BullFightMatchHandler : AEvent<int, List<GamePlayerData>>
    {
        private readonly List<BullFightPlayer> tempList = new List<BullFightPlayer>();
        public override void Run(int roomId, List<GamePlayerData> playerList)
        {
            var roomMgr = Game.Scene.GetComponent<GameRoomComponent>();
            var room = roomMgr.GetRoom<BullFightRoom>(roomId);
            tempList.Clear();
            foreach (var item in playerList)
            {
                var player = BullFightFactory.CreatePlayer(item);
                tempList.Add(player);
            }
            room.EnterRoom(tempList);
        }
    }

}
