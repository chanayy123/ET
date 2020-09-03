using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETModel
{
    public partial class ViewLoadingComponent
    {
        public Vector2 OriginBarSize { get; set; }

        protected override void OnDisable()
        {
            base.OnDisable();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
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
            this.ShowProgress(0);
        }

        public void ShowProgress(float value)
        {

        }
    }
}
