using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETModel;
namespace ETHotfix
{

    [ObjectSystem]
    public class UserComponentSystem : AwakeSystem<UserComponent>
    {
        public override async void Awake(UserComponent self)
        {
            self.Awake();
            await self.FetchMaxUserId();
        }
    }

    [ObjectSystem]
    public class UserSystem : AwakeSystem<User,UserInfo>
    {
        public override void Awake(User self,UserInfo info)
        {
            self.Awake(info);
        }
    }

}
