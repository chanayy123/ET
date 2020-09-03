using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ETHotfix
{
    public static class GameobjectUIHelper
    {
        public static T  Attach<T>(this GameObject self) where T:ReferenceUIComponent
        {
            T com = ComponentFactory.Create<T>();
            com.Attach(self);
            return com;
        }
    }
}
