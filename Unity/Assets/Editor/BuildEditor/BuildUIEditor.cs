using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using System.IO;
using ETModel;

namespace ETEditor
{
    public class BuildUIEditor
    {
        [MenuItem("Tools/生成UI预制件脚本(ETHotfix)")]
        public static void BuildHotfixUI()
        {
            //if (Selection.gameObjects != null && Selection.gameObjects.Length > 0)
            //{
            //    Build(Selection.gameObjects,"ETHotfix");
            //}
            //else
            //{
                GameObject[] list = GetGoListAtDir(new string[] { "Assets/Bundles/UI/Hotfix" });
                Build(list, "ETHotfix");
            //}
            BuildUIType("ETHotfix");
        }

        [MenuItem("Tools/生成UI预制件脚本(ETModel)")]
        public static void BuildModelUI()
        {
            //if (Selection.gameObjects != null && Selection.gameObjects.Length > 0)
            //{
            //    Build(Selection.gameObjects,"ETModel");
            //}
            //else
            //{
                GameObject[] list = GetGoListAtDir(new string[] { "Assets/Bundles/UI/Model" });
                Build(list, "ETModel");
            //}
            BuildUIType("ETModel");
        }


        public static GameObject[] GetGoListAtDir(string[] dirs)
        {
            string[] allPath = AssetDatabase.FindAssets("t:Prefab", dirs);
            List<GameObject> list = new List<GameObject>();
            for (int i = 0; i < allPath.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(allPath[i]);
                var obj = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;
                list.Add(obj);
            }
            return list.ToArray();
        }

        public static void Build(GameObject[] list, string nameSpace)
        {
            //先清除生成目录所有脚本
            string path = GetGeneratePath(nameSpace);
            DeleteAllFiles(path);
            for (var i = 0; i < list.Length; ++i)
            {
                var go = list[i];
                if (go.name.StartsWith("View"))
                {
                    BuildViewScript(go, nameSpace);
                }
                else if (go.name.StartsWith("Window"))
                {
                    BuildWindowScript(go, nameSpace);
                }
                else if (go.name.StartsWith("Ref"))
                {
                    BuildRefScript(go, nameSpace);
                }
                else
                {
                    Debug.LogError("预制件命名不合法!");
                }
            }
            AssetDatabase.Refresh();
        }

        private static void BuildUIType(string nameSpace)
        {
            var subPath = nameSpace.Equals("ETModel") ? "Model" : "Hotfix"; ;
            GameObject[] list = GetGoListAtDir(new string[] { $"Assets/Bundles/UI/{subPath}" });
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("using System;");
            sb.AppendLine();
            sb.AppendLine($"namespace {nameSpace}");
            sb.AppendLine("{");
            sb.AppendLine("\tpublic static partial class UIType");
            sb.AppendLine("\t{");
            foreach (var item in list)
            {
                sb.AppendLine($"\t\tpublic const string {item.name} = \"{item.name}\";");
            }
            sb.AppendLine("\t}");
            sb.AppendLine("}");
            string dstPath = $"{GetGeneratePath(nameSpace)}UIType.cs";
            File.WriteAllText(dstPath, sb.ToString());
            AssetDatabase.Refresh();
        }

        private static string GetGeneratePath(string nameSpace)
        {
            var subPath = nameSpace.Equals("ETModel") ? "Model" : "Hotfix";
            string path = $"{Application.dataPath}/{subPath}/Module/UI/Generate/";
            return path;
        }

        private static void DeleteAllFiles(string path)
        {
            if (!Directory.Exists(path)) return;
            List<string> fileList = new List<string>();
            FileHelper.GetAllFiles(fileList, path);
            foreach (var item in fileList)
            {
                File.Delete(item);
            }
        }

        private static void BuildViewScript(GameObject go,string nameSpace)
        {
            string path = GetGeneratePath(nameSpace);
            string filePath = $"{path}{go.name}Component.cs";
            ReferenceCollector referenceCollector = go.GetComponent<ReferenceCollector>();
            if (referenceCollector != null)
            {
                StringBuilder strVar = new StringBuilder();
                StringBuilder strProcess = new StringBuilder();
                Walk(referenceCollector.data, strVar, strProcess);
                StringBuilder strFile = new StringBuilder();

                strFile.AppendLine("using System;");
                strFile.AppendLine("using ETModel;");
                strFile.AppendLine("using UnityEngine;");
                strFile.AppendLine("using UnityEngine.UI;");

                strFile.AppendLine();
                strFile.AppendLine($"namespace {nameSpace}");
                strFile.AppendLine("{");

                strFile.AppendLine("\t//脚本工具生成的代码,只包含公开成员变量,界面逻辑不要写在这里会覆盖");
                strFile.AppendLine($"\t[UI(UIType.{go.name})]");
                strFile.AppendLine($"\tpublic partial class {go.name}Component:ViewUIComponent");
                strFile.AppendLine("\t{");
                strFile.Append(strVar);
                strFile.AppendLine("\t\tprotected override void OnCreate()");
                strFile.AppendLine("\t\t{");
                strFile.AppendLine("\t\t\tbase.OnCreate();");
                strFile.AppendLine("\t\t\tReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();");
                strFile.Append(strProcess);
                strFile.AppendLine("\t\t}");

                strFile.AppendLine("\t}");
                strFile.AppendLine("}");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                File.WriteAllText(filePath, strFile.ToString());
                Debug.Log($"成功生成UI脚本文件: {path}");
            }
            else
            {
                Debug.LogError("无法获取Panel上的ReferenceCollector组件");
            }
        }

