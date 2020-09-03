using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
	//脚本工具生成的代码,只包含公开成员变量,界面逻辑不要写在这里会覆盖
	[UI(UIType.ViewSelect)]
	public partial class ViewSelectComponent:ViewUIComponent
	{
		public RefSelectLevelComponent ItemSelect0;
		public RefSelectLevelComponent ItemSelect1;
		public RefSelectLevelComponent ItemSelect2;
		public RefPlayerInfoComponent playerInfo;
		protected override void OnCreate()
		{
			base.OnCreate();
			ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
			ItemSelect0=rc.Get<GameObject>("ItemSelect0").Attach<RefSelectLevelComponent>();
			ItemSelect1=rc.Get<GameObject>("ItemSelect1").Attach<RefSelectLevelComponent>();
			ItemSelect2=rc.Get<GameObject>("ItemSelect2").Attach<RefSelectLevelComponent>();
			playerInfo=rc.Get<GameObject>("playerInfo").Attach<RefPlayerInfoComponent>();
		}
	}
}
