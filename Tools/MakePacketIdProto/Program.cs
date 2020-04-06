
using ETModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace MakePacketIdProto
{
    class Program
    {
        static void Main(string[] args)
        {
            LoadDll();
            MakePacketIdProto();
        }

        private static void LoadDll()
        {
            Game.EventSystem.Add(DLLType.Model, typeof(Game).Assembly);
            Game.EventSystem.Add(DLLType.Hotfix, GetHotfixAssembly());
        }

        private static Assembly GetHotfixAssembly()
        {
            byte[] dllBytes = File.ReadAllBytes("./Hotfix.dll");
            byte[] pdbBytes = File.ReadAllBytes("./Hotfix.pdb");
            Assembly assembly = Assembly.Load(dllBytes, pdbBytes);
            return assembly;
        }

        private static Assembly GetModelAssembly()
        {
            byte[] dllBytes = File.ReadAllBytes("./Model.dll");
            byte[] pdbBytes = File.ReadAllBytes("./Model.pdb");
            Assembly assembly = Assembly.Load(dllBytes, pdbBytes);
            return assembly;
        }

        private static void MakePacketIdProto()
        {
            string ns = "ETModel";
            string en = "PacketId";
            string output = $"../{en}.proto";
            StringBuilder sb = new StringBuilder();
            sb.Append("syntax = \"proto3\";\n");
            sb.Append($"package {ns};\n");
            sb.Append($"enum {en} \n{{\n");
            List<Type> types = Game.EventSystem.GetTypes(typeof(MessageAttribute));
            SortedDictionary<ushort, string> st = new SortedDictionary<ushort, string>();
            foreach (Type type in types)
            {
                object[] objects = type.GetCustomAttributes(typeof(BaseAttribute), false);
                if (objects.Length == 0)
                {
                    continue;
                }
                MessageAttribute att = (MessageAttribute)objects[0];
                var opcode = att.Opcode;
                var name = type.FullName.Split(".")[1];
                st.Add(opcode, name);
            }
            foreach (var pair in st)
            {
                sb.Append($"\t{pair.Value} = {pair.Key}; \n");
            }
            sb.Append("}\n");

            File.WriteAllText(output, sb.ToString());
            Console.WriteLine("packeid.proto生成完成!");
            Console.ReadKey();
        }
    }
}
