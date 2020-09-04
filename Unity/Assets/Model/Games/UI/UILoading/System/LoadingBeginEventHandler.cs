using System.Collections.Generic;
using UnityEngine;

namespace ETModel
{
    [Event(EventIdType.LoadingBegin)]
    public class LoadingBeginEvent_CreateLoadingUI : AEvent<IList<object>>
    {
        public override void Run(IList<object> keys)
        {
            Singleton<UIManagerComponent>.Instance.Show(UIType.ViewLoading);
        }
    }
}
