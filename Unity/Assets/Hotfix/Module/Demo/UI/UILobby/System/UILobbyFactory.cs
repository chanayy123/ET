using System;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
    public static class UILobbyFactory
    {
        public static UI Create()
        {
	        try
	        {
				ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
		        resourcesComponent.LoadBundle(UIType.ViewLobby.StringToAB());
				GameObject bundleGameObject = (GameObject)resourcesComponent.GetAsset(UIType.ViewLobby.StringToAB(), UIType.ViewLobby);
				GameObject gameObject = UnityEngine.Object.Instantiate(bundleGameObject);
		        UI ui = ComponentFactory.Create<UI, string, GameObject>(UIType.ViewLobby, gameObject, false);

				ui.AddComponent<UILobbyComponent>();
				return ui;
	        }
	        catch (Exception e)
	        {
				Log.Error(e);
		        return null;
	        }
		}
    }
}