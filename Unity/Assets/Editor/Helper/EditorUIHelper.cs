using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace ETEditor
{
    public class EditorUIHelper
    {
        /// <summary>
        /// 根据名字获取对应类型字符串
        /// UI起名规则: 按钮:btn 图片:img 文本:txt  输入框:input 切换开关按钮: toggle  滑块: slider
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string NameToUIType(string name)
        {
            string type="";
            if (name.StartsWith("btn"))
            {
                type = "Button";
            }
            else if (name.StartsWith("txt"))
            {
                type = "Text";
            }
            else if (name.StartsWith("img"))
            {
                type = "Image";
            }
            else if (name.StartsWith("go"))
            {
                type = "GameObject";
            }
            else if (name.StartsWith("toggle"))
            {
                type = "Toggle";
            }
            else if (name.StartsWith("input"))
            {
                type = "InputField";
            }
            else if (name.StartsWith("slider"))
            {
                type = "Slider";
            }
            return type;
        }

        public static string ObjectToUIType(Object obj)
        {
            if (PrefabUtility.GetPrefabAssetType(obj) == PrefabAssetType.Regular)
            {
                var prefab = PrefabUtility.GetCorrespondingObjectFromSource(obj);
                return prefab?.name;
            }
            return "";
        }



    }
}
