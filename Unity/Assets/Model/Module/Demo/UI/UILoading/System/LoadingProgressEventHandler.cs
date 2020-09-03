using UnityEngine;

namespace ETModel
{
    [Event(EventIdType.LoadingProgress)]
    public class LoadingProgressEvent_CreateLoadingUI : AEvent<float>
    {
        public override void Run(float progress)
        {
            UI ui = Game.Scene.GetComponent<UIManagerComponent>().Get(UIType.ViewLoading);
            if (ui == null) return;
            ui.GetComponent<UILoadingComponent>().ShowProgress(progress);
         }
    }
}
