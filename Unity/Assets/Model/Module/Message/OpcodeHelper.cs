using System.Collections.Generic;

namespace ETModel
{
	public static class OpcodeHelper
	{
        //与工具生成范围保持一致
        public  const ushort INNER_MSG_START = 1000;
        public  const ushort OUTER_MSG_START = 20000;
		private static readonly HashSet<ushort> ignoreDebugLogMessageSet = new HashSet<ushort>
		{
			OuterOpcode.C2R_Ping,
			OuterOpcode.R2C_Ping,
		};

		public static bool IsNeedDebugLogMessage(ushort opcode)
		{
			if (ignoreDebugLogMessageSet.Contains(opcode))
			{
				return false;
			}

			return true;
		}

        public static bool IsInnerMessage(ushort opcode)
        {
            return opcode > INNER_MSG_START && opcode < OUTER_MSG_START;
        }

        public static bool IsOuterMessage(ushort opcode)
        {
            return opcode > OUTER_MSG_START;
        }

		//public static bool IsClientHotfixMessage(ushort opcode)
		//{
		//	return opcode > 10000;
		//}
	}
}