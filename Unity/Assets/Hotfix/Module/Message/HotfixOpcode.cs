using ETModel;
namespace ETHotfix
{
	[Message(HotfixOpcode.CS_Register)]
	public partial class CS_Register : IRequest {}

	[Message(HotfixOpcode.SC_Register)]
	public partial class SC_Register : IResponse {}

	[Message(HotfixOpcode.CS_Login)]
	public partial class CS_Login : IRequest {}

	[Message(HotfixOpcode.SC_Login)]
	public partial class SC_Login : IResponse {}

	[Message(HotfixOpcode.CS_VerifyKey)]
	public partial class CS_VerifyKey : IRequest {}

	[Message(HotfixOpcode.SC_VerifyKey)]
	public partial class SC_VerifyKey : IResponse {}

	[Message(HotfixOpcode.SC_PlayerData)]
	public partial class SC_PlayerData : IActorMessage {}

	[Message(HotfixOpcode.CS_UserInfo)]
	public partial class CS_UserInfo : IRequest {}

	[Message(HotfixOpcode.SC_UserInfo)]
	public partial class SC_UserInfo : IResponse {}

	[Message(HotfixOpcode.SC_KickUser)]
	public partial class SC_KickUser : IMessage {}

	[Message(HotfixOpcode.CS_RoomList)]
	public partial class CS_RoomList : IUserRequest {}

	[Message(HotfixOpcode.SC_RoomList)]
	public partial class SC_RoomList : IResponse {}

	[Message(HotfixOpcode.CS_CreateRoomList)]
	public partial class CS_CreateRoomList : IUserRequest {}

	[Message(HotfixOpcode.SC_CreateRoomList)]
	public partial class SC_CreateRoomList : IResponse {}

	[Message(HotfixOpcode.CS_EnterRoom)]
	public partial class CS_EnterRoom : IUserRequest {}

	[Message(HotfixOpcode.SC_EnterRoom)]
	public partial class SC_EnterRoom : IResponse {}

	[Message(HotfixOpcode.CS_LeaveRoom)]
	public partial class CS_LeaveRoom : IActorRequest {}

	[Message(HotfixOpcode.SC_LeaveRoom)]
	public partial class SC_LeaveRoom : IActorResponse {}

	[Message(HotfixOpcode.CS_CreateRoom)]
	public partial class CS_CreateRoom : IUserRequest {}

	[Message(HotfixOpcode.SC_CreateRoom)]
	public partial class SC_CreateRoom : IResponse {}

	[Message(HotfixOpcode.CS_DisbandRoom)]
	public partial class CS_DisbandRoom : IUserRequest {}

	[Message(HotfixOpcode.SC_DisbandRoom)]
	public partial class SC_DisbandRoom : IResponse {}

	[Message(HotfixOpcode.SC_GameRoomInfo)]
	public partial class SC_GameRoomInfo : IActorMessage {}

	[Message(HotfixOpcode.SC_PlayerLeave)]
	public partial class SC_PlayerLeave : IActorMessage {}

//服务器主动踢玩家出房间
	[Message(HotfixOpcode.SC_KickPlayer)]
	public partial class SC_KickPlayer : IActorMessage {}

	[Message(HotfixOpcode.SC_RoomListChanged)]
	public partial class SC_RoomListChanged : IActorMessage {}

	[Message(HotfixOpcode.CS_EnterScene)]
	public partial class CS_EnterScene : IUserRequest {}

	[Message(HotfixOpcode.SC_EnterScene)]
	public partial class SC_EnterScene : IResponse {}

	[Message(HotfixOpcode.CS_LeaveScene)]
	public partial class CS_LeaveScene : IUserRequest {}

	[Message(HotfixOpcode.SC_LeaveScene)]
	public partial class SC_LeaveScene : IResponse {}

	[Message(HotfixOpcode.SC_CoinChange)]
	public partial class SC_CoinChange : IActorMessage {}

	[Message(HotfixOpcode.CS_GetRoomInfo)]
	public partial class CS_GetRoomInfo : IActorRequest {}

}
namespace ETHotfix
{
	public static partial class HotfixOpcode
	{
		 public const ushort CS_Register = 20001;
		 public const ushort SC_Register = 20002;
		 public const ushort CS_Login = 20003;
		 public const ushort SC_Login = 20004;
		 public const ushort CS_VerifyKey = 20005;
		 public const ushort SC_VerifyKey = 20006;
		 public const ushort SC_PlayerData = 20007;
		 public const ushort CS_UserInfo = 20008;
		 public const ushort SC_UserInfo = 20009;
		 public const ushort SC_KickUser = 20010;
		 public const ushort CS_RoomList = 20011;
		 public const ushort SC_RoomList = 20012;
		 public const ushort CS_CreateRoomList = 20013;
		 public const ushort SC_CreateRoomList = 20014;
		 public const ushort CS_EnterRoom = 20015;
		 public const ushort SC_EnterRoom = 20016;
		 public const ushort CS_LeaveRoom = 20017;
		 public const ushort SC_LeaveRoom = 20018;
		 public const ushort CS_CreateRoom = 20019;
		 public const ushort SC_CreateRoom = 20020;
		 public const ushort CS_DisbandRoom = 20021;
		 public const ushort SC_DisbandRoom = 20022;
		 public const ushort SC_GameRoomInfo = 20023;
		 public const ushort SC_PlayerLeave = 20024;
		 public const ushort SC_KickPlayer = 20025;
		 public const ushort SC_RoomListChanged = 20026;
		 public const ushort CS_EnterScene = 20027;
		 public const ushort SC_EnterScene = 20028;
		 public const ushort CS_LeaveScene = 20029;
		 public const ushort SC_LeaveScene = 20030;
		 public const ushort SC_CoinChange = 20031;
		 public const ushort CS_GetRoomInfo = 20032;
	}
}
