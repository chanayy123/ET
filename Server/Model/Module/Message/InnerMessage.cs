using ETModel;
using System.Collections.Generic;
namespace ETModel
{
/// <summary>
/// 传送unit
/// </summary>
	[Message(InnerOpcode.M2M_TrasferUnitRequest)]
	public partial class M2M_TrasferUnitRequest: IRequest
	{
		public int RpcId { get; set; }

		public Unit Unit { get; set; }

	}

	[Message(InnerOpcode.M2M_TrasferUnitResponse)]
	public partial class M2M_TrasferUnitResponse: IResponse
	{
		public int RpcId { get; set; }

		public int Error { get; set; }

		public string Message { get; set; }

		public long InstanceId { get; set; }

	}

	[Message(InnerOpcode.M2A_Reload)]
	public partial class M2A_Reload: IRequest
	{
		public int RpcId { get; set; }

	}

	[Message(InnerOpcode.A2M_Reload)]
	public partial class A2M_Reload: IResponse
	{
		public int RpcId { get; set; }

		public int Error { get; set; }

		public string Message { get; set; }

	}

	[Message(InnerOpcode.G2G_LockRequest)]
	public partial class G2G_LockRequest: IRequest
	{
		public int RpcId { get; set; }

		public long Id { get; set; }

		public string Address { get; set; }

	}

	[Message(InnerOpcode.G2G_LockResponse)]
	public partial class G2G_LockResponse: IResponse
	{
		public int RpcId { get; set; }

		public int Error { get; set; }

		public string Message { get; set; }

	}

	[Message(InnerOpcode.G2G_LockReleaseRequest)]
	public partial class G2G_LockReleaseRequest: IRequest
	{
		public int RpcId { get; set; }

		public long Id { get; set; }

		public string Address { get; set; }

	}

	[Message(InnerOpcode.G2G_LockReleaseResponse)]
	public partial class G2G_LockReleaseResponse: IResponse
	{
		public int RpcId { get; set; }

		public int Error { get; set; }

		public string Message { get; set; }

	}

	[Message(InnerOpcode.DBSaveRequest)]
	public partial class DBSaveRequest: IRequest
	{
		public int RpcId { get; set; }

		public string CollectionName { get; set; }

		public ComponentWithId Component { get; set; }

	}

	[Message(InnerOpcode.DBSaveBatchResponse)]
	public partial class DBSaveBatchResponse: IResponse
	{
		public int RpcId { get; set; }

		public int Error { get; set; }

		public string Message { get; set; }

	}

	[Message(InnerOpcode.DBSaveBatchRequest)]
	public partial class DBSaveBatchRequest: IRequest
	{
		public int RpcId { get; set; }

		public string CollectionName { get; set; }

		public List<ComponentWithId> Components = new List<ComponentWithId>();

	}

	[Message(InnerOpcode.DBSaveResponse)]
	public partial class DBSaveResponse: IResponse
	{
		public int RpcId { get; set; }

		public int Error { get; set; }

		public string Message { get; set; }

	}

	[Message(InnerOpcode.DBQueryRequest)]
	public partial class DBQueryRequest: IRequest
	{
		public int RpcId { get; set; }

		public long Id { get; set; }

		public string CollectionName { get; set; }

	}

	[Message(InnerOpcode.DBQueryResponse)]
	public partial class DBQueryResponse: IResponse
	{
		public int RpcId { get; set; }

		public int Error { get; set; }

		public string Message { get; set; }

		public ComponentWithId Component { get; set; }

	}

	[Message(InnerOpcode.DBQueryBatchRequest)]
	public partial class DBQueryBatchRequest: IRequest
	{
		public int RpcId { get; set; }

		public string CollectionName { get; set; }

		public List<long> IdList = new List<long>();

	}

	[Message(InnerOpcode.DBQueryBatchResponse)]
	public partial class DBQueryBatchResponse: IResponse
	{
		public int RpcId { get; set; }

		public int Error { get; set; }

		public string Message { get; set; }

		public List<ComponentWithId> Components = new List<ComponentWithId>();

	}

	[Message(InnerOpcode.DBQueryJsonRequest)]
	public partial class DBQueryJsonRequest: IRequest
	{
		public int RpcId { get; set; }

		public string CollectionName { get; set; }

		public string Json { get; set; }

	}

	[Message(InnerOpcode.DBSortQueryJsonRequest)]
	public partial class DBSortQueryJsonRequest: IRequest
	{
		public int RpcId { get; set; }

		public string CollectionName { get; set; }

		public string QueryJson { get; set; }

