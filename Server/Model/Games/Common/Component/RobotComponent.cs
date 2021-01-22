using System.Collections.Generic;

namespace ETModel
{
	public class RobotComponent: Singleton<RobotComponent>
	{
        /// <summary>
        /// 起始机器人账号数量
        /// </summary>
        public const int INIT_COUNT = 100;
        /// <summary>
        /// 可用机器人列表
        /// </summary>
        public readonly List<RobotUser> AvailableList = new List<RobotUser>();
        /// <summary>
        /// 不可用机器人列表:机器人已占用
        /// </summary>
        public readonly List<RobotUser> UnAvailableList = new List<RobotUser>();
        public readonly List<UserInfo> DispatchList = new List<UserInfo>();
        public DBProxyComponent DBProxy { get; private set; }

        public void Awake()
        {
            DBProxy = Game.Scene.GetComponent<DBProxyComponent>();
        }

        public override void Dispose()
		{
			if (this.IsDisposed)
			{
				return;
			}

			base.Dispose();
		}
	}
}