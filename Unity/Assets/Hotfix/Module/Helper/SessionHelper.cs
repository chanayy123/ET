using ETModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ETHotfix
{
    public  class SessionHelper
    {
        public static Session Create(string address) {
            var mSession = ETModel.Game.Scene.GetComponent<NetOuterComponent>().Create(address);
            var hSession = ComponentFactory.Create<Session, ETModel.Session>(mSession);
            return hSession;
        }
    }
}