        private static void BuildWindowScript(GameObject go, string nameSpace)
        {
            string path = GetGeneratePath(nameSpace);
            string filePath = $"{path}{go.name}Component.cs";
            ReferenceCollector referenceCollector = go.GetComponent<ReferenceCollector>();
            if (referenceCollector != null)
            {
                StringBuilder strVar = new StringBuilder();
                StringBuilder strProcess = new StringBuilder();
                Walk(referenceCollector.data, strVar, strProcess);
                StringBuilder strFile = new StringBuilder();

                strFile.AppendLine("using System;");
                strFile.AppendLine("using ETModel;");
                strFile.AppendLine("using UnityEngine;");
                strFile.AppendLine("using UnityEngine.UI;");

                strFile.AppendLine();
                strFile.AppendLine($"namespace {nameSpace}");
                strFile.AppendLine("{");

                strFile.AppendLine("\t//脚本工具生成的代码,只包含公开成员变量,界面逻辑不要写在这里会覆盖");
                strFile.AppendLine($"\t[UI(UIType.{go.name})]");
                strFile.AppendLine($"\tpublic partial class {go.name}Component:WindowUIComponent");
                strFile.AppendLine("\t{");
                strFile.Append(strVar);
                strFile.AppendLine("\t\tprotected override void OnCreate()");
                strFile.AppendLine("\t\t{");
                strFile.AppendLine("\t\t\tbase.OnCreate();");
                strFile.AppendLine("\t\t\tReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();");
                strFile.Append(strProcess);
                strFile.AppendLine("\t\t}");

                strFile.AppendLine("\t}");
                strFile.AppendLine("}");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                File.WriteAllText(filePath, strFile.ToString());
                Debug.Log($"成功生成UI脚本文件: {path}");
            }
            else
            {
                Debug.LogError("无法获取Panel上的ReferenceCollector组件");
            }
        }

        private static void BuildRefScript(GameObject go, string nameSpace)
        {
            string path = GetGeneratePath(nameSpace);
            string filePath = $"{path}{go.name}Component.cs";
            ReferenceCollector referenceCollector = go.GetComponent<ReferenceCollector>();
            if (referenceCollector != null)
            {
                StringBuilder strVar = new StringBuilder();
                StringBuilder strProcess = new StringBuilder();
                Walk(referenceCollector.data, strVar, strProcess);
                StringBuilder strFile = new StringBuilder();

                strFile.AppendLine("using System;");
                strFile.AppendLine("using ETModel;");
                strFile.AppendLine("using UnityEngine;");
                strFile.AppendLine("using UnityEngine.UI;");

                strFile.AppendLine();
                strFile.AppendLine($"namespace {nameSpace}");
                strFile.AppendLine("{");

                strFile.AppendLine("\t//脚本工具生成的代码,只包含公开成员变量,界面逻辑不要写在这里会覆盖");
                strFile.AppendLine($"\tpublic partial class {go.name}Component:ReferenceUIComponent");
                strFile.AppendLine("\t{");
                strFile.Append(strVar);
                strFile.AppendLine("\t\tpublic override void Attach(GameObject go)");
                strFile.AppendLine("\t\t{");
                strFile.AppendLine("\t\t\tbase.Attach(go);");
                strFile.AppendLine("\t\t\tReferenceCollector rc = this._rc;");
                strFile.Append(strProcess);
                strFile.AppendLine("\t\t}");

                strFile.AppendLine("\t}");
                strFile.AppendLine("}");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                File.WriteAllText(filePath, strFile.ToString());
                Debug.Log($"成功生成UI脚本文件: {path}");
            }
            else
            {
                Debug.Log("无法获取Panel上的ReferenceCollector组件");
            }
        }

        private static void Walk(List<ReferenceCollectorData> datas,  StringBuilder strVar,  StringBuilder strProcess)
        {
            string[] builtinType = new string[] { "Button","Image", "Text", "Toggle", "InputField","Slider" };
            foreach (var data in datas)
            {
                string name = data.key;
                string type = data.type;
                if (builtinType.Contains(type))
                {
                    strVar.AppendLine($"\t\tpublic {type} {name};");
                    strProcess.AppendLine($"\t\t\t{name}=rc.Get<GameObject>(\"{name}\").GetComponent<{type}>();");
                }else if(type == "GameObject")
                {
                    strVar.AppendLine($"\t\tpublic {type} {name};");
                    strProcess.AppendLine($"\t\t\t{name}=rc.Get<GameObject>(\"{name}\");");
                }
                else
                {
                    strVar.AppendLine($"\t\tpublic {type}Component {name};");
                    strProcess.AppendLine($"\t\t\t{name}=rc.Get<GameObject>(\"{name}\").Attach<{type}Component>();");
                }
            }
        }

    }
}
