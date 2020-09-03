using ETModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ETModel
{
    /// <summary>
    /// 自定义组件类
    /// </summary>
    public class ReferenceUIComponent : Component
    {
        protected GameObject _uiGo;
        protected ReferenceCollector _rc;
        public virtual void Attach(GameObject go)
        {
            _uiGo = go;
            _rc = go.GetComponent<ReferenceCollector>();
            this.GameObject.transform.SetParent(go.transform);
        }

    }
}