		public string SortJson { get; set; }

		public int Count { get; set; }

	}

	[Message(InnerOpcode.DBQueryJsonResponse)]
	public partial class DBQueryJsonResponse: IResponse
	{
		public int RpcId { get; set; }

		public int Error { get; set; }

		public string Message { get; set; }

		public List<ComponentWithId> Components = new List<ComponentWithId>();

	}

	[Message(InnerOpcode.ObjectAddRequest)]
	public partial class ObjectAddRequest: IRequest
	{
		public int RpcId { get; set; }

		public long Key { get; set; }

		public long InstanceId { get; set; }

	}

	[Message(InnerOpcode.ObjectAddResponse)]
	public partial class ObjectAddResponse: IResponse
	{
		public int RpcId { get; set; }

		public int Error { get; set; }

		public string Message { get; set; }

	}

	[Message(InnerOpcode.ObjectRemoveRequest)]
	public partial class ObjectRemoveRequest: IRequest
	{
		public int RpcId { get; set; }

		public long Key { get; set; }

	}

	[Message(InnerOpcode.ObjectRemoveResponse)]
	public partial class ObjectRemoveResponse: IResponse
	{
		public int RpcId { get; set; }

		public int Error { get; set; }

		public string Message { get; set; }

	}

	[Message(InnerOpcode.ObjectLockRequest)]
	public partial class ObjectLockRequest: IRequest
	{
		public int RpcId { get; set; }

		public long Key { get; set; }

		public long InstanceId { get; set; }

		public int Time { get; set; }

	}

	[Message(InnerOpcode.ObjectLockResponse)]
	public partial class ObjectLockResponse: IResponse
	{
		public int RpcId { get; set; }

		public int Error { get; set; }

		public string Message { get; set; }

	}

	[Message(InnerOpcode.ObjectUnLockRequest)]
	public partial class ObjectUnLockRequest: IRequest
	{
		public int RpcId { get; set; }

		public long Key { get; set; }

		public long OldInstanceId { get; set; }

		public long InstanceId { get; set; }

	}

	[Message(InnerOpcode.ObjectUnLockResponse)]
	public partial class ObjectUnLockResponse: IResponse
	{
		public int RpcId { get; set; }

		public int Error { get; set; }

		public string Message { get; set; }

	}

	[Message(InnerOpcode.ObjectGetRequest)]
	public partial class ObjectGetRequest: IRequest
	{
		public int RpcId { get; set; }

		public long Key { get; set; }

	}

	[Message(InnerOpcode.ObjectGetResponse)]
	public partial class ObjectGetResponse: IResponse
	{
		public int RpcId { get; set; }

		public int Error { get; set; }

		public string Message { get; set; }

		public long InstanceId { get; set; }

	}

	[Message(InnerOpcode.R2G_GetLoginKey)]
	public partial class R2G_GetLoginKey: IRequest
	{
		public int RpcId { get; set; }

		public int UserId { get; set; }

	}

	[Message(InnerOpcode.G2R_GetLoginKey)]
	public partial class G2R_GetLoginKey: IResponse
	{
		public int RpcId { get; set; }

		public int Error { get; set; }

		public string Message { get; set; }

		public string Key { get; set; }

	}

	[Message(InnerOpcode.G2M_CreateUnit)]
	public partial class G2M_CreateUnit: IRequest
	{
		public int RpcId { get; set; }

		public long PlayerId { get; set; }

		public long GateSessionId { get; set; }

	}

	[Message(InnerOpcode.M2G_CreateUnit)]
	public partial class M2G_CreateUnit: IResponse
	{
		public int RpcId { get; set; }

		public int Error { get; set; }

		public string Message { get; set; }

// 自己的unit id
// 自己的unit id
		public long UnitId { get; set; }

// 所有的unit
// 所有的unit
		public List<UnitInfo> Units = new List<UnitInfo>();

	}

	[Message(InnerOpcode.G2M_SessionDisconnect)]
	public partial class G2M_SessionDisconnect: IActorLocationMessage
	{
		public int RpcId { get; set; }

		public long ActorId { get; set; }

	}

// gate->other server
	[Message(InnerOpcode.GS_Online)]
	public partial class GS_Online: IMessage
	{
		public int UserId { get; set; }

		public long GateSessionId { get; set; }

	}

//gate->other server
	[Message(InnerOpcode.GS_Offline)]
	public partial class GS_Offline: IMessage
	{
		public int UserId { get; set; }

	}

//other server->gate
	[Message(InnerOpcode.SG_KickUser)]
	public partial class SG_KickUser: IRequest
	{
		public int RpcId { get; set; }

