using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
	//脚本工具生成的代码,只包含公开成员变量,界面逻辑不要写在这里会覆盖
	public partial class RefPlayerInfoComponent:ReferenceUIComponent
	{
		public Image imgHead;
		public Text txtName;
		public Text txtId;
		public Text txtGold;
		public override void Attach(GameObject go)
		{
			base.Attach(go);
			ReferenceCollector rc = this._rc;
			imgHead=rc.Get<GameObject>("imgHead").GetComponent<Image>();
			txtName=rc.Get<GameObject>("txtName").GetComponent<Text>();
			txtId=rc.Get<GameObject>("txtId").GetComponent<Text>();
			txtGold=rc.Get<GameObject>("txtGold").GetComponent<Text>();
		}
	}
}
