using ETModel;
namespace ETModel
{
	[Message(GameBullOpcode.BullFightPlayerData)]
	public partial class BullFightPlayerData {}

	[Message(GameBullOpcode.BullFightRoomData)]
	public partial class BullFightRoomData : IMessage {}

	[Message(GameBullOpcode.SC_BullRoomInfo)]
	public partial class SC_BullRoomInfo : IActorMessage {}

//断线重连,客户端主动请求当前游戏房间信息
	[Message(GameBullOpcode.SC_GetBullRoomInfo)]
	public partial class SC_GetBullRoomInfo : IActorResponse {}

	[Message(GameBullOpcode.SC_BullPlayerEnter)]
	public partial class SC_BullPlayerEnter : IActorMessage {}

	[Message(GameBullOpcode.SC_BullState)]
	public partial class SC_BullState : IActorMessage {}

	[Message(GameBullOpcode.CS_BullOp)]
	public partial class CS_BullOp : IActorRequest {}

	[Message(GameBullOpcode.SC_BullOp)]
	public partial class SC_BullOp : IActorResponse {}

	[Message(GameBullOpcode.SC_BullBankerRate)]
	public partial class SC_BullBankerRate : IActorMessage {}

	[Message(GameBullOpcode.SC_BullPlayerRate)]
	public partial class SC_BullPlayerRate : IActorMessage {}

	[Message(GameBullOpcode.SC_BullBankerPos)]
	public partial class SC_BullBankerPos : IActorMessage {}

	[Message(GameBullOpcode.SC_BullCardsInfo)]
	public partial class SC_BullCardsInfo : IActorMessage {}

	[Message(GameBullOpcode.BullBillInfo)]
	public partial class BullBillInfo {}

	[Message(GameBullOpcode.SC_BullBillInfo)]
	public partial class SC_BullBillInfo : IActorMessage {}

}
namespace ETModel
{
	public static partial class GameBullOpcode
	{
		 public const ushort BullFightPlayerData = 30001;
		 public const ushort BullFightRoomData = 30002;
		 public const ushort SC_BullRoomInfo = 30003;
		 public const ushort SC_GetBullRoomInfo = 30004;
		 public const ushort SC_BullPlayerEnter = 30005;
		 public const ushort SC_BullState = 30006;
		 public const ushort CS_BullOp = 30007;
		 public const ushort SC_BullOp = 30008;
		 public const ushort SC_BullBankerRate = 30009;
		 public const ushort SC_BullPlayerRate = 30010;
		 public const ushort SC_BullBankerPos = 30011;
		 public const ushort SC_BullCardsInfo = 30012;
		 public const ushort BullBillInfo = 30013;
		 public const ushort SC_BullBillInfo = 30014;
	}
}
