using System;
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


    [ObjectSystem]
    public class MatchRoomComponentAwakeSystem : AwakeSystem<MatchRoomComponent>
    {
        public override void Awake(MatchRoomComponent self)
        {
            IConfig[] list = Game.Scene.GetComponent<ConfigComponent>().GetAll(typeof(RoomConfig));
            //缓存房间配置
            foreach (var item in list)
            {
                self.roomConfigDic.Add(item.Id, item as RoomConfig);
            }
            //房间列表模式:预生成房间列表
            for (int i = 0; i < list.Length; ++i)
            {
                var cfg = list[i] as RoomConfig;
                for (var j = 0; j < MatchFactory.DEFAULT_LISTMODE_COUNT; ++j)
                {
                    var room = MatchFactory.CreateListModeRoom((int)cfg.Id + j, cfg);
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
                    var cfg = self.roomConfigDic[item.Key];
                    var matchCount = 2;// RandomHelper.RandomNumber(cfg.MinLimitCoin, cfg.MaxLimitCoin + 1);
                    while (item.Value.Count >= matchCount)
                    {
                        mpList.Clear();
                        gpList.Clear();
                        for (var i = 0; i < matchCount; ++i)
                        {
                            var p = item.Value[i];
                            mpList.Add(p);
                        }
                        item.Value.RemoveRange(0, matchCount);
                        //凑成一桌,分配房间
                        var room = self.GetMatchModeRoom((int)item.Key);
                        //更新matchplayer的roomid字段,同时生成gameplayer
                        foreach (var mp in mpList)
                        {
                            mp.RoomId = room.RoomId;
                            self.userMatchDic.Remove(mp.UserId); //移除匹配队列玩家对象
                            var gp = GameFactory.CreatePlayerData(mp, userMgr.Get(mp.UserId));
                            gpList.Add(gp);
                        }
                        var gameSession = MatchHelper.RandomGameSession;
                        gameSession.Send(new MG_MatchRoom()
                        {
                            GameId = cfg.GameId,
                            PlayerList = gpList,
                            RoomId = room.RoomId
                        });
                        //发送消息完毕,回收gameplayer对象
                        gpList.ForEach((data) => data.Dispose());
                        self.EnterRoom(mpList);
                    }
                }
            }
        }
    }
}
