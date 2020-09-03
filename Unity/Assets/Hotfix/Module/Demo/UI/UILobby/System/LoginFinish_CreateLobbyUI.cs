using ETModel;

namespace ETHotfix
{
	[Event(EventIdType.LoginFinish)]
	public class LoginFinish_CreateLobbyUI: AEvent
	{
		public override void Run()
		{
            Singleton<UIManagerComponent>.Instance.Show(UIType.ViewSelect);
		}
	}
}
