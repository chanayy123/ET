using ETModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ETHotfix
{
    public class ViewUIComponent : BaseUIComponent
    {
        /// <summary>
        /// 对象构造回调
        /// </summary>
        protected override void OnCreate()
        {
            var canvas = this.GetParent<UI>().GameObject.GetComponent<Canvas>();
            canvas.sortingLayerName = ETModel.SortingLayer.VIEW;
            canvas.worldCamera = Global.GetComponent<ReferenceCollector>().Get<GameObject>("UICamera").GetComponent<Camera>();
        }
        /// <summary>
        /// 对象启动回调
        /// </summary>
        protected override void OnEnable()
        {

        }

        /// <summary>
        /// 对象禁用回调
        /// </summary>
        protected override void OnDisable()
        {

        }

        /// <summary>
        /// 对象显示回调
        /// </summary>
        protected override void OnShow()
        {

        }
        /// <summary>
        /// 对象隐藏回调
        /// </summary>
        protected override void OnHide()
        {

        }
        /// <summary>
        /// 分辨率变更回调
        /// </summary>
        protected override void OnResize()
        {

        }

    }
}
