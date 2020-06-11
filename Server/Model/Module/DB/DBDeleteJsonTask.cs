using System;
using System.Collections.Generic;
using MongoDB.Driver;

namespace ETModel
{
	[ObjectSystem]
	public class DBDeleteJsonTaskAwakeSystem : AwakeSystem<DBDeleteJsonTask, string, string, ETTaskCompletionSource<long>>
	{
		public override void Awake(DBDeleteJsonTask self, string collectionName, string json, ETTaskCompletionSource<long> tcs)
		{
			self.CollectionName = collectionName;
			self.Json = json;
			self.Tcs = tcs;
		}
	}

	public sealed class DBDeleteJsonTask : DBTask
	{
		public string CollectionName { get; set; }

		public string Json { get; set; }

		public ETTaskCompletionSource<long> Tcs { get; set; }
		
		public override async ETTask Run()
		{
			DBComponent dbComponent = Game.Scene.GetComponent<DBComponent>();
			try
			{
				// 执行查询数据库任务
				FilterDefinition<ComponentWithId> filterDefinition = new JsonFilterDefinition<ComponentWithId>(this.Json);
                DeleteResult result = await dbComponent.GetCollection(this.CollectionName).DeleteManyAsync(filterDefinition);            
                this.Tcs.SetResult(result.DeletedCount);
			}
			catch (Exception e)
			{
				this.Tcs.SetException(new Exception($"删除数据库文档异常! {CollectionName} {this.Json}", e));
			}
		}
	}
}