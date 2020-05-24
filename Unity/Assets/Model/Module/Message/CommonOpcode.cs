using ETModel;
namespace ETModel
{
	[Message(CommonOpcode.CS_Ping)]
	public partial class CS_Ping : IRequest {}

	[Message(CommonOpcode.SC_Ping)]
	public partial class SC_Ping : IResponse {}

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

	[Message(CommonOpcode.UserInfo)]
	public partial class UserInfo : IMessage {}

//游戏配置:控制游戏是否开启,房间模式等
	[Message(CommonOpcode.GameConfig)]
	public partial class GameConfig : IMessage {}

	[Message(CommonOpcode.SC_PlayerData)]
	public partial class SC_PlayerData : IActorMessage {}

	[Message(CommonOpcode.CS_UserInfo)]
	public partial class CS_UserInfo : IRequest {}

	[Message(CommonOpcode.SC_UserInfo)]
	public partial class SC_UserInfo : IResponse {}

	[Message(CommonOpcode.SC_KickUser)]
	public partial class SC_KickUser : IMessage {}

	[Message(CommonOpcode.MatchRoom)]
	public partial class MatchRoom {}

	[Message(CommonOpcode.CS_RoomList)]
	public partial class CS_RoomList : IUserRequest {}

	[Message(CommonOpcode.SC_RoomList)]
	public partial class SC_RoomList : IResponse {}

	[Message(CommonOpcode.CS_CreateRoomList)]
	public partial class CS_CreateRoomList : IUserRequest {}

	[Message(CommonOpcode.SC_CreateRoomList)]
	public partial class SC_CreateRoomList : IResponse {}

	[Message(CommonOpcode.CS_EnterRoom)]
	public partial class CS_EnterRoom : IUserRequest {}

	[Message(CommonOpcode.SC_EnterRoom)]
	public partial class SC_EnterRoom : IResponse {}

	[Message(CommonOpcode.CS_LeaveRoom)]
	public partial class CS_LeaveRoom : IActorRequest {}

	[Message(CommonOpcode.SC_LeaveRoom)]
	public partial class SC_LeaveRoom : IActorResponse {}

	[Message(CommonOpcode.CS_CreateRoom)]
	public partial class CS_CreateRoom : IUserRequest {}

	[Message(CommonOpcode.SC_CreateRoom)]
	public partial class SC_CreateRoom : IResponse {}

	[Message(CommonOpcode.CS_DisbandRoom)]
	public partial class CS_DisbandRoom : IUserRequest {}

	[Message(CommonOpcode.SC_DisbandRoom)]
	public partial class SC_DisbandRoom : IResponse {}

	[Message(CommonOpcode.GamePlayerData)]
	public partial class GamePlayerData : IMessage {}

	[Message(CommonOpcode.GameRoomData)]
	public partial class GameRoomData : IMessage {}

	[Message(CommonOpcode.SC_GameRoomInfo)]
	public partial class SC_GameRoomInfo : IActorMessage {}

	[Message(CommonOpcode.SC_PlayerLeave)]
	public partial class SC_PlayerLeave : IActorMessage {}

	[Message(CommonOpcode.SC_RoomListChanged)]
	public partial class SC_RoomListChanged : IActorMessage {}

	[Message(CommonOpcode.CS_EnterScene)]
	public partial class CS_EnterScene : IUserRequest {}

	[Message(CommonOpcode.SC_EnterScene)]
	public partial class SC_EnterScene : IResponse {}

	[Message(CommonOpcode.CS_LeaveScene)]
	public partial class CS_LeaveScene : IUserRequest {}

	[Message(CommonOpcode.SC_LeaveScene)]
	public partial class SC_LeaveScene : IResponse {}

	[Message(CommonOpcode.SC_CoinChange)]
	public partial class SC_CoinChange : IActorMessage {}

	[Message(CommonOpcode.CS_GetRoomInfo)]
	public partial class CS_GetRoomInfo : IActorRequest {}

}
namespace ETModel
{
	public static partial class CommonOpcode
	{
		 public const ushort CS_Ping = 20001;
		 public const ushort SC_Ping = 20002;
		 public const ushort CS_Register = 20003;
		 public const ushort SC_Register = 20004;
		 public const ushort CS_Login = 20005;
		 public const ushort SC_Login = 20006;
		 public const ushort CS_VerifyKey = 20007;
		 public const ushort SC_VerifyKey = 20008;
		 public const ushort UserInfo = 20009;
		 public const ushort GameConfig = 20010;
		 public const ushort SC_PlayerData = 20011;
		 public const ushort CS_UserInfo = 20012;
		 public const ushort SC_UserInfo = 20013;
		 public const ushort SC_KickUser = 20014;
		 public const ushort MatchRoom = 20015;
		 public const ushort CS_RoomList = 20016;
		 public const ushort SC_RoomList = 20017;
		 public const ushort CS_CreateRoomList = 20018;
		 public const ushort SC_CreateRoomList = 20019;
		 public const ushort CS_EnterRoom = 20020;
		 public const ushort SC_EnterRoom = 20021;
		 public const ushort CS_LeaveRoom = 20022;
		 public const ushort SC_LeaveRoom = 20023;
		 public const ushort CS_CreateRoom = 20024;
		 public const ushort SC_CreateRoom = 20025;
		 public const ushort CS_DisbandRoom = 20026;
		 public const ushort SC_DisbandRoom = 20027;
		 public const ushort GamePlayerData = 20028;
		 public const ushort GameRoomData = 20029;
		 public const ushort SC_GameRoomInfo = 20030;
		 public const ushort SC_PlayerLeave = 20031;
		 public const ushort SC_RoomListChanged = 20032;
		 public const ushort CS_EnterScene = 20033;
		 public const ushort SC_EnterScene = 20034;
		 public const ushort CS_LeaveScene = 20035;
		 public const ushort SC_LeaveScene = 20036;
		 public const ushort SC_CoinChange = 20037;
		 public const ushort CS_GetRoomInfo = 20038;
	}
}
