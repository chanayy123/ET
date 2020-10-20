using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using ETModel;
namespace ETHotfix
{
    [ObjectSystem]
    public class ClientComponentSystem : AwakeSystem<ClientComponent,NetworkProtocol>
    {
        public override void Awake(ClientComponent self,NetworkProtocol protocol)
        {
            self.Awake(protocol);
        }
    }

    [ObjectSystem]
    public class ClientComponentStartSystem : StartSystem<ClientComponent>
    {
        public override  async void Start(ClientComponent self)
        {
            await TimerComponent.Instance.WaitAsync(2000);
            self.TestLoginMatch(1);
            //self.TestHttpRequest(100);
        }
    }

}
