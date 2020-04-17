using System;
using System.Collections.Generic;
using System.Text;
using ETHotfix;
namespace ETModel
{
    /// <summary>
    /// 游戏服房间管理组件
    /// </summary>
    public class GameRoomComponent : Component
    {
        /// <summary>
        /// 房间id : 房间对象
        /// </summary>
        public readonly Dictionary<int, AGameRoom> roomsDic = new Dictionary<int, AGameRoom>();

    }
}
