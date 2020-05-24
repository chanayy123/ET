using System;
using System.Collections.Generic;
using System.Text;
using ETHotfix;
namespace ETModel
{
    public class SessionGateUserComponent: Component
    {
        public GateUser User { get; set; }

        public override void Dispose()
        {
            if (this.IsDisposed) return;
            base.Dispose();
            User = null;
        }
    }
}