		public int UserId { get; set; }

	}

	[Message(InnerOpcode.GS_KickUser)]
	public partial class GS_KickUser: IResponse
	{
		public int RpcId { get; set; }

		public int Error { get; set; }

		public string Message { get; set; }

	}

//match->game
	[Message(InnerOpcode.MG_EnterRoom)]
	public partial class MG_EnterRoom: IMessage
	{
		public int RoomId { get; set; }

		public int GameId { get; set; }

		public int GameMode { get; set; }

		public int HallType { get; set; }

		public int RoomType { get; set; }

		public GamePlayerData Player { get; set; }

	}

//game->match
	[Message(InnerOpcode.GM_LeaveRoom)]
	public partial class GM_LeaveRoom: IMessage
	{
		public int RoomId { get; set; }

		public List<int> UserIdList = new List<int>();

	}

//match->game
	[Message(InnerOpcode.MG_MatchRoom)]
	public partial class MG_MatchRoom: IMessage
	{
		public int RoomId { get; set; }

		public int GameId { get; set; }

		public int GameMode { get; set; }

		public int HallType { get; set; }

		public int RoomType { get; set; }

		public List<GamePlayerData> PlayerList = new List<GamePlayerData>();

	}

//game->match
	[Message(InnerOpcode.GM_SynRoomData)]
	public partial class GM_SynRoomData: IMessage
	{
		public int RoomId { get; set; }

		public int State { get; set; }

		public long RoomActorId { get; set; }

	}

//gate->game
	[Message(InnerOpcode.Actor_OnlineState)]
	public partial class Actor_OnlineState: IActorMessage
	{
		public long ActorId { get; set; }

		public long GateSessionId { get; set; }

		public bool Flag { get; set; }

	}

//game->other
	[Message(InnerOpcode.GS_SynActorId)]
	public partial class GS_SynActorId: IMessage
	{
		public int UserId { get; set; }

		public int GameId { get; set; }

		public int RoomId { get; set; }

		public long ActorId { get; set; }

	}

//realm->world
	[Message(InnerOpcode.RW_Login)]
	public partial class RW_Login: IRequest
	{
		public int RpcId { get; set; }

		public int LoginType { get; set; }

		public int PlatformType { get; set; }

		public string DataStr { get; set; }

	}

//world->realm
	[Message(InnerOpcode.WR_Login)]
	public partial class WR_Login: IResponse
	{
		public int RpcId { get; set; }

		public int Error { get; set; }

		public string Message { get; set; }

		public int UserId { get; set; }

	}

//realm->world
	[Message(InnerOpcode.RW_Register)]
	public partial class RW_Register: IRequest
	{
		public int RpcId { get; set; }

		public string Account { get; set; }

		public string Password { get; set; }

		public string Name { get; set; }

	}

//world->realm
	[Message(InnerOpcode.WR_Register)]
	public partial class WR_Register: IResponse
	{
		public int RpcId { get; set; }

		public int Error { get; set; }

		public int UserId { get; set; }

		public string Message { get; set; }

	}

//other server->world
	[Message(InnerOpcode.SW_GetUserInfo)]
	public partial class SW_GetUserInfo: IRequest
	{
		public int RpcId { get; set; }

		public int UserId { get; set; }

	}

//world -> other server
	[Message(InnerOpcode.WS_GetUserInfo)]
	public partial class WS_GetUserInfo: IResponse
	{
		public int RpcId { get; set; }

		public int Error { get; set; }

		public string Message { get; set; }

		public User UserInfo { get; set; }

	}

	[Message(InnerOpcode.SW_GetGameCfgList)]
	public partial class SW_GetGameCfgList: IRequest
	{
		public int RpcId { get; set; }

	}

	[Message(InnerOpcode.WS_GetGameCfgList)]
	public partial class WS_GetGameCfgList: IResponse
	{
		public int RpcId { get; set; }

		public int Error { get; set; }

		public string Message { get; set; }

		public List<GameConfig> GameCfgList = new List<GameConfig>();

	}

	[Message(InnerOpcode.SW_UpdateUserInfo)]
	public partial class SW_UpdateUserInfo: IRequest
	{
		public int RpcId { get; set; }

		public int UserId { get; set; }

		public string Key { get; set; }

		public string Value { get; set; }

	}

	[Message(InnerOpcode.WS_UpdateUserInfo)]
	public partial class WS_UpdateUserInfo: IResponse
	{
		public int RpcId { get; set; }

		public int Error { get; set; }

		public string Message { get; set; }

	}

}
