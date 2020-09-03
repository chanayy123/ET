using ETModel;
namespace ETModel
{
	[Message(CommonDataCode.GamePlayerData)]
	public partial class GamePlayerData : IMessage {}

	[Message(CommonDataCode.GameRoomData)]
	public partial class GameRoomData : IMessage {}

	[Message(CommonDataCode.UserInfo)]
	public partial class UserInfo : IMessage {}

//游戏配置:控制游戏是否开启,房间模式等
	[Message(CommonDataCode.GameConfig)]
	public partial class GameConfig : IMessage {}

	[Message(CommonDataCode.MatchRoom)]
	public partial class MatchRoom {}

}
namespace ETModel
{
	public static partial class CommonDataCode
	{
		 public const ushort GamePlayerData = 40001;
		 public const ushort GameRoomData = 40002;
		 public const ushort UserInfo = 40003;
		 public const ushort GameConfig = 40004;
		 public const ushort MatchRoom = 40005;
	}
}
