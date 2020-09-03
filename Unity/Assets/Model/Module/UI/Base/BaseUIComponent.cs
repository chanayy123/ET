using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETModel
{

    public class SortingLayer
    {
        public static string VIEW = "View";
        public static string WINDOW = "Window";
    }

    public class BaseUIComponent : Component
    {
        /// <summary>
        /// 对象构造回调
        /// </summary>
        protected virtual void OnCreate()
        {

        }
        /// <summary>
        /// 对象启用回调
        /// </summary>
        protected virtual void OnEnable()
        {

        }

        /// <summary>
        /// 对象禁用回调
        /// </summary>
        protected virtual void OnDisable()
        {

        }

        /// <summary>
        /// 对象显示回调
        /// </summary>
        protected virtual void OnShow()
        {

        }
        /// <summary>
        /// 对象隐藏回调
        /// </summary>
        protected virtual void OnHide()
        {

        }
        /// <summary>
        /// 分辨率变更回调
        /// </summary>
        protected virtual void OnResize()
        {

        }

        public void Show()
        {
            OnEnable();
            OnShow();
        }

        public void Hide()
        {
            OnDisable();
            OnHide();
        }

        public BaseUIComponent Create()
        {
            OnCreate();
            return this;
        }

    }
}
