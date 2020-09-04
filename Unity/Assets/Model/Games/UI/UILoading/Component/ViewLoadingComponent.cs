using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETModel
{
    public partial class ViewLoadingComponent: ViewUIComponent
    {
        public Vector2 OriginBarSize { get; set; }

        protected override void OnDisable()
        {
            base.OnDisable();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            OriginBarSize = imgProgress.GetComponent<RectTransform>().sizeDelta;
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
            txt.text = $"{Mathf.Floor(value * 100)}%";
            imgProgress.rectTransform.sizeDelta = new Vector2(OriginBarSize.x * value, OriginBarSize.y);
        }
    }
}
