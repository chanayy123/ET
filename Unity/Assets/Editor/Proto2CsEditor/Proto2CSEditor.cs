using System.Diagnostics;
using ETModel;
using UnityEditor;
using UnityEngine;

namespace ETEditor
{
	internal class OpcodeInfo
	{
		public string Name;
		public int Opcode;
	}

	public class Proto2CSEditor: EditorWindow
	{
		[MenuItem("Tools/Proto2CS")]
		public static void AllProto2CS()
		{
            //第三个参数路径:unity项目根目录为参考目录
            Process process = ProcessHelper.Run("dotnet", "Proto2CS.dll", "../Proto/ProtoTool/", true);
            Log.Info(process.StandardOutput.ReadToEnd());
            AssetDatabase.Refresh();
		}
	}
}
