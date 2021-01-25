using System.Collections.Generic;
using UnityEngine;

namespace ETModel
{
    [Event(EventIdType.LoadingFinish)]
    public class LoadingFinishEvent_RemoveLoadingUI : AEvent<IList<object>>
    {
        public override void Run(IList<object> keys)
        {
            var loading = UIManagerComponent.Instance.Get<ViewLoadingComponent>(UIType.ViewLoading);
            loading?.ShowProgress(1);
        }
    }
}
