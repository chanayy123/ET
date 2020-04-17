using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace ETModel
{
    [ObjectSystem]
    public class DBSortQueryJsonTaskAwakeSystem : AwakeSystem<DBSortQueryJsonTask, string[], int, ETTaskCompletionSource<List<ComponentWithId>>>
    {
        public override void Awake(DBSortQueryJsonTask self, string[] strs, int count, ETTaskCompletionSource<List<ComponentWithId>> tcs)
        {
            self.CollectionName = strs[0];
            self.QueryJson = strs[1];
            self.SortJson = strs[2];
            self.Count = count;
            self.Tcs = tcs;
        }
    }

    public sealed class DBSortQueryJsonTask : DBTask
    {
        public string CollectionName { get; set; }

        public string QueryJson { get; set; }

        public string SortJson { get; set; }

        public int Count { get; set; }

        public ETTaskCompletionSource<List<ComponentWithId>> Tcs { get; set; }

        public override async ETTask Run()
        {
            DBComponent dbComponent = Game.Scene.GetComponent<DBComponent>();
            try
            {
                FilterDefinition<ComponentWithId> filterDefinition = new JsonFilterDefinition<ComponentWithId>(this.QueryJson);
                SortDefinition<ComponentWithId> sortDefinition = new JsonSortDefinition<ComponentWithId>(this.SortJson);
                IFindFluent<ComponentWithId, ComponentWithId> ifindiluent = dbComponent.GetCollection(this.CollectionName).Find(filterDefinition).Sort(sortDefinition).Limit(this.Count);
                List<ComponentWithId> components = await ifindiluent.ToCursor().ToListAsync();
                this.Tcs.SetResult(components);
            }
            catch (Exception e)
            {
                this.Tcs.SetException(new Exception($"查询数据库异常! {CollectionName} {this.QueryJson}", e));
            }
        }
    }
}