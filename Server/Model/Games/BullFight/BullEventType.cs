using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETModel
{
    //enum BullGameState
    //{
    //    Bull_GS_IDLE = 0;
    //    Bull_GS_WAIT_PLAYER = 1;
    //    Bull_GS_WAIT_START = 2;
    //    Bull_GS_ROBBANK = 3;
    //    Bull_GS_SELBANK = 4;
    //    Bull_GS_PLAYERBET = 5;
    //    Bull_GS_DISPATCH = 6;
    //    Bull_GS_SHOWCARD = 7;
    //    Bull_GS_BILL = 8;
    //}
    /// <summary>
    /// 游戏状态改变事件格式:  id_mode_state
    /// </summary>
    public static class BullEventType 
    {
        public const string Bull_Game = "1";
        public const string Bull_GS_WAIT_PLAYER = "1_1";
        public const string Bull_GS_WAIT_START = "1_2";
        public const string Bull_GS_ROBBANK = "1_3";
        public const string Bull_GS_SELBANK = "1_4";
        public const string Bull_GS_PLAYERBET = "1_5";
        public const string Bull_GS_DISPATCH = "1_6";
        public const string Bull_GS_SHOWCARD = "1_7";
        public const string Bull_GS_BILL = "1_8";
    }
}
