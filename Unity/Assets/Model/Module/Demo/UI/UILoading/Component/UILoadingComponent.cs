using UnityEngine;
using UnityEngine.UI;

namespace ETModel
{
    [ObjectSystem]
    public class UiLoadingComponentAwakeSystem : AwakeSystem<UILoadingComponent>
    {
        public override void Awake(UILoadingComponent self)
        {
            self.text = self.GetParent<UI>().GameObject.Get<GameObject>("text").GetComponent<Text>();
            self.progressBar = self.GetParent<UI>().GameObject.Get<GameObject>("progressBar").GetComponent<Image>();
            self.OriginBarSize = self.progressBar.GetComponent<RectTransform>().sizeDelta;
        }
    }

    //[ObjectSystem]
    //public class UiLoadingComponentStartSystem : StartSystem<UILoadingComponent>
    //{
    //	public override void Start(UILoadingComponent self)
    //	{
    //		StartAsync(self).Coroutine();
    //	}

    //	public async ETVoid StartAsync(UILoadingComponent self)
    //	{
    //		TimerComponent timerComponent = Game.Scene.GetComponent<TimerComponent>();
    //		long instanceId = self.InstanceId;
    //		while (true)
    //		{
    //			await timerComponent.WaitAsync(1000);

    //			if (self.InstanceId != instanceId)
    //			{
    //				return;
    //			}

    //			BundleDownloaderComponent bundleDownloaderComponent = Game.Scene.GetComponent<BundleDownloaderComponent>();
    //			if (bundleDownloaderComponent == null)
    //			{
    //				continue;
    //			}
    //			self.text.text = $"{bundleDownloaderComponent.Progress}%";
    //		}
    //	}
    //}

    public class UILoadingComponent : Component
	{
		public Text text;
        public Image progressBar;
        public Vector2 OriginBarSize { get; set; }

        public void  ShowProgress(float value)
        {
            text.text = $"{Mathf.Floor(value * 100)}%";
            progressBar.rectTransform.sizeDelta = new Vector2(OriginBarSize.x * value, OriginBarSize.y);
        }

	}
}
