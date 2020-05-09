﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ETModel;
namespace ETHotfix
{
    [ObjectSystem]
    public class MatchPlayerAwakeSystem : AwakeSystem<MatchPlayer, int, int, long>
    {
        public override void Awake(MatchPlayer self, int userId, int roomId, long sessionId)
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

    /// <summary>
    /// 初始化:从数据库取出所有玩家创建玩家数据,然后创建匹配房间
    /// </summary>
    [ObjectSystem]
    public class MatchRoomComponentAwakeSystem3 : AwakeSystem<MatchRoomComponent>
    {
        public override async void Awake(MatchRoomComponent self)
        {
            var dbProxy = Game.Scene.GetComponent<DBProxyComponent>();
            var list = await dbProxy.Query<UserRoom>((u) => true);
            list.ForEach((room) =>
            {
                self.AddUserRoom(room as UserRoom);
            });
            foreach (var item in self.userCreateRoomList)
            {
                var matchRoom = MatchFactory.CreateCardModeRoom(item.RoomId, item.GameId, item.GameMode,item.HallType,item.Params);
                self.AddMatchRoom(item.RoomId, matchRoom);
            }
        }
    }

    /// <summary>
    /// 初始化列表模式房间预生成
    /// </summary>
    [ObjectSystem]
    public class MatchRoomComponentStartSystem3 : StartSystem<MatchRoomComponent>
    {
        public override async void Start(MatchRoomComponent self)
        {           
            //缓存游戏配置
            await GameConfigCacheComponent.Instance.GetAllAsync();
            var configDIc = RoomConfigComponent.Instance.roomConfigDic;
            //房间列表模式:预生成房间列表
            foreach (var item in configDIc)
            {
                var cfg = item.Value;
                if (!self.IsHallOpen(cfg.Id)) continue; //如果游戏没开启就不用创建房间
                for (var j = 0; j < MatchFactory.DEFAULT_LISTMODE_COUNT; ++j)
                {
                    var room = MatchFactory.CreateListModeRoom((int)cfg.Id + j + 1, cfg);
                    self.AddListModeRoom(cfg.Id, room);
                }
            }
        }
    }

    /// <summary>
    /// 每隔一定时间刷新状态变化的房间列表
    /// </summary>
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

    /// <summary>
    /// 每隔一定时间匹配足够的玩家进入游戏房间
    /// </summary>
    [ObjectSystem]
    public class MatchRoomComponentStartSystem2 : StartSystem<MatchRoomComponent>
    {
        public const int MATCH_ROOM_INTERVAL = 2000; //毫秒

        public override async void Start(MatchRoomComponent self)
        {
            var mpList = new List<MatchPlayer>();
            var gpList = new List<GamePlayerData>();
            var userMgr = UserCacheComponent.Instance;
            while (true)
            {
                await TimerComponent.Instance.WaitAsync(MATCH_ROOM_INTERVAL);
                foreach (var item in self.matchQueueDic)
                {
                    var cfg = RoomConfigComponent.Instance.Get(item.Key);
                    var matchCount = 2;// RandomHelper.RandomNumber(cfg.MinLimitCoin, cfg.MaxLimitCoin + 1);
                    while (item.Value.Count >= matchCount)
                    {
                        //凑成一桌,分配房间
                        var room = self.GetMatchModeRoom((int)item.Key);
                        for (var i = 0; i < matchCount; ++i)
                        {
                            var p = item.Value[i];
                            mpList.Add(p);
                            self.userMatchDic.Remove(p.UserId); //移除匹配玩家对象
                        }
                        item.Value.RemoveRange(0, matchCount);//移除匹配队列玩家对象
                        //更新matchplayer的roomid字段,同时生成gameplayer
                        foreach (var mp in mpList)
                        {
                            mp.RoomId = room.RoomId;
                            var gp = GameFactory.CreatePlayerData(mp, userMgr.Get(mp.UserId));
                            gpList.Add(gp);
                        }
                        var gameSession = MatchHelper.RandomGameSession;
                        MG_MatchRoom msg = MatchFactory.CreateMsgMG_MatchRoom( room.RoomId, cfg.GameId,cfg.GameMode,cfg.HallType, gpList);
                        gameSession.Send(msg);
                        self.EnterRoom(mpList);
                        //发送消息完毕,回收gameplayer对象
                        gpList.ForEach((data) => data.Dispose());
                        gpList.Clear();
                        mpList.Clear();
                        MatchFactory.RecycleMsg(msg);
                    }
                }
            }
        }
    }
}
