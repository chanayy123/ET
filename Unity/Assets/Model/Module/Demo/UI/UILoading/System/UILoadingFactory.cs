using System;
using UnityEngine;

namespace ETModel
{
    public static class UILoadingFactory
    {
        public static async  ETTask<UI> Create()
        {
	        try
	        {
				//GameObject bundleGameObject = ((GameObject)ResourcesHelper.Load("KV")).Get<GameObject>(UIType.UILoading);
                GameObject prefab = await Singleton<AddressableResComponent>.Instance.LoadAssetAsync(UIType.ViewLoading) as GameObject;
				GameObject go = UnityEngine.Object.Instantiate(prefab);
				go.layer = LayerMask.NameToLayer(LayerNames.UI);
				UI ui = ComponentFactory.Create<UI, string, GameObject>(UIType.ViewLoading, go, false);

				ui.AddComponent<UILoadingComponent>();
				return ui;
	        }
	        catch (Exception e)
	        {
				Log.Error(e);
		        return null;
	        }
		}

	    public static void Remove(string type)
	    {
	    }
    }
}