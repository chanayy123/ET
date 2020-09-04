﻿using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson.Serialization.Attributes;

namespace ETModel
{
	[BsonIgnoreExtraElements]
	public class Entity : ComponentWithId
	{
		[BsonElement("C")]
		[BsonIgnoreIfNull]
		private HashSet<Component> components = new HashSet<Component>();

		[BsonIgnore]
		private Dictionary<Type, Component> componentDict = new Dictionary<Type, Component>();

		public Entity()
		{
		}

		protected Entity(long id): base(id)
		{
		}

		public override void Dispose()
		{
			if (this.IsDisposed)
			{
				return;
			}
			
			base.Dispose();
			
			foreach (Component component in this.componentDict.Values)
			{
				try
				{
					component.Dispose();
				}
				catch (Exception e)
				{
					Log.Error(e);
				}
			}
			
			this.components.Clear();
			this.componentDict.Clear();
		}
		
        public virtual Component AddSingletonComponent<K>() where K : Component, new()
        {
            return AddComponent(Singleton<K>.Create(this));
        }

		public virtual Component AddComponent(Component component)
		{
			Type type = component.GetType();
			if (this.componentDict.ContainsKey(type))
			{
				throw new Exception($"AddComponent, component already exist, id: {this.Id}, component: {type.Name}");
			}
			
			component.Parent = this;

			this.componentDict.Add(type, component);

			if (component is ISerializeToEntity)
			{
				this.components.Add(component);
			}
			
			return component;
		}

		public virtual Component AddComponent(Type type)
		{
			if (this.componentDict.ContainsKey(type))
			{
				throw new Exception($"AddComponent, component already exist, id: {this.Id}, component: {type.Name}");
			}

			Component component = ComponentFactory.CreateWithParent(type, this, this.IsFromPool);

			this.componentDict.Add(type, component);
			
			if (component is ISerializeToEntity)
			{
				this.components.Add(component);
			}
			
			return component;
		}

		public virtual K AddComponent<K>() where K : Component, new()
		{
			Type type = typeof (K);
			if (this.componentDict.ContainsKey(type))
			{
				throw new Exception($"AddComponent, component already exist, id: {this.Id}, component: {typeof(K).Name}");
			}

			K component = ComponentFactory.CreateWithParent<K>(this, this.IsFromPool);

			this.componentDict.Add(type, component);
			
			if (component is ISerializeToEntity)
			{
				this.components.Add(component);
			}
			
			return component;
		}

		public virtual K AddComponent<K, P1>(P1 p1) where K : Component, new()
		{
			Type type = typeof (K);
			if (this.componentDict.ContainsKey(type))
			{
				throw new Exception($"AddComponent, component already exist, id: {this.Id}, component: {typeof(K).Name}");
			}

			K component = ComponentFactory.CreateWithParent<K, P1>(this, p1, this.IsFromPool);
			
			this.componentDict.Add(type, component);
			
			if (component is ISerializeToEntity)
			{
				this.components.Add(component);
			}
			
			return component;
		}

		public virtual K AddComponent<K, P1, P2>(P1 p1, P2 p2) where K : Component, new()
		{
			Type type = typeof (K);
			if (this.componentDict.ContainsKey(type))
			{
				throw new Exception($"AddComponent, component already exist, id: {this.Id}, component: {typeof(K).Name}");
			}

			K component = ComponentFactory.CreateWithParent<K, P1, P2>(this, p1, p2, this.IsFromPool);
			
			this.componentDict.Add(type, component);
			
			if (component is ISerializeToEntity)
			{
				this.components.Add(component);
			}
			
			return component;
		}

		public virtual K AddComponent<K, P1, P2, P3>(P1 p1, P2 p2, P3 p3) where K : Component, new()
		{
			Type type = typeof (K);
			if (this.componentDict.ContainsKey(type))
			{
				throw new Exception($"AddComponent, component already exist, id: {this.Id}, component: {typeof(K).Name}");
			}

			K component = ComponentFactory.CreateWithParent<K, P1, P2, P3>(this, p1, p2, p3, this.IsFromPool);
			
			this.componentDict.Add(type, component);
			
			if (component is ISerializeToEntity)
			{
				this.components.Add(component);
			}
			
			return component;
		}

		public virtual void RemoveComponent<K>() where K : Component
		{
			if (this.IsDisposed)
			{
				return;
			}
			Type type = typeof (K);
			Component component;
			if (!this.componentDict.TryGetValue(type, out component))
			{
				return;
			}

			this.componentDict.Remove(type);
			this.components.Remove(component);

			component.Dispose();
		}

		public virtual void RemoveComponent(Type type)
		{
			if (this.IsDisposed)
			{
				return;
			}
			Component component;
			if (!this.componentDict.TryGetValue(type, out component))
			{
				return;
			}

			this.componentDict.Remove(type);
			this.components.Remove(component);

			component.Dispose();
		}
        /// <summary>
        /// 老版:获取K类型匹配的组件,不包含子类
        /// 新版: 获取K类型以及K子类型匹配的组件
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <returns></returns>
		public K GetComponent<K>() where K : Component
        {
            foreach (var item in componentDict)
            {
                if (item.Value is K k)
                {
                    return k;
                }
            }
            return null;
		}

		public Component GetComponent(Type type)
		{
			Component component;
			if (!this.componentDict.TryGetValue(type, out component))
			{
				return null;
			}
			return component;
		}

		public Component[] GetComponents()
		{
			return this.componentDict.Values.ToArray();
		}
		
		public override void EndInit()
		{
			try
			{
				base.EndInit();
				
				this.componentDict.Clear();

				if (this.components != null)
				{
					foreach (Component component in this.components)
					{
						component.Parent = this;
						this.componentDict.Add(component.GetType(), component);
					}
				}
			}
			catch (Exception e)
			{
				Log.Error(e);
			}
		}
	}
}