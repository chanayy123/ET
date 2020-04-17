﻿using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
namespace ETHotfix
{
    public static class BullClassicPlayerExtensions
    {
        public static void Awake(this BullClassicPlayer self,GamePlayerData data)
        {
            self.PlayerData = data;
            self.PlayerData.Parent = self;
        }
    }

    [ObjectSystem]
    public class BullClassicPlayerAwakeSystem : AwakeSystem<BullClassicPlayer, GamePlayerData>
    {
        public override void Awake(BullClassicPlayer self, GamePlayerData data)
        {
            self.Awake(data);
        }
    }





}
