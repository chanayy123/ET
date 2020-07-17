namespace ETModel
{
	[Config((int)( AppType.Match|AppType.Game|AppType.World|AppType.ClientH))]
	public partial class RoomConfigCategory : ACategory<RoomConfig>
	{
	}

	public class RoomConfig: IConfig
	{
		public long Id { get; set; }
		public string Name;
		public string Desc;
		public int GameId;
		public int GameMode;
		public int HallType;
		public int BaseScore;
		public int[] IntParams;
		public int MinLimitCoin;
		public int MaxLimitCoin;
		public int MinPlayers;
		public int MaxPlayers;
	}
}
