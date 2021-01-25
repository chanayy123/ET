namespace ETModel
{
	[ObjectSystem]
	public class GlobalConfigComponentAwakeSystem : AwakeSystem<GlobalConfigComponent>
	{
		public override void Awake(GlobalConfigComponent t)
		{
			t.Awake();
		}
	}

    public class GlobalConfigComponent : Singleton<GlobalConfigComponent>
    {
        public GlobalConfig GlobalConfig;
        public  void Awake()
        {
            GlobalConfig = ConfigComponent.Instance.GetOne(typeof(GlobalConfig)) as GlobalConfig;
        }
    }

    //public class GlobalConfigComponent : Component
    //{
    //    public static GlobalConfigComponent Instance;
    //    public GlobalConfig GlobalConfig;

    //    public void Awake()
    //    {
    //        Instance = this;
    //        GlobalConfig = Game.Scene.GetComponent<ConfigComponent>().GetOne(typeof(GlobalConfig)) as GlobalConfig;
    //    }

    //}
}