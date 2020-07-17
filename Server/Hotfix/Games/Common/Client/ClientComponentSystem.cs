using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
namespace ETHotfix
{
    [ObjectSystem]
    public class ClientComponentSystem : AwakeSystem<ClientComponent,NetworkProtocol>
    {
        public override void Awake(ClientComponent self,NetworkProtocol protocol)
        {
            self.Awake(protocol);
            self.InitMatch();   
        }
    }

    [ObjectSystem]
    public class ClientComponentStartSystem : StartSystem<ClientComponent>
    {
        public override  async void Start(ClientComponent self)
        {
            while (self.clientList.Count < ClientComponent.INIT_COUNT)
            {
                await TimerComponent.Instance.WaitAsync(2000);
            }
            self.StartMatch(100);
            //self.TestHttpRequest();
        }
    }

}
