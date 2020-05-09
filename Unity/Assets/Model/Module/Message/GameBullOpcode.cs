using ETModel;
namespace ETModel
{
	[Message(GameBullOpcode.BullPlayerData)]
	public partial class BullPlayerData {}

	[Message(GameBullOpcode.BullRoomData)]
	public partial class BullRoomData : IMessage {}

	[Message(GameBullOpcode.SC_BullRoomInfo)]
	public partial class SC_BullRoomInfo : IActorMessage {}

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
		 public const ushort BullPlayerData = 30001;
		 public const ushort BullRoomData = 30002;
		 public const ushort SC_BullRoomInfo = 30003;
		 public const ushort SC_BullPlayerEnter = 30004;
		 public const ushort SC_BullState = 30005;
		 public const ushort CS_BullOp = 30006;
		 public const ushort SC_BullOp = 30007;
		 public const ushort SC_BullBankerRate = 30008;
		 public const ushort SC_BullPlayerRate = 30009;
		 public const ushort SC_BullBankerPos = 30010;
		 public const ushort SC_BullCardsInfo = 30011;
		 public const ushort BullBillInfo = 30012;
		 public const ushort SC_BullBillInfo = 30013;
	}
}
