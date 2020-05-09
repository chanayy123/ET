using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
namespace ETHotfix
{
    public static class BullClassicRoomExtensions
    {
        public static void Awake(this BullClassicRoom self, int roomId, RoomConfig cfg)
        {
            self.RoomData = BullClassicFactory.CreateRoomData(roomId,cfg);
            self.Cfg = cfg;
        }

        public static void EnterRoom(this BullClassicRoom self, List<BullClassicPlayer> playerList)
        {
            foreach (var item in playerList)
            {
                self.AddPlayer(item);
            }
            //广播进房间消息
            self.BroadcastRoomInfo();
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
            GameHelper.SynLeaveRoom(self.RoomId, userId);
        }

        public static OpRetCode CanLeaveRoom(this BullClassicRoom self, int userId)
        {
            if(self.State != (int)RoomState.GAMING)
            {
                return OpRetCode.Success;
            }
            else
            {
                return OpRetCode.RoomAlreadyGaming;
            }

        }

        public static void AddPlayer(this BullClassicRoom self, BullClassicPlayer player)
        {
            player.Parent = self; //方便通过玩家对象获取房间对象
            self.RoomData.PlayerList.Add(player.PlayerData);
            self.playerDic.Add(player.UserId, player);
            //同步玩家actorId方便以后通信
            player.AddComponent<MailBoxComponent>();
            GameHelper.SynActorId(player.GateSessionId, player.UserId, player.InstanceId,(int)GameId.BullClassic,self.RoomId);
        }

        public static void RemovePlayer(this BullClassicRoom self, int userId)
        {
            self.playerDic.Remove(userId, out BullClassicPlayer player);
            if(player == null)
            {
                Log.Warning($"删除用户失败: 用户{userId}不存在");
                return;
            }
            self.RoomData.PlayerList.Remove(player.PlayerData);
            //离开游戏房间重置玩家actorid
            GameHelper.SynActorId(player.GateSessionId, player.UserId, 0);
            player.Dispose();
        }

        private static void ChangeState(this BullClassicRoom self, RoomState state)
        {
            var rs = (int)state;
            if (self.State != rs)
            {
                self.State = rs;
                GameHelper.SynRoomData(self.RoomId, rs,self.InstanceId);
            }
        }

        public static void BroadcastRoomInfo(this BullClassicRoom self)
        {
            foreach (var item in self.playerDic)
            {
                var bullPlayer = item.Value;
                SC_BullRoomInfo msg = BullClassicFactory.CreateMsgSC_BullRoomInfo(bullPlayer.GateSessionId, self.RoomData);
                NetInnerHelper.SendActorMsg(msg);
                BullClassicFactory.RecycleMsg(msg);
            }
        }

        public static void BroadcastPlayerEnter(this BullClassicRoom self, BullClassicPlayer player)
        {
            foreach (var item in self.playerDic)
            {
                if (item.Key != player.UserId)
                {
                    var bullPlayer = item.Value;
                    SC_BullPlayerEnter msg = BullClassicFactory.CreateMsgSC_BullPlayerEnter(bullPlayer.GateSessionId, player.PlayerData);
                    NetInnerHelper.SendActorMsg(msg);
                    BullClassicFactory.RecycleMsg(msg);
                }
                else
                {
                    SC_BullRoomInfo msg = BullClassicFactory.CreateMsgSC_BullRoomInfo(player.GateSessionId, self.RoomData);
                    NetInnerHelper.SendActorMsg(msg);
                    BullClassicFactory.RecycleMsg(msg);
                }
            }
        }

        public static void BroadcastPlayerLeave(this BullClassicRoom self, int userId)
        {
            foreach (var item in self.playerDic)
            {
                if (item.Key != userId)
                {
                    var bullPlayer = item.Value;
                    SC_PlayerLeave msg = GameFactory.CreateMsgSC_PlayerLeave(bullPlayer.GateSessionId, userId, 0);
                    NetInnerHelper.SendActorMsg(msg);
                    GameFactory.RecycleMsg(msg);
                }
            }

        }
    }
}
