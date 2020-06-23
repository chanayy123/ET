using System.Net;
using MongoDB.Bson.Serialization.Attributes;

namespace ETModel
{
	[BsonIgnoreExtraElements]
	public class InnerConfig: AConfigComponent
	{
		[BsonIgnore]
		public IPEndPoint IPEndPoint { get; private set; }
		public string Hostname { get; set; }
		public string Address { get; set; }
        public string Address2 { get; set; }

        public override void EndInit()
		{
            this.IPEndPoint = NetworkHelper.ToIPEndPoint(this.Address2);
		}
	}
}