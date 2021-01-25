using ETModel;

namespace ETHotfix
{
	[Event(EventIdType.InitSceneStart)]
	public class InitSceneStartHandler : AEvent
	{
		public override void Run()
		{
            ETModel.UIManagerComponent.Instance.Hide(ETModel.UIType.ViewLoading);
            Singleton<UIManagerComponent>.Instance.Show(UIType.ViewLogin);
        }
	}
}
