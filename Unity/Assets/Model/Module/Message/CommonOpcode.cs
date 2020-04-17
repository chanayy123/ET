using ETModel;
namespace ETModel
{
	[Message(CommonOpcode.CS_Register)]
	public partial class CS_Register : IRequest {}

	[Message(CommonOpcode.SC_Register)]
	public partial class SC_Register : IResponse {}

	[Message(CommonOpcode.CS_Login)]
	public partial class CS_Login : IRequest {}

	[Message(CommonOpcode.SC_Login)]
	public partial class SC_Login : IResponse {}

	[Message(CommonOpcode.CS_VerifyKey)]
	public partial class CS_VerifyKey : IRequest {}

	[Message(CommonOpcode.SC_VerifyKey)]
	public partial class SC_VerifyKey : IResponse {}

	[Message(CommonOpcode.User)]
	public partial class User : IMessage {}

	[Message(CommonOpcode.CS_UserInfo)]
	public partial class CS_UserInfo : IRequest {}

	[Message(CommonOpcode.SC_UserInfo)]
	public partial class SC_UserInfo : IResponse {}

	[Message(CommonOpcode.SC_KickUser)]
	public partial class SC_KickUser : IMessage {}

	[Message(CommonOpcode.MatchRoom)]
	public partial class MatchRoom {}

	[Message(CommonOpcode.CS_RoomList)]
	public partial class CS_RoomList : IRequest {}

	[Message(CommonOpcode.SC_RoomList)]
	public partial class SC_RoomList : IResponse {}

	[Message(CommonOpcode.CS_EnterRoom)]
	public partial class CS_EnterRoom : IUserRequest {}

	[Message(CommonOpcode.SC_EnterRoom)]
	public partial class SC_EnterRoom : IResponse {}

	[Message(CommonOpcode.CS_LeaveRoom)]
	public partial class CS_LeaveRoom : IUserRequest {}

	[Message(CommonOpcode.SC_LeaveRoom)]
	public partial class SC_LeaveRoom : IResponse {}

	[Message(CommonOpcode.GamePlayerData)]
	public partial class GamePlayerData : IMessage {}

	[Message(CommonOpcode.GameRoomData)]
	public partial class GameRoomData : IMessage {}

	[Message(CommonOpcode.SC_GameRoomInfo)]
	public partial class SC_GameRoomInfo : IActorMessage {}

	[Message(CommonOpcode.SC_PlayerEnter)]
	public partial class SC_PlayerEnter : IActorMessage {}

	[Message(CommonOpcode.SC_PlayerLeave)]
	public partial class SC_PlayerLeave : IActorMessage {}

}
namespace ETModel
{
	public static partial class CommonOpcode
	{
		 public const ushort CS_Register = 20001;
		 public const ushort SC_Register = 20002;
		 public const ushort CS_Login = 20003;
		 public const ushort SC_Login = 20004;
		 public const ushort CS_VerifyKey = 20005;
		 public const ushort SC_VerifyKey = 20006;
		 public const ushort User = 20007;
		 public const ushort CS_UserInfo = 20008;
		 public const ushort SC_UserInfo = 20009;
		 public const ushort SC_KickUser = 20010;
		 public const ushort MatchRoom = 20011;
		 public const ushort CS_RoomList = 20012;
		 public const ushort SC_RoomList = 20013;
		 public const ushort CS_EnterRoom = 20014;
		 public const ushort SC_EnterRoom = 20015;
		 public const ushort CS_LeaveRoom = 20016;
		 public const ushort SC_LeaveRoom = 20017;
		 public const ushort GamePlayerData = 20018;
		 public const ushort GameRoomData = 20019;
		 public const ushort SC_GameRoomInfo = 20020;
		 public const ushort SC_PlayerEnter = 20021;
		 public const ushort SC_PlayerLeave = 20022;
	}
}
