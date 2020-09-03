using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETModel
{
	//脚本工具生成的代码,只包含公开成员变量,界面逻辑不要写在这里会覆盖
	[UI(UIType.ViewLoading)]
	public partial class ViewLoadingComponent:ViewUIComponent
	{
		public Image imgProgress;
		public Text txt;
		protected override void OnCreate()
		{
			base.OnCreate();
			ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
			imgProgress=rc.Get<GameObject>("imgProgress").GetComponent<Image>();
			txt=rc.Get<GameObject>("txt").GetComponent<Text>();
		}
	}
}
