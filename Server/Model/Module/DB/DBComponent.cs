using System;
using System.Collections.Generic;
using System.Net;
using MongoDB.Driver;

namespace ETModel
{
	[ObjectSystem]
	public class DbComponentSystem : AwakeSystem<DBComponent>
	{
		public override void Awake(DBComponent self)
		{
			self.Awake();
		}
	}

	/// <summary>
	/// 用来缓存数据
	/// </summary>
	public class DBComponent : Component
	{
		public MongoClient mongoClient;
		public IMongoDatabase database;

        public const int taskCount = 32;
		public List<DBTaskQueue> tasks = new List<DBTaskQueue>(taskCount);

		public void Awake()
		{
            var config = StartConfigComponent.Instance.StartConfig.GetComponent<DBConfig>();
            var option = Game.Scene.GetComponent<OptionComponent>().Options;
            string connectionString = this.GetConnectionString(config,option);
			mongoClient = new MongoClient(connectionString);
			this.database = this.mongoClient.GetDatabase(config.DBName);
			Log.Debug($"mongo dbName: {config.DBName} connectionString: {connectionString}");
			for (int i = 0; i < taskCount; ++i)
			{
				DBTaskQueue taskQueue = ComponentFactory.Create<DBTaskQueue>();
				this.tasks.Add(taskQueue);
			}
		}
		
        public string GetConnectionString(DBConfig cfg,Options option=null)
        {
            string connectionString = cfg.ConnectionString;
            if (option !=null && option.RuntimeMode == 1)
            { 
                //暂定原始配置ip都是127.0.0.1
                IPAddress address;
                //目前dotnet2.X在linux下域名访问支持不太好,这里手动域名转ip
                try
                {
                    var list = Dns.GetHostAddresses(cfg.ConnectionHostname);
                    if (list != null && list.Length > 0)
                    {
                        address = list[0];
                        connectionString = connectionString.Replace("127.0.0.1", address.ToString());
                        Log.Debug("mongo连接域名: " + cfg.ConnectionHostname + "对应IP列表: " + string.Join<IPAddress>(",", list));
                    }
                }
                catch (Exception e)
                {
                    Log.Error("mongo连接域名无效: " + e.ToString());
                }
            }
            return connectionString;
        }


        public IMongoCollection<ComponentWithId> GetCollection(string name)
		{
			return this.database.GetCollection<ComponentWithId>(name);
		}

		public ETTask Add(ComponentWithId component, string collectionName = "")
		{
			ETTaskCompletionSource tcs = new ETTaskCompletionSource();
			if (string.IsNullOrEmpty(collectionName))
			{
				collectionName = component.GetType().Name;
			}
			DBSaveTask task = ComponentFactory.CreateWithId<DBSaveTask, ComponentWithId, string, ETTaskCompletionSource>(component.Id, component, collectionName, tcs);
			this.tasks[(int)((ulong)task.Id % taskCount)].Add(task);

			return tcs.Task;
		}

		public ETTask AddBatch(List<ComponentWithId> components, string collectionName)
		{
			ETTaskCompletionSource tcs = new ETTaskCompletionSource();
			DBSaveBatchTask task = ComponentFactory.Create<DBSaveBatchTask, List<ComponentWithId>, string, ETTaskCompletionSource>(components, collectionName, tcs);
			this.tasks[(int)((ulong)task.Id % taskCount)].Add(task);
			return tcs.Task;
		}

		public ETTask<ComponentWithId> Get(string collectionName, long id)
		{
			ETTaskCompletionSource<ComponentWithId> tcs = new ETTaskCompletionSource<ComponentWithId>();
			DBQueryTask dbQueryTask = ComponentFactory.CreateWithId<DBQueryTask, string, ETTaskCompletionSource<ComponentWithId>>(id, collectionName, tcs);
			this.tasks[(int)((ulong)id % taskCount)].Add(dbQueryTask);

			return tcs.Task;
		}

		public ETTask<List<ComponentWithId>> GetBatch(string collectionName, List<long> idList)
		{
			ETTaskCompletionSource<List<ComponentWithId>> tcs = new ETTaskCompletionSource<List<ComponentWithId>>();
			DBQueryBatchTask dbQueryBatchTask = ComponentFactory.Create<DBQueryBatchTask, List<long>, string, ETTaskCompletionSource<List<ComponentWithId>>>(idList, collectionName, tcs);
			this.tasks[(int)((ulong)dbQueryBatchTask.Id % taskCount)].Add(dbQueryBatchTask);

			return tcs.Task;
		}
		
		public ETTask<List<ComponentWithId>> GetJson(string collectionName, string json)
		{
			ETTaskCompletionSource<List<ComponentWithId>> tcs = new ETTaskCompletionSource<List<ComponentWithId>>();
			
			DBQueryJsonTask dbQueryJsonTask = ComponentFactory.Create<DBQueryJsonTask, string, string, ETTaskCompletionSource<List<ComponentWithId>>>(collectionName, json, tcs);
			this.tasks[(int)((ulong)dbQueryJsonTask.Id % taskCount)].Add(dbQueryJsonTask);

			return tcs.Task;
		}

        public ETTask<long> GetDeleteJson(string collectionName, string json)
        {
            ETTaskCompletionSource<long> tcs = new ETTaskCompletionSource<long>();

            DBDeleteJsonTask dbDeleteJsonTask = ComponentFactory.Create<DBDeleteJsonTask, string, string, ETTaskCompletionSource<long>>(collectionName, json, tcs);
            this.tasks[(int)((ulong)dbDeleteJsonTask.Id % taskCount)].Add(dbDeleteJsonTask);
            return tcs.Task;
        }


        public ETTask<List<ComponentWithId>> GetJson(string[] strs, int count)
        {
            ETTaskCompletionSource<List<ComponentWithId>> tcs = new ETTaskCompletionSource<List<ComponentWithId>>();

            DBSortQueryJsonTask dbSortQueryJsonTask = ComponentFactory.Create<DBSortQueryJsonTask, string[], int, ETTaskCompletionSource<List<ComponentWithId>>>(strs, count, tcs);
            this.tasks[(int)((ulong)dbSortQueryJsonTask.Id % taskCount)].Add(dbSortQueryJsonTask);
            return tcs.Task;
        }
    }
}
