using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
	//脚本工具生成的代码,只包含公开成员变量,界面逻辑不要写在这里会覆盖
	[UI(UIType.ViewLogin)]
	public partial class ViewLoginComponent:ViewUIComponent
	{
		public InputField input_acc;
		public InputField input_pwd;
		public Button btn_login;
		protected override void OnCreate()
		{
			base.OnCreate();
			ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
			input_acc=rc.Get<GameObject>("input_acc").GetComponent<InputField>();
			input_pwd=rc.Get<GameObject>("input_pwd").GetComponent<InputField>();
			btn_login=rc.Get<GameObject>("btn_login").GetComponent<Button>();
		}
	}
}
