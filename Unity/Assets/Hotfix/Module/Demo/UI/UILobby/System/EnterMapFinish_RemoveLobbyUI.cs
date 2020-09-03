using ETModel;

namespace ETHotfix
{
	[Event(EventIdType.EnterMapFinish)]
	public class EnterMapFinish_RemoveLobbyUI: AEvent
	{
		public override void Run()
		{
			//Game.Scene.GetComponent<UIManagerComponent>().Remove(UIType.ViewLobby);
			//ETModel.Game.Scene.GetComponent<ResourcesComponent>().UnloadBundle(UIType.ViewLobby.StringToAB());
		}
	}
}
