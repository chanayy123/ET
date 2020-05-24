using System;
using System.Collections.Generic;
using System.Text;

namespace ETModel
{
    /// <summary>
    /// 游戏公共事件类型前缀
    /// </summary>
    public static class EventType
    {
        public const string GameRoomCreate = "GameRoomCreate";
        public const string GameRoomEnter = "GameRoomEnter";
        public const string GameRoomMatch = "GameRoomMatch";
        public const string GameRoomStateChange = "GameRoomStateChange";
        public const string PropertyChange = "PropertyChange";
    }
}
