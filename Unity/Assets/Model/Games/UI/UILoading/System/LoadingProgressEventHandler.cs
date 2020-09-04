using UnityEngine;

namespace ETModel
{
    [Event(EventIdType.LoadingProgress)]
    public class LoadingProgressEvent_CreateLoadingUI : AEvent<float>
    {
        public override void Run(float progress)
        {
            var loading = Singleton<UIManagerComponent>.Instance.Get<ViewLoadingComponent>(UIType.ViewLoading);
            loading?.ShowProgress(progress);
        }
    }
}
