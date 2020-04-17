namespace ETModel
{
	public interface IMessage
	{
	}
	
	public interface IRequest: IMessage
	{
		int RpcId { get; set; }
	}
    /// <summary>
    /// 玩家上线之后请求消息接口:须带userid和网关sessionid
    /// </summary>
    public interface IUserRequest : IRequest
    {
        int UserId { get; set; }
        long GateSessionId { get; set; }
    }

    public interface IResponse : IMessage
	{
		int Error { get; set; }
		string Message { get; set; }
		int RpcId { get; set; }
	}

    public class ErrorResponse : IResponse
	{
		public int Error { get; set; }
		public string Message { get; set; }
		public int RpcId { get; set; }
	}
}