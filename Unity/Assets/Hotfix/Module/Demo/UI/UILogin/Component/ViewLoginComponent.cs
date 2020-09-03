using System;
using System.Net;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    /// <summary>
    /// 界面逻辑处理
    /// </summary>
    public partial class ViewLoginComponent: ViewUIComponent
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            btn_login.AddListener(OnBtnClick);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            this.btn_login.RemoveListener();
        }

        protected override void OnHide()
        {
            base.OnHide();
        }

        protected override void OnResize()
        {
            base.OnResize();
        }

        protected override void OnShow()
        {
            base.OnShow();
        }

        private void OnBtnClick()
        {
            Log.Debug("点击登陆");
            LoginHelper.LoginAsync(input_acc.text);
        }

    }

}
