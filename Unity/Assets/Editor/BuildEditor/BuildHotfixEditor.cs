using System;
using System.IO;
using ETModel;
using UnityEditor;

namespace ETEditor
{
    [InitializeOnLoad]
    public class Startup
    {
        private const string ScriptAssembliesDir = "Library/ScriptAssemblies";
        private const string CodeDir = "Assets/Res/Code/";
        private const string HotfixDll = "Unity.Hotfix.dll";
        private const string HotfixPdb = "Unity.Hotfix.pdb";

        static Startup()
        {
            File.Copy(Path.Combine(ScriptAssembliesDir, HotfixDll), Path.Combine(CodeDir, "Hotfix.dll.bytes"), true);
            File.Copy(Path.Combine(ScriptAssembliesDir, HotfixPdb), Path.Combine(CodeDir, "Hotfix.pdb.bytes"), true);
            Log.Info($"复制Hotfix.dll, Hotfix.pdb到Res/Code完成");
            //调用刷新总是报错
            //NullReferenceException: Object reference not set to an instance of an object
            //UnityEditor.GameObjectInspector.ClearPreviewCache()(at<d0ffe769b7a34b4cac3a7cdc5c696293>:0)
            //UnityEditor.GameObjectInspector.OnDisable()(at<d0ffe769b7a34b4cac3a7cdc5c696293>:0)
            //UnityEditor.AssetDatabase:Refresh()
            //ETEditor.Startup:.cctor()(at Assets / Editor / BuildEditor / BuildHotfixEditor.cs:21)
            //UnityEditor.EditorAssemblies:ProcessInitializeOnLoadAttributes(Type[])
            //AssetDatabase.Refresh ();
        }
    }
}