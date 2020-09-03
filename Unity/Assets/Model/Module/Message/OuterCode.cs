using ETModel;
namespace ETModel
{
	[Message(OuterCode.CS_Ping)]
	public partial class CS_Ping : IRequest {}

	[Message(OuterCode.SC_Ping)]
	public partial class SC_Ping : IResponse {}

	[Message(OuterCode.C2M_Reload)]
	public partial class C2M_Reload : IRequest {}

	[Message(OuterCode.M2C_Reload)]
	public partial class M2C_Reload : IResponse {}

}
namespace ETModel
{
	public static partial class OuterCode
	{
		 public const ushort CS_Ping = 10001;
		 public const ushort SC_Ping = 10002;
		 public const ushort C2M_Reload = 10003;
		 public const ushort M2C_Reload = 10004;
	}
}
