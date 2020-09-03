namespace ETModel
{
	public static partial class InnerOpcode
	{
		 public const ushort M2A_Reload = 1001;
		 public const ushort A2M_Reload = 1002;
		 public const ushort G2G_LockRequest = 1003;
		 public const ushort G2G_LockResponse = 1004;
		 public const ushort G2G_LockReleaseRequest = 1005;
		 public const ushort G2G_LockReleaseResponse = 1006;
		 public const ushort DBSaveRequest = 1007;
		 public const ushort DBSaveBatchResponse = 1008;
		 public const ushort DBSaveBatchRequest = 1009;
		 public const ushort DBSaveResponse = 1010;
		 public const ushort DBQueryRequest = 1011;
		 public const ushort DBQueryResponse = 1012;
		 public const ushort DBQueryBatchRequest = 1013;
		 public const ushort DBQueryBatchResponse = 1014;
		 public const ushort DBQueryJsonRequest = 1015;
		 public const ushort DBDeleteJsonRequest = 1016;
		 public const ushort DBSortQueryJsonRequest = 1017;
		 public const ushort DBQueryJsonResponse = 1018;
		 public const ushort DBDeleteJsonResponse = 1019;
		 public const ushort ObjectAddRequest = 1020;
		 public const ushort ObjectAddResponse = 1021;
		 public const ushort ObjectRemoveRequest = 1022;
		 public const ushort ObjectRemoveResponse = 1023;
		 public const ushort ObjectLockRequest = 1024;
		 public const ushort ObjectLockResponse = 1025;
		 public const ushort ObjectUnLockRequest = 1026;
		 public const ushort ObjectUnLockResponse = 1027;
		 public const ushort ObjectGetRequest = 1028;
		 public const ushort ObjectGetResponse = 1029;
		 public const ushort R2G_GetLoginKey = 1030;
		 public const ushort G2R_GetLoginKey = 1031;
		 public const ushort GS_Online = 1032;
		 public const ushort GS_Offline = 1033;
		 public const ushort SG_KickUser = 1034;
		 public const ushort GS_KickUser = 1035;
		 public const ushort MG_EnterRoom = 1036;
		 public const ushort GM_LeaveRoom = 1037;
		 public const ushort MG_MatchRoom = 1038;
		 public const ushort GM_SynRoomData = 1039;
		 public const ushort Actor_OnlineState = 1040;
		 public const ushort GS_SynActorId = 1041;
		 public const ushort RW_Login = 1042;
		 public const ushort WR_Login = 1043;
		 public const ushort RW_Register = 1044;
		 public const ushort WR_Register = 1045;
		 public const ushort SW_GetUserInfo = 1046;
		 public const ushort WS_GetUserInfo = 1047;
		 public const ushort SW_GetGameCfgList = 1048;
		 public const ushort WS_GetGameCfgList = 1049;
		 public const ushort SW_UpdateUserInfo = 1050;
		 public const ushort WS_UpdateUserInfo = 1051;
		 public const ushort WS_UserInfoChanged = 1052;
		 public const ushort SW_LockMaxUserId = 1053;
		 public const ushort WS_LockMaxUserId = 1054;
		 public const ushort SW_UnlockMaxUserId = 1055;
		 public const ushort MR_CallRobot = 1056;
		 public const ushort RM_CallRobot = 1057;
		 public const ushort MR_ReturnRobot = 1058;
		 public const ushort SR_UpdateCoin = 1059;
	}
}
