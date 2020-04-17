namespace ETModel
{
	[Config((int)( AppType.Match))]
	public partial class RoomConfigCategory : ACategory<RoomConfig>
	{
	}

	public class RoomConfig: IConfig
	{
		public long Id { get; set; }
		public string Name;
		public string Desc;
		public int GameId;
		public int MinLimitCoin;
		public int MaxLimitCoin;
		public int MaxPlayers;
	}
}
