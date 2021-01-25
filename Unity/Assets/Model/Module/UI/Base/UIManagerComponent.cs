using System;
using System.Collections.Generic;
using ETModel;
using UnityEngine;

namespace ETModel
{
    /// <summary>
    /// 管理所有UI
    /// </summary>
    public class UIManagerComponent : Singleton<UIManagerComponent>
    {
        private Dictionary<string, UI> _uis = new Dictionary<string, UI>();
        private Dictionary<string, BaseUIComponent> _views = new Dictionary<string, BaseUIComponent>();
        private Dictionary<string, BaseUIComponent> _windows = new Dictionary<string, BaseUIComponent>();
        private ViewUIComponent _activeView;
        private WindowUIComponent _activeWindow;

        public async void Show(string uiType)
        {
            if (_uis.TryGetValue(uiType, out UI ui))
            {
                if (PreShow(ui)) return;
                var baseUI = ui.GetComponent<BaseUIComponent>();
                baseUI.Show();
                ui.Parent = this;
            }
            else
            {
                ui = await UIFactoryComponent.Instance.Create(uiType);
                if (PreShow(ui)) return;
                var baseUI = ui.GetComponent<BaseUIComponent>();
                baseUI.Create().Show();
                ui.Parent = this;
                AddUI(ui);
            }
            PostShow(ui);
        }

        private bool PreShow(UI ui)
        {
            var baseUI = ui.GetComponent<BaseUIComponent>();
            if (_activeView != null && baseUI is ViewUIComponent)
            {
                if (_activeView != baseUI)
                {
                    _activeView.Hide();
                    _activeView = null;
                }
                else
                {
                    Log.Warning("当前view已经在打开状态");
                    return true;
                }
            }
            else if (_activeWindow != null && baseUI is WindowUIComponent)
            {
                if (_activeWindow != baseUI)
                {
                    _activeWindow.Hide();
                    _activeWindow = null;
                }
                else
                {
                    Log.Warning("当前window已经在打开状态");
                    return true;
                }
            }
            return false;
        }

        private void PostShow(UI ui)
        {
            var baseUI = ui.GetComponent<BaseUIComponent>();
            if (baseUI is ViewUIComponent)
            {
                _activeView = baseUI as ViewUIComponent;
            }
            else if (baseUI is WindowUIComponent)
            {
                _activeWindow = baseUI as WindowUIComponent;
            }
        }

        private void AddUI(UI ui)
        {
            _uis.Add(ui.Name, ui);
            var baseUI = ui.GetComponent<BaseUIComponent>();
            if (baseUI is ViewUIComponent)
            {
                _views.Add(ui.Name, baseUI);
            }
            else if (baseUI is WindowUIComponent)
            {
                _windows.Add(ui.Name, baseUI);
            }
        }

        public void Hide(string uiType)
        {
            if (_uis.TryGetValue(uiType, out UI ui))
            {
                ui.GetComponent<BaseUIComponent>().Hide();
            }
        }

        public void Remove(string name)
        {
            if (!this._uis.TryGetValue(name, out UI ui))
            {
                return;
            }
            this._uis.Remove(name);
            ui.Dispose();
        }

        public UI Get(string name)
        {
            UI ui = null;
            this._uis.TryGetValue(name, out ui);
            return ui;
        }


        public T Get<T>(string name) where T : Component
        {
            if (_uis.TryGetValue(name, out UI ui))
            {
                return ui.GetComponent<T>();
            }
            return null;
        }
    }
}