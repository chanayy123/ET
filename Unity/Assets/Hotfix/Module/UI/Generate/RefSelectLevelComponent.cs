using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
	//脚本工具生成的代码,只包含公开成员变量,界面逻辑不要写在这里会覆盖
	public partial class RefSelectLevelComponent:ReferenceUIComponent
	{
		public Text txtLevel;
		public Button btnEnter;
		public override void Attach(GameObject go)
		{
			base.Attach(go);
			ReferenceCollector rc = this._rc;
			txtLevel=rc.Get<GameObject>("txtLevel").GetComponent<Text>();
			btnEnter=rc.Get<GameObject>("btnEnter").GetComponent<Button>();
		}
	}
}
