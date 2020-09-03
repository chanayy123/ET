using System.Collections.Generic;
using UnityEngine;

namespace ETModel
{
    [Event(EventIdType.LoadingFinish)]
    public class LoadingFinishEvent_RemoveLoadingUI : AEvent<IList<object>>
    {
        public override void Run(IList<object> keys)
        {
            UI ui = Game.Scene.GetComponent<UIManagerComponent>().Get(UIType.ViewLoading);
            if (ui == null) return;
            ui.GetComponent<UILoadingComponent>().ShowProgress(1);
        }
    }
}
