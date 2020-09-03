using ETModel;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ETHotfix
{

    [ObjectSystem]
    public class UIFactoryAwakeSystem : AwakeSystem<UIFactoryComponent>
    {
        public override void Awake(UIFactoryComponent self)
        {
            self.Awake();
        }
    }

    [ObjectSystem]
    public class UIFactoryLoadAwakeSystem : LoadSystem<UIFactoryComponent>
    {
        public override void Load(UIFactoryComponent self)
        {
            throw new System.NotImplementedException();
        }
    }

    public class UIFactoryComponent : Component
    {
        private Dictionary<string, Type> _uiDic = new Dictionary<string, Type>();
        public  void Awake()
        {
            Load();
        }

        public void Load()
        {
            _uiDic.Clear();
            List<Type> list = Game.EventSystem.GetTypes();
            for (int i = 0; i < list.Count; i++)
            {
                object[] atts = list[i].GetCustomAttributes(typeof(UIAttribute), false);
                if(atts.Length == 0)
                {
                    continue;
                }
                UIAttribute att = atts[0] as UIAttribute;
                if(att != null)
                {
                    _uiDic.Add(att.Type, list[i]);
                }
            }
        }

        public  async ETTask<UI> Create(string uiType)
        {
            if(_uiDic.TryGetValue(uiType, out Type type))
            {
                var res = await ETModel.Singleton<AddressableResComponent>.Instance.LoadAssetAsync(uiType);
                var go = UnityEngine.Object.Instantiate(res) as GameObject;
                var ui = ComponentFactory.Create<UI, string, GameObject>(uiType, go);
                ui.AddComponent(type);
                return ui;
            }
            else
            {
                Log.Error($"没有注册此类型:{uiType}");
            }
            return null;
        }
    }
}