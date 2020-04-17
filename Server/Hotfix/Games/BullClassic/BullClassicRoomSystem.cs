using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
namespace ETHotfix
{
    public static class BullClassicRoomExtensions
    {
        public static void Awake(this BullClassicRoom self, GameRoomData data)
        {
            self.RoomData = data;
        }
        public static void Awake(this BullClassicRoom self, int roomId)
        {
            self.RoomData = BullClassicFactory.CreateRoomData(roomId, 0);
        }

        public static void EnterRoom(this BullClassicRoom self, BullClassicPlayer player)
        {
            self.AddPlayer(player);
            self.ChangeState(RoomState.WAIT);
            //广播进房间消息
            self.BroadcastPlayerEnter(player);
        }

        public static void LeaveRoom(this BullClassicRoom self, int userId)
        {
            self.RemovePlayer(userId);
            var state = self.playerDic.Keys.Count == 0 ? RoomState.IDLE : RoomState.WAIT;
            self.ChangeState(state);
            self.BroadcastPlayerLeave(userId);
        }

        public static void AddPlayer(this BullClassicRoom self, BullClassicPlayer player)
        {
            player.Parent = self; //方便通过玩家对象获取房间对象
            var playerData = player.PlayerData;
            self.RoomData.PlayerList.Add(playerData);
            self.playerDic.Add(playerData.UserId, player);
            //同步玩家actorId方便以后通信
            player.AddComponent<MailBoxComponent>();
            GameHelper.SynActorId(player.PlayerData.GateSessionId, player.PlayerData.UserId, player.InstanceId);
        }

        public static void RemovePlayer(this BullClassicRoom self, int userId)
        {
            self.playerDic.Remove(userId, out BullClassicPlayer player);
            self.RoomData.PlayerList.Remove(player?.PlayerData);
            player?.Dispose();
        }

        private static void ChangeState(this BullClassicRoom self, RoomState state)
        {
            var rs = (int)state;
            if (self.RoomData.State != rs)
            {
                self.RoomData.State = rs;
                GameHelper.SynRoomData(self.RoomData.RoomId, rs,self.InstanceId);
            }
        }

        public static void BroadcastPlayerEnter(this BullClassicRoom self, BullClassicPlayer player)
        {
            foreach (var item in self.playerDic)
            {
                if (item.Key != player.PlayerData.UserId)
                {
                    NetInnerHelper.SendActorMsg(item.Value.PlayerData.GateSessionId, new SC_PlayerEnter()
                    {
                        Player = player.PlayerData
                    });
                }
                else
                {
                    NetInnerHelper.SendActorMsg(player.PlayerData.GateSessionId, new SC_GameRoomInfo()
                    {
                        RoomInfo = self.RoomData
                    });
                }
            }
        }

        public static void BroadcastPlayerLeave(this BullClassicRoom self, int userId)
        {
            foreach (var item in self.playerDic)
            {
                if (item.Key != userId)
                {
                    NetInnerHelper.SendActorMsg(item.Value.PlayerData.GateSessionId, new SC_PlayerLeave()
                    {
                        UserId = userId
                    });

                }
            }

        }

        [ObjectSystem]
        public class BullClassicRoomAwakeSystem : AwakeSystem<BullClassicRoom, GameRoomData>
        {
            public override void Awake(BullClassicRoom self, GameRoomData data)
            {
                self.Awake(data);
            }
        }
        [ObjectSystem]
        public class BullClassicRoomAwakeSystem2 : AwakeSystem<BullClassicRoom, int>
        {
            public override void Awake(BullClassicRoom self, int roomId)
            {
                self.Awake(roomId);
            }
        }




    }
}
